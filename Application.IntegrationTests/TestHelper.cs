using System.IO;
using Application.Common.Interfaces;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Respawn;
using Respawn.Graph;
using Serilog;

namespace Application.IntegrationTests;

public class TestHelper
{
    public readonly IServiceScopeFactory ServiceScopeFactory;

    public const string UserId = "test-user-0000000";

    public TestHelper()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName)
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();
        
        var services = new ServiceCollection();
        
        services.AddApplication();
        services.AddInfrastructure(configuration);

        services.AddTransient(_ =>
            Mock.Of<ICurrentUserService>(service => service.UserId == UserId));

        services.AddSingleton(_ => Mock.Of<ILogger>());

        ServiceScopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
        
        EnsureDatabase();
    }

    private void EnsureDatabase()
    {
        using var scope = ServiceScopeFactory.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
    }
}