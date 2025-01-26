using System;
using EG_ERP.Models;

namespace EG_ERP.Data.Repos;

public interface IPayrollRepository : IGenericRepository<Payroll>
{
    Task<List<Payroll>> GetByEmployee(int id, bool trackChanges = true, string[]? includes = null);
    Task<List<Payroll>> GetByEmployee(string id, bool trackChanges = true, string[]? includes = null);
}
