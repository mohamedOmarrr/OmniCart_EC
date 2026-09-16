using E_commerce_domain.Entities;

namespace E_commerce_application.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByPaymentTransactionIdAsync(
        string paymentTransactionId,
        CancellationToken cancellationToken);
    
    Task<IReadOnlyList<Order?>> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Order?>> GetByOrderStatusAsync(string OrderStatus, CancellationToken cancellationToken);
}