using Domain.Common;

namespace Domain.ValueObjects;

public class Isbn : ValueObject
{
    public Isbn(string rawValue)
    {
        RawValue = rawValue;
    }

    public string RawValue { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return RawValue;
    }
}