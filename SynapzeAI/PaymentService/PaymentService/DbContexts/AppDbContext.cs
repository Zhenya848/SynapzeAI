using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.DbContexts;

public class AppDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<PaymentSession>  PaymentSessions => Set<PaymentSession>();
    public DbSet<Product> Products => Set<Product>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); 

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(b => b.AddConsole());
}