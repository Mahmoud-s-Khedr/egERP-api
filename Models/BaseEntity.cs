using System;

namespace EG_ERP.Models;

public class BaseEntity
{
    public int Id { get; set; }
    public string Uuid { get; set; } = Guid.NewGuid().ToString();
}
