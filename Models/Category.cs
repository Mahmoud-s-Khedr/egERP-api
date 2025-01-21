using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
namespace EG_ERP.Models;

public class Category : BaseEntity
{
    [StringLength(255,MinimumLength =1,ErrorMessage = "Name must be between 1 and 255 characters")]
    [Required]
    public required string Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
