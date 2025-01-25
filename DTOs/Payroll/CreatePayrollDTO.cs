namespace EG_ERP.DTOs.Payroll;

public class CreatePayrollDTO
{
    public decimal BaseSalary { get; set; }
    public decimal Deduction { get; set; }
    public decimal Bonus { get; set; }

    // public int EmployeeId { get; set; }
    // public string EmployeeId { get; set; }
}
