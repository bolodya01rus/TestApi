using Microsoft.EntityFrameworkCore;
using TestApi.Model;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) => Database.EnsureCreated();
    public DbSet<RequestInfo> RequestInfos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RequestInfo>().HasKey(e => e.Guid);
        base.OnModelCreating(modelBuilder);
    }
}