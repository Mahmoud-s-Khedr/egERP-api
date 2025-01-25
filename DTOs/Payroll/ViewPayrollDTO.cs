using EG_ERP.DTOs.Product;
using EG_ERP.Utils;
namespace EG_ERP.DTOs.Payroll;

public class ViewPayrollDTO
{
    public string? Id { get;set;}
    public decimal BaseSalary { get; set; }
    public decimal Bonus { get; set; }
    public decimal Deduction { get; set; }
    public decimal Tax { get; set; }
    public Status Status { get; set; }
    public DateOnly PaymentDate { get; set; }

    //public string? EmployeeId { get; set; }
    //public int EmployeeId { get; set; }
    //public ViewEmployeeDTO? Employee { get; set; }

    public IEnumerable<ViewPaymentDTO>? PayrollPayments { get; set; }

}