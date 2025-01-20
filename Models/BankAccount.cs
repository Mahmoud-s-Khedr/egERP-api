
using System.ComponentModel.DataAnnotations;

public class BankAccount : BaseEntity
{
    
    [Required]
    [MaxLength(255)]
    public string BankName { get; set; }
    [Required]
    [MaxLength(255)]
    public string AccountNumber { get; set; }
}