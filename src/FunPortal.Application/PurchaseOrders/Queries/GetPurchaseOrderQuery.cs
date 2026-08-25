using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.PurchaseOrders.Queries;

public record GetPurchaseOrderQuery(int PurchaseOrderId) : IRequest<PurchaseOrderResponse?>;

public class GetPurchaseOrderQueryHandler : IRequestHandler<GetPurchaseOrderQuery, PurchaseOrderResponse?>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;

    public GetPurchaseOrderQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    public async Task<PurchaseOrderResponse?> Handle(GetPurchaseOrderQuery query, CancellationToken cancellationToken)
    {
        var order = await _purchaseOrderRepository.GetByIdAsync(query.PurchaseOrderId, cancellationToken);
        
        if (order == null)
            return null;

        return new PurchaseOrderResponse
        {
            PurchaseOrderId = order.PurchaseOrderId,
            CustomerId = order.CustomerId,
            TotalPrice = order.TotalPrice,
            OrderedOn = order.OrderedOn,
            Status = order.Status,
            Items = order.ItemLines.Select(il => new OrderItemLineDto
            {
                OrderItemLineId = il.OrderItemLineId,
                ProductId = il.ProductId,
                ProductName = il.ProductName,
                Price = il.Price,
                Quantity = il.Quantity
            }).ToList()
        };
    }
}
