using System.ComponentModel.DataAnnotations;
EG_ERP.Models
class Category : BaseEntity{
    [StringLength(255,MinimumLength =1,ErrorMessage = "Name must be between 1 and 255 characters")]
    public string? Name { get; set; }
}