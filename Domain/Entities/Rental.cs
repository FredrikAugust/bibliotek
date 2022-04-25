using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Rental
{
    public Guid Id { get; set; }
    
    public Guid BookId { get; set; }

    public Book Book { get; set; } = null!;
    
    [DataType(DataType.DateTime)]
    public DateTime Start { get; set; } = DateTime.Now;
    
    [DataType(DataType.DateTime)]
    public DateTime? End { get; set; }

    public string UserId { get; set; } = null!;
}