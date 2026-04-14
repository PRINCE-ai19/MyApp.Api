using System;
using System.Collections.Generic;

namespace MyApp.Domain.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Code { get; set; }

    public string? RecordStatus { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
