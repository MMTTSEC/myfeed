using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyFeed.Infrastructure.Data;

/// <summary>
/// Design-time factory for EF Core tools (migrations). Uses SQLite database file under Infrastructure/Database.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=Database/myfeed.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}

