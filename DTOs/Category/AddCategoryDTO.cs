using System;
using System.ComponentModel.DataAnnotations;

namespace EG_ERP.DTOs.Category;

public class AddCategoryDTO
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 255 characters")]
    public required string Name { get; set; }
}

