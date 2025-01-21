using System;
using System.Threading.Tasks;
using EG_ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace EG_ERP.Data.Repos;

public class GenericRepository<TEnitity> : IGenericRepository<TEnitity>
    where TEnitity : BaseEntity
{
    private readonly AppDbContext context;
    private readonly DbSet<TEnitity> dbSet;

    public GenericRepository(AppDbContext context, DbSet<TEnitity> dbSet)
    {
        this.context = context;
        this.dbSet = dbSet;
    }

    public async Task<List<TEnitity>> GetAll()
    {
        return await dbSet.ToListAsync();
    }

    public async Task<TEnitity?> GetById(int id, bool trackChanges = true, string includeProperties = "")
    {
        if (trackChanges)
            return await dbSet.FindAsync(id);
        else
            return await dbSet.AsNoTracking().Include("Manager").SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task Add(TEnitity entity)
    {
        await dbSet.AddAsync(entity);
    }

    public async Task Delete(TEnitity entity)
    {
        dbSet.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        TEnitity? entity = await GetById(id);
        if (entity != null)
            await Delete(entity);
        else
            throw new Exception($"Entity with id {id} not found");
    }

    public async Task Update(TEnitity entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

public async Task UpdateWithoutTracker(TEnitity entity)
{
    var existingEntity = await dbSet.AsNoTracking().SingleOrDefaultAsync(e => e.Id == entity.Id);
    if (existingEntity != null)
    {
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }
    else
    {
        throw new Exception($"Entity with id {entity.Id} not found");
    }
}
}
