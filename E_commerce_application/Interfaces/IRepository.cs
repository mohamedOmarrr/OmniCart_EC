using E_commerce_domain.Entities;

namespace E_commerce_application.Interfaces;

using System.Linq.Expressions;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}