using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using AltV.Net;
using Microsoft.Extensions.Logging;

namespace TerraTex_RolePlay_AltV_Server.Database;

public class TerraTexDatabaseContext : DbContext 
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        String host = Alt.Core.GetServerConfig().Get("TerraTex").Get("Database").Get("host").GetString();
        String user = Alt.Core.GetServerConfig().Get("TerraTex").Get("Database").Get("user").GetString();
        String password = Alt.Core.GetServerConfig().Get("TerraTex").Get("Database").Get("password").GetString();
        String databaseName = Alt.Core.GetServerConfig().Get("TerraTex").Get("Database").Get("database-name").GetString();


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
}