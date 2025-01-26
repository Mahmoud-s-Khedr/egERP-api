using System.ComponentModel.DataAnnotations;
using EG_ERP.DTOs.Payment;
using EG_ERP.Utils;

namespace EG_ERP.DTOs.Payroll;

public class ViewPayrollDTO
{
    public string? Id { get;set;}
    public decimal BaseSalary { get; set; }
    public decimal Bonus { get; set; }
    public decimal Deduction { get; set; }
    public decimal Tax { get; set; }
    public decimal Total => BaseSalary + Bonus - Deduction - Tax;
    public Status Status { get; set; }
    
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly PaymentDate { get; set; }

    public string? Employee { get; set; }

    public ViewPaymentDTO? Payment { get; set; }
}
