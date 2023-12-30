using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VirtualMenuAPI.Data.Models;
using VirtualMenuAPI.Data.Models.Users;
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Data
{
  public class DataContext : IdentityDbContext<Account>
  {
    //public DataContext()
    //{
    //}

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      builder.Entity<Customer>(entity => { entity.ToTable("Customers"); });
    }
  }
}