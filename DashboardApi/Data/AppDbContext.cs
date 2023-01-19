using Microsoft.EntityFrameworkCore;

namespace DashboardApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product>? Productlar { get; set; }
}

public class Product
{
    public long Id { get; set; }
    public string? ProductId { get; set; }
    public string? ProductName { get; set; }
    public int ProductCount { get; set; }
}