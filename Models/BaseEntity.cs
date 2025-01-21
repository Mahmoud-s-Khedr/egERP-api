using System;
using Microsoft.EntityFrameworkCore;

namespace EG_ERP.Models;

[Index(nameof(Uuid), IsUnique = true)]
public class BaseEntity
{
    public int Id { get; set; }
    public string Uuid { get; set; } = Guid.NewGuid().ToString();
}
