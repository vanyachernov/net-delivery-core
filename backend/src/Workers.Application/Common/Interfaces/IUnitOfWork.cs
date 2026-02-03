namespace Workers.Application.Common.Interfaces;



public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
}
    