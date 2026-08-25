using FunPortal.Domain.Entities;

namespace FunPortal.Application.Interfaces.Repositories;

public interface IPurchaseOrderRepository
{
    Task<PurchaseOrder?> GetByIdAsync(int purchaseOrderId, CancellationToken cancellationToken);

    Task<IEnumerable<PurchaseOrder>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken);

    Task<PurchaseOrder> AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken);

    Task<PurchaseOrder> UpdateAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken);
}
