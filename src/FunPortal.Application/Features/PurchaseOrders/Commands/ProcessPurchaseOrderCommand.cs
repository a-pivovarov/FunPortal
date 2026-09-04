using FunPortal.Application.DTOs.Enums;
using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Features.PurchaseOrders.Commands.Processing;
using FunPortal.Application.Interfaces;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities;
using FunPortal.Domain.Entities.Products;

using MediatR;
using Microsoft.Extensions.Logging;

namespace FunPortal.Application.Features.PurchaseOrders.Commands;

public record ProcessPurchaseOrderCommand(CreatePurchaseOrderRequest Request)
    : IRequest<PurchaseOrderResponse>;

public class ProcessPurchaseOrderCommandHandler(
    IIdentityContext identityContext,
    IPurchaseOrderRepository purchaseOrderRepository,
    IUserRepository userRepository,
    IProductRepository productRepository,
    IPurchaseOrderProcessor purchaseOrderProcessor,
    IUnitOfWork unitOfWork,
    ILogger<ProcessPurchaseOrderCommandHandler> logger)
    : IRequestHandler<ProcessPurchaseOrderCommand, PurchaseOrderResponse>
{
    public async Task<PurchaseOrderResponse> Handle(
        ProcessPurchaseOrderCommand command,
        CancellationToken cancellationToken)
    {
        var userId = identityContext.UserId;

        // Validate user
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null || !user.IsActive || user.Role != Domain.Enums.UserRole.Admin)
            throw new UnauthorizedAccessException("User is not authorized to place orders");

        // Validate all products exist and retrieve them
        var productDict = new Dictionary<int, Product>();
        foreach (var item in command.Request.Items)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId, cancellationToken)
                ?? throw new KeyNotFoundException($"Product with ID {item.ProductId} not found");

            productDict[item.ProductId] = product;
        }

        // Create purchase order
        var purchaseOrder = CreatePurchaceOrder(
            command.Request.Items,
            productDict,
            userId);

        // Process the purchase order (save, apply business rules, etc.)
        return await ProcessPurchaseOrderAsync(
            purchaseOrder,
            productDict,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new purchase order based on the provided items, product dictionary, and user ID.
    /// </summary>
    /// <param name="items">The items to include in the purchase order.</param>
    /// <param name="productDict">A dictionary of product IDs to product entities.</param>
    /// <param name="userId">The ID of the user placing the order.</param>
    /// <returns>The created purchase order.</returns>
    private static PurchaseOrder CreatePurchaceOrder(
        IReadOnlyCollection<OrderItemDto> items,
        Dictionary<int, Product> productDict,
        int userId)
    {
        // Create purchase order
        var purchaseOrder = new PurchaseOrder
        {
            UserId = userId,
            OrderedOn = DateTime.UtcNow,
            Status = Domain.Enums.OrderStatus.Processing,
            ItemLines = []
        };

        // Calculate total price and populate item lines
        decimal totalPrice = 0;
        foreach (var item in items)
        {
            var product = productDict[item.ProductId];
            var lineTotal = product.Price * item.Quantity;
            totalPrice += lineTotal;

            purchaseOrder.ItemLines.Add(new OrderItemLine
            {
                ProductId = item.ProductId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = item.Quantity
            });
        }

        purchaseOrder.TotalPrice = totalPrice;

        return purchaseOrder;
    }

    /// <summary>
    /// Processes the purchase order by saving it, applying business rules, and returning a response.
    /// </summary>
    /// <param name="purchaseOrder">The purchase order to process.</param>
    /// <param name="productDict">A dictionary of product IDs to product entities.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The response containing details of the processed purchase order.</returns>
    private async Task<PurchaseOrderResponse> ProcessPurchaseOrderAsync(
        PurchaseOrder purchaseOrder,
        Dictionary<int, Product> productDict,
        CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Save purchase order
            var savedOrder = purchaseOrderRepository.Add(purchaseOrder);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Create processing context
            var context = new OrderProcessingContext
            {
                Order = savedOrder,
                Products = productDict
            };

            // Execute all business rules via the processor
            await purchaseOrderProcessor.ProcessAsync(context, cancellationToken);

            // Save changes and commit transaction
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            // Return response
            return new PurchaseOrderResponse
            {
                PurchaseOrderId = savedOrder.PurchaseOrderId,
                UserId = savedOrder.UserId,
                TotalPrice = savedOrder.TotalPrice,
                OrderedOn = savedOrder.OrderedOn,
                Status =  (OrderStatus)savedOrder.Status,
                ActivatedMembershipsCount = context.ActivatedMembershipsCount,
                GeneratedShippingSlipsCount = context.GeneratedShippingSlipsCount,
                Items = [.. savedOrder
                    .ItemLines
                    .Select(il => new OrderItemLineDto
                    {
                        OrderItemLineId = il.OrderItemLineId,
                        ProductId = il.ProductId,
                        ProductName = il.ProductName,
                        Price = il.Price,
                        Quantity = il.Quantity
                    })]
            };
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "An error occurred while processing the purchase order. Transaction rolled back.");
            throw;
        }
    }
}
