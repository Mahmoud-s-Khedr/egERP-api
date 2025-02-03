

using System.ComponentModel.DataAnnotations;

public class AddCustomerDTO
{
    [Required]
    public required string Name {get; set;}

    public string? Address {get; set;}
    public string? Phone {get; set;}

    [EmailAddress]
    public string? Email {get; set;}
}
