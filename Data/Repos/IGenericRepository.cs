using System;
using EG_ERP.Models;

namespace EG_ERP.Data.Repos;

public interface IGenericRepository<TEnitity>
    where TEnitity : BaseEntity
{
    public Task<List<TEnitity>> GetAll();
    public Task<TEnitity?> GetById(int id, bool trackChanges = true, string includeProperties = "");
    public Task Add(TEnitity entity);
    public Task Update(TEnitity entity);
    public Task Delete(TEnitity entity);
    public Task Delete(int id);
}
