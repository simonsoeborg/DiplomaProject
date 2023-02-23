﻿namespace ClassLibrary.EFModels;

public partial class SubCategory
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public virtual Category Category { get; set; } = null!;
}