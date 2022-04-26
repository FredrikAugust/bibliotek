using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.Property(e => e.Id).HasConversion<string>();
    }
}