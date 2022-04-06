using Microsoft.EntityFrameworkCore;
using Balalayka.Data.Models;

namespace Balalayka.Data;

public class BalalaykaDbContext: DbContext {
    public BalalaykaDbContext(DbContextOptions<BalalaykaDbContext> options): base(options) 
    {
    }
    
    public BalalaykaDbContext()
    {
    }
    public DbSet<BalalaykaEntity> Balalaykas { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BalalaykaDbContext).Assembly);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
       
    }
}