using System;
using EG_ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace EG_ERP.Data.Repos;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    private readonly AppDbContext context;
    private readonly DbSet<Payment> dbSet;

    public PaymentRepository(AppDbContext context) : base(context)
    {
        this.context = context;
        dbSet = context.Payments;
    }

    public async Task<Payment?> GetByPayroll(string id, bool trackChanges = true, string[]? includes = null)
    {
        IQueryable<Payment> query = dbSet;
        if (!trackChanges)
            query = query.AsNoTracking();
        
        includes ??= Array.Empty<string>();
        foreach (string include in includes)
            query = query.Include(include);
        query = query.Include(p => p.PayrollPayroll).ThenInclude(p => p!.Payroll);

        query = query.Where(p => p.PayrollPayroll != null && p.PayrollPayroll.Payroll != null && p.PayrollPayroll.Payroll.Uuid == id);
        return await query.SingleOrDefaultAsync();
    }
}
