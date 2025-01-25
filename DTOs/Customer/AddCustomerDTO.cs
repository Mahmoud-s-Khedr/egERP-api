

using System.ComponentModel.DataAnnotations;

public class AddCustomerDTO{
    [Required]
    public string Name {get; set;}
    [Required]
    public string Address {get; set;}
    [Required]
    public string Phone {get; set;}

    [EmailAddress]
    public string Email {get; set;}
}