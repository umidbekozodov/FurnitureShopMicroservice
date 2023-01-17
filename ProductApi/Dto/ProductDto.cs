using ProductApi.Entities;

namespace ProductApi.Dto;

public class ProductDto
{
    public string? Name { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
    public CategoryDto? Category { get; set; }
    public BrandDto? Brand { get; set; }
}

public class BrandDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class CategoryDto
{
    public string? Name { get; set; }
}