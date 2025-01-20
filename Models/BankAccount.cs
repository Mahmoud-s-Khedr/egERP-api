
using System.ComponentModel.DataAnnotations;
namespace EG_ERP.Models;

public class BankAccount : BaseEntity
{
    [Required]
    [StringLength(255)]
    public required string BankName { get; set; }

    [Required]
    [StringLength(255)]
    public required string AccountNumber { get; set; }
}