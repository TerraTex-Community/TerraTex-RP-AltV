using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using AltV.Net;
using Microsoft.Extensions.Logging;
using TerraTex_RolePlay_AltV_Server.Database.Entities;

namespace TerraTex_RolePlay_AltV_Server.Database;

public class TerraTexDatabaseContext : DbContext 
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // default = local test data
        String host = "localhost";
        String user = "root";
        String password = "Asdf123!";
        String databaseName = "TerraTex_RolePlay";

        if (Alt.Core != null)
        {
            host = Alt.Core.GetServerConfig().Get("TerraTex").Get("Database").Get("host").GetString()!;
            user = Alt.Core.GetServerConfig().Get("TerraTex").Get("Database").Get("user").GetString()!;
            password = Alt.Core.GetServerConfig().Get("TerraTex").Get("Database").Get("password").GetString()!;
            databaseName = Alt.Core.GetServerConfig().Get("TerraTex").Get("Database").Get("database-name").GetString()!;
        }


        // Replace with your connection string.
        var connectionString = $"server={host};user={user};password={password};database={databaseName}";

        // Replace with your server version and type.
        // Use 'MariaDbServerVersion' for MariaDB.
        // Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
        // For common usages, see pull request #1233.
        // var serverVersion = new MariaDbServerVersion(new Version(10, 10, 2));
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        optionsBuilder.UseMySql(connectionString, serverVersion)
            // The following three options help with debugging, but should
            // be changed or removed for production.
            .LogTo(Console.WriteLine, LogLevel.Information)
            // .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        AddTimestamps();
        return base.SaveChangesAsync();
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow; // current datetime

            if (entity.State == EntityState.Added)
            {
                ((BaseEntity)entity.Entity).CreatedAt = now;
            }
            ((BaseEntity)entity.Entity).UpdatedAt = now;
        }
    }
}