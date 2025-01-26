using System;
using System.Threading.Tasks;
using EG_ERP.Data.Repos;
using EG_ERP.Models;

namespace EG_ERP.Data.UoWs;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly AppDbContext db;
    private bool disposed = false;
    public UnitOfWork(AppDbContext _db)
    {
        db = _db;
    }

    private IDictionary<Type, object> repositories = new Dictionary<Type, object>();

    public IGenericRepository<TEnitity> GetRepository<TEnitity>() where TEnitity : BaseEntity
    {
        Type entityType = typeof(TEnitity);
        if (!repositories.ContainsKey(entityType))
        {
            switch (entityType.Name)
            {
                case "Payroll":
                    repositories[entityType] = new PayrollRepository(db);
                    break;
                case "Payment":
                    repositories[entityType] = new PaymentRepository(db);
                    break;
                default:
                    repositories[entityType] = new GenericRepository<TEnitity>(db);
                    break;
            }
        }

        return repositories[entityType] as IGenericRepository<TEnitity> ?? throw new Exception("Repository not found");
    }

    public async Task Commit()
    {
        try
        {
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            //TODO:: Log the exception
            throw new InvalidOperationException("An error occurred while saving changes.", ex);
        }
    }

    public async Task Rollback()
    {
        await db.DisposeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual async Task Dispose(bool disposing)
    {
        if (!disposed && disposing)
            await db.DisposeAsync();
        disposed = true;
    }
}
