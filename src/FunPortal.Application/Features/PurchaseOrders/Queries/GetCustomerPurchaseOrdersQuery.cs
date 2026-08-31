using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Mappers;
using MediatR;

namespace FunPortal.Application.Features.PurchaseOrders.Queries;

public record GetCustomerPurchaseOrdersQuery(int CustomerId)
    : IRequest<IReadOnlyCollection<PurchaseOrderResponse>>;

public class GetCustomerPurchaseOrdersQueryHandler(
    IPurchaseOrderRepository purchaseOrderRepository)
    : IRequestHandler<GetCustomerPurchaseOrdersQuery, IReadOnlyCollection<PurchaseOrderResponse>>
{
    public async Task<IReadOnlyCollection<PurchaseOrderResponse>> Handle(
        GetCustomerPurchaseOrdersQuery query,
        CancellationToken cancellationToken)
    {
        var orders = await purchaseOrderRepository
            .GetByCustomerIdAsync(query.CustomerId, cancellationToken);

        return orders.ToPurchaseOrderResponses();
    }
}
