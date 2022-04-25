using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationContext
{
    DbSet<Book> Books { get; }
    
    DbSet<Rental> Rentals { get; }

    public Task<int> SaveAsync(CancellationToken cancellationToken);
}