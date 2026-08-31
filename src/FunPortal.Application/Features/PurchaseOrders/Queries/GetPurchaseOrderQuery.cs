using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Mappers;
using MediatR;

namespace FunPortal.Application.Features.PurchaseOrders.Queries;

public record GetPurchaseOrderQuery(int PurchaseOrderId) : IRequest<PurchaseOrderResponse?>;

public class GetPurchaseOrderQueryHandler(IPurchaseOrderRepository purchaseOrderRepository) : IRequestHandler<GetPurchaseOrderQuery, PurchaseOrderResponse?>
{
    public async Task<PurchaseOrderResponse?> Handle(GetPurchaseOrderQuery query, CancellationToken cancellationToken)
    {
        var order = await purchaseOrderRepository
            .GetByIdAsync(query.PurchaseOrderId, cancellationToken);
        
        if (order == null)
            return null;

        return order.ToPurchaseOrderResponse();
    }
}