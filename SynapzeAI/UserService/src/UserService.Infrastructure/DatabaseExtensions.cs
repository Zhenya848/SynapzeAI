using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.Infrastructure;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        AccountsDbContext dbContext = scope.ServiceProvider.GetRequiredService<AccountsDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}