using System;
using EG_ERP.Data.Repos;
using EG_ERP.Models;

namespace EG_ERP.Data.UoWs;

public interface IUnitOfWork
{
    public IGenericRepository<TEnitity> GetRepository<TEnitity>() where TEnitity : BaseEntity;
    public Task Commit();
    public Task Rollback();
    public ValueTask DisposeAsync();
}
