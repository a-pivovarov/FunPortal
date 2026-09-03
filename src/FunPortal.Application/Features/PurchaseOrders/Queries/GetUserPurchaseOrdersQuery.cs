using AutoMapper;
using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.Features.PurchaseOrders.Queries;

public record GetUserPurchaseOrdersQuery(int UserId)
    : IRequest<IReadOnlyCollection<PurchaseOrderResponse>>;

public class GetUserPurchaseOrdersQueryHandler(
    IPurchaseOrderRepository purchaseOrderRepository,
    IMapper mapper)
    : IRequestHandler<GetUserPurchaseOrdersQuery, IReadOnlyCollection<PurchaseOrderResponse>>
{
    public async Task<IReadOnlyCollection<PurchaseOrderResponse>> Handle(
        GetUserPurchaseOrdersQuery query,
        CancellationToken cancellationToken)
    {
        var orders = await purchaseOrderRepository
            .GetByUserIdAsync(query.UserId, cancellationToken);

        return mapper.Map<IReadOnlyCollection<PurchaseOrderResponse>>(orders);
    }
}
