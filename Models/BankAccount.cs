
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace EG_ERP.Models;

[Index(nameof(AccountNumber), IsUnique = true)]

public class BankAccount : BaseEntity
{
    [Required]
    [StringLength(255)]
    public required string BankName { get; set; }

    [StringLength(255)]
    public required string AccountNumber { get; set; }
}
