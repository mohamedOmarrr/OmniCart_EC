using E_commerce_application.Interfaces;
using E_commerce_domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_infrastructure.Repos;

public class OrderRepository(
    AppDbContext context)
    : Repository<Order>(context), IOrderRepository
{
    public async Task<Order?> GetByPaymentTransactionIdAsync(
        string paymentTransactionId,
        CancellationToken cancellationToken)
    {
        return await context.Set<Order>()
            .FirstOrDefaultAsync(
                order => order.PaymentTransactionId == paymentTransactionId,
                cancellationToken);
    }


    public async Task<IReadOnlyList<Order?>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await context.Set<Order>()
            .Where(
                order => order.UserId.ToString() == userId).OrderByDescending(order => order.CreatedAt)
            .ToListAsync(cancellationToken);
   
    }
    public async Task<IReadOnlyList<Order?>> GetByOrderStatusAsync(string orderStatus, CancellationToken cancellationToken)
    {
        return await context.Set<Order>()
            .Where(
                order => order.Status.ToString() == orderStatus).OrderByDescending(order => order.CreatedAt)
            .ToListAsync(cancellationToken);
   
    }
}