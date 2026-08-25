using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.PurchaseOrders.Queries;

public record GetCustomerPurchaseOrdersQuery(int CustomerId) : IRequest<IEnumerable<PurchaseOrderResponse>>;

public class GetCustomerPurchaseOrdersQueryHandler : IRequestHandler<GetCustomerPurchaseOrdersQuery, IEnumerable<PurchaseOrderResponse>>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;

    public GetCustomerPurchaseOrdersQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    public async Task<IEnumerable<PurchaseOrderResponse>> Handle(GetCustomerPurchaseOrdersQuery query, CancellationToken cancellationToken)
    {
        var orders = await _purchaseOrderRepository.GetByCustomerIdAsync(query.CustomerId, cancellationToken);

        return orders.Select(o => new PurchaseOrderResponse
        {
            PurchaseOrderId = o.PurchaseOrderId,
            CustomerId = o.CustomerId,
            TotalPrice = o.TotalPrice,
            OrderedOn = o.OrderedOn,
            Status = o.Status,
            Items = o.ItemLines.Select(il => new OrderItemLineDto
            {
                OrderItemLineId = il.OrderItemLineId,
                ProductId = il.ProductId,
                ProductName = il.ProductName,
                Price = il.Price,
                Quantity = il.Quantity
            }).ToList()
        });
    }
}
