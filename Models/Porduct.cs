using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EG_ERP.Models;
class Product : BaseEntity{
    [StringLength(255,MinimumLength = 1, ErrorMessage = "")]
    public string? Name { get; set; }
    public string? Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    [ForeignKey("Category")]
    public int CategoryId { get; set; }

}