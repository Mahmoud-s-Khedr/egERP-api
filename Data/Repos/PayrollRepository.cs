using System;
using EG_ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace EG_ERP.Data.Repos;

public class PayrollRepository : GenericRepository<Payroll>, IPayrollRepository
{
    private readonly AppDbContext context;
    private readonly DbSet<Payroll> dbSet;

    public PayrollRepository(AppDbContext context) : base(context)
    {
        this.context = context;
        dbSet = context.Payrolls;
    }

    public async Task<List<Payroll>> GetByEmployee(int id, bool trackChanges = true, string[]? includes = null)
    {
        IQueryable<Payroll> query = dbSet;
        if (!trackChanges)
            query = query.AsNoTracking();
        
        includes ??= Array.Empty<string>();
        foreach (string include in includes)
            query = query.Include(include);

        query = query.Where(p => p.EmployeeId == id)
                     .Include(p => p.PayrollPayment)
                     .ThenInclude(p => p!.Payment)
                     .OrderByDescending(p => p.PaymentDate);

        return await query.ToListAsync();
    }
    public async Task<List<Payroll>> GetByEmployee(string id, bool trackChanges = true, string[]? includes = null)
    {
        IQueryable<Payroll> query = dbSet;
        if (!trackChanges)
            query = query.AsNoTracking();
        
        includes ??= Array.Empty<string>();
        foreach (string include in includes)
            query = query.Include(include);

        query = query.Include(p => p.Employee)
                     .Where(p => p.Employee != null && p.Employee.Uuid == id)
                     .Include(p => p.PayrollPayment)
                     .ThenInclude(p => p!.Payment)
                     .OrderByDescending(p => p.PaymentDate);

        return await query.ToListAsync();
    }
}
