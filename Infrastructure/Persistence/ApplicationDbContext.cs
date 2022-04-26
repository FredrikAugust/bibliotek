using System.Reflection;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }
    
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Rental> Rentals { get; set; } = null!;

    public async Task<int> SaveAsync(CancellationToken cancellationToken = new())
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    /**
     * This makes asp.net find configurations in the assembly, so we can define configurations for models in separate files.
     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}