﻿namespace ClassLibrary.EFModels;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();

    public virtual ICollection<SubCategory> SubCategories { get; } = new List<SubCategory>();
}