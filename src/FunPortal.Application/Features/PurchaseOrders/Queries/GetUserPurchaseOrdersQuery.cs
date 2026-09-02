using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Mappers;
using MediatR;

namespace FunPortal.Application.Features.PurchaseOrders.Queries;

public record GetUserPurchaseOrdersQuery(int UserId)
    : IRequest<IReadOnlyCollection<PurchaseOrderResponse>>;

public class GetUserPurchaseOrdersQueryHandler(
    IPurchaseOrderRepository purchaseOrderRepository)
    : IRequestHandler<GetUserPurchaseOrdersQuery, IReadOnlyCollection<PurchaseOrderResponse>>
{
    public async Task<IReadOnlyCollection<PurchaseOrderResponse>> Handle(
        GetUserPurchaseOrdersQuery query,
        CancellationToken cancellationToken)
    {
        var orders = await purchaseOrderRepository
            .GetByUserIdAsync(query.UserId, cancellationToken);

        return orders.ToPurchaseOrderResponses();
    }
}
