using EG_ERP.Data.Repos;
using EG_ERP.Data.UoWs;
using EG_ERP.DTOs.Payroll;
using EG_ERP.Models;
using Microsoft.AspNetCore.Mvc;
using EG_ERP.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EG_ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PayrollsController : ControllerBase
{
    private readonly IUnitOfWork unit;
    private readonly ICalculator calc;

    public PayrollsController(IUnitOfWork unit, ICalculator calc)
    {
        this.unit = unit;
        this.calc = calc;
    }
    [HttpGet]
    public async Task<IActionResult> GetPayrolls() { 
        IGenericRepository<Payroll> repo = unit.GetRepository<Payroll>();
        List<Payroll> payrolls = await repo.GetAll();
        List<ViewPayrollDTO> viewPayrolls = payrolls.Select(p => new ViewPayrollDTO
        {
            Id = p.Uuid,
            BaseSalary = p.BaseSalary,
            Tax = p.Tax,
            Status = p.Status,
            Deduction = p.Deduction,
            Bonus = p.Bonus,
            PaymentDate = p.PaymentDate
        }).ToList();
        return Ok(payrolls);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayroll(CreatePayrollDTO PayrollDTO){
        IGenericRepository<Payroll> repo = unit.GetRepository<Payroll>();
        Payroll payroll = new Payroll
        {
            //EmployeeId = PayrollDTO.EmployeeId,
            Uuid = Guid.NewGuid().ToString(),
            BaseSalary = PayrollDTO.BaseSalary,
            Tax = calc.CalculateTax(PayrollDTO.BaseSalary + PayrollDTO.Bonus - PayrollDTO.Deduction),
            Status = Status.Pending,
            Deduction = PayrollDTO.Deduction,
            Bonus = PayrollDTO.Bonus,
            PaymentDate = DateOnly.FromDateTime(DateTime.Now)
        };
        await repo.Add(payroll);
        await unit.Commit();
        return Ok();
    }

    [HttpPut("{id}/ChangeStatus")]
    public async Task<IActionResult> ChangeStatus(string id, Status status){
        IGenericRepository<Payroll> repo = unit.GetRepository<Payroll>();
        Payroll payroll = await repo.GetById(id);
        payroll.Status = status;
        await repo.Update(payroll);
        await unit.Commit();
        return Ok();
    }

    
}