using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TestsService.Domain;
using Task = TestsService.Domain.Task;

namespace TestsService.Infrastructure.DbContexts;

public class AppDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Test> Tests => Set<Test>();
    public DbSet<Task> Tasks => Set<Task>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly, 
            type => type.FullName?.Contains("Configurations.Write") ?? false); 

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(b => b.AddConsole());
}