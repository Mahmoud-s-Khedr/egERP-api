using System.ComponentModel.DataAnnotations;

public class WareHouse : BaseEntity{


    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    [Required]
    [MaxLength(255)]
    public string Address { get; set; }
    [Required]
    [MaxLength(255)]
    public string PhoneNumber { get; set; }
}