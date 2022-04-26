using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();

        var dbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

        if (dbContext == null) return;

        await SeedBooksAsync(dbContext);
    }
    
    private static async Task SeedBooksAsync(IApplicationDbContext applicationDbContext)
    {
        if (applicationDbContext.Books.Any()) return;
        
        var books = new List<Book>
        {
            new ()
            {
                Name = "my cool book",
                Isbn = new Isbn("isbn")
            },
            new ()
            {
                Name = "my cool book 2",
                Isbn = new Isbn("isbn")
            },
            new ()
            {
                Name = "my cool book 3",
                Isbn = new Isbn("isbn")
            },
        };
        
        applicationDbContext.Books.AddRange(books);

        await applicationDbContext.SaveAsync();
    }
}