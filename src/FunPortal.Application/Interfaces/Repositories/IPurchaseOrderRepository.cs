using FunPortal.Domain.Entities;

namespace FunPortal.Application.Interfaces.Repositories;

public interface IPurchaseOrderRepository
{
    Task<PurchaseOrder?> GetByIdAsync(int purchaseOrderId, CancellationToken cancellationToken);

    Task<IEnumerable<PurchaseOrder>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken);

    PurchaseOrder Add(PurchaseOrder purchaseOrder);

    void Update(PurchaseOrder purchaseOrder);
}
