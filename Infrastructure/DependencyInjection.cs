using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

// ReSharper disable once UnusedType.Global
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationContext>();

        services.AddScoped<IApplicationContext>(provider => provider.GetRequiredService<ApplicationContext>());
    }
}