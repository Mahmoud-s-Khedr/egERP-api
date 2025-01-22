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

    public GenericRepository(AppDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<TEnitity>();
    }

    public async Task<List<TEnitity>> GetAll(bool trackChanges = true, string[]? includes = null)
    {
        IQueryable<TEnitity> query = dbSet;
        if (!trackChanges)
            query = query.AsNoTracking();

        includes ??= Array.Empty<string>();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    public async Task<TEnitity?> GetById(int id, bool trackChanges = true, string[]? includes = null)
    {
        IQueryable<TEnitity> query = dbSet;
        if (!trackChanges)
            query = query.AsNoTracking();

        includes ??= Array.Empty<string>();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.SingleOrDefaultAsync(e => e.Id == id);
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
            throw new Exception($"Entity with id not found");
    }

    public async Task Update(TEnitity entity)
    {
        try
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new Exception($"Error Occur While Updating Entity {entity.Uuid}", ex);
        }
    }
}
