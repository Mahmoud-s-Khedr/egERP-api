using System;
using EG_ERP.Models;

namespace EG_ERP.Data.Repos;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<Payment?> GetByPayroll(string id, bool trackChanges = true, string[]? includes = null);
}
