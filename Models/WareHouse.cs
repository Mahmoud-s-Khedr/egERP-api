using System.ComponentModel.DataAnnotations;

namespace EG_ERP.Models;
public class WareHouse : BaseEntity
{
    [Required]
    [StringLength(255)]
    public required string Name { get; set; }
    public string? Address { get; set; }
    [StringLength(255)]
    public string? PhoneNumber { get; set; }
}