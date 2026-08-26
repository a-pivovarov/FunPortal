using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.PurchaseOrders.Processing;
using FunPortal.Domain.Entities;
using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;
using MediatR;

namespace FunPortal.Application.PurchaseOrders.Commands;

public record ProcessPurchaseOrderCommand(CreatePurchaseOrderRequest Request)
    : IRequest<PurchaseOrderResponse>;

public class ProcessPurchaseOrderCommandHandler(
    IPurchaseOrderRepository purchaseOrderRepository,
    ICustomerRepository customerRepository,
    IProductRepository productRepository,
    IPurchaseOrderProcessor purchaseOrderProcessor,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ProcessPurchaseOrderCommand, PurchaseOrderResponse>
{
    public async Task<PurchaseOrderResponse> Handle(ProcessPurchaseOrderCommand command, CancellationToken cancellationToken)
    {
        // Validate customer exists
        var customerExists = await customerRepository.ExistsAsync(command.Request.CustomerId, cancellationToken);
        if (!customerExists)
            throw new KeyNotFoundException($"Customer with ID {command.Request.CustomerId} not found");

        // Validate all products exist and retrieve them
        var productDict = new Dictionary<int, Product>();
        foreach (var item in command.Request.Items)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {item.ProductId} not found");
            productDict[item.ProductId] = product;
        }

        // Create purchase order
        var purchaseOrder = new PurchaseOrder
        {
            CustomerId = command.Request.CustomerId,
            OrderedOn = DateTime.UtcNow,
            Status = OrderStatus.Processing,
            ItemLines = []
        };

        // Calculate total price and populate item lines
        decimal totalPrice = 0;
        foreach (var item in command.Request.Items)
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
                CustomerId = savedOrder.CustomerId,
                TotalPrice = savedOrder.TotalPrice,
                OrderedOn = savedOrder.OrderedOn,
                Status = savedOrder.Status,
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
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
