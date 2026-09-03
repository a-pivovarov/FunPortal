using AutoMapper;
using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Interfaces.Repositories;
using MediatR;

namespace FunPortal.Application.Features.PurchaseOrders.Queries;

public record GetPurchaseOrderQuery(int PurchaseOrderId) : IRequest<PurchaseOrderResponse?>;

public class GetPurchaseOrderQueryHandler(
    IPurchaseOrderRepository purchaseOrderRepository,
    IMapper mapper)
    : IRequestHandler<GetPurchaseOrderQuery, PurchaseOrderResponse?>
{
    public async Task<PurchaseOrderResponse?> Handle(
        GetPurchaseOrderQuery query,
        CancellationToken cancellationToken)
    {
        var order = await purchaseOrderRepository
            .GetByIdAsync(query.PurchaseOrderId, cancellationToken);
        
        if (order == null)
            return null;

        return mapper.Map<PurchaseOrderResponse>(order);
    }
}