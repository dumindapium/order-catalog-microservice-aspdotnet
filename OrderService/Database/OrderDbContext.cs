using Microsoft.EntityFrameworkCore;
using OrderService.Entities;

namespace OrderService.Database;


public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options):
        base(options){ }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<Product> Products { get; set; }
}
