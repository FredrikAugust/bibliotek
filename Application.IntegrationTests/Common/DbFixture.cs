using System.IO;
using Microsoft.Extensions.Configuration;
using Respawn;
using Respawn.Graph;

namespace Application.IntegrationTests.Common;

public class DbFixture
{
    private static readonly Checkpoint Checkpoint = new ()
    {
        TablesToIgnore = new []
        {
            new Table("__EFMigrationsHistory")
        }
    };

    public DbFixture()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName)
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Checkpoint.Reset(connectionString);
    }
}