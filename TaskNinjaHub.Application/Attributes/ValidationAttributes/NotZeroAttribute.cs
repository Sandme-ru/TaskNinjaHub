using System.ComponentModel.DataAnnotations;

namespace TaskNinjaHub.Application.Attributes.ValidationAttributes;

public class NotZeroAttribute : ValidationAttribute
{
    public NotZeroAttribute()
    {
        ErrorMessage = "The {0} field must not be zero.";
    }

    public override bool IsValid(object? value)
    {
        return value switch
        {
            null => false,
            int intValue => intValue != 0,
            long longValue => longValue != 0,
            _ => throw new InvalidOperationException("Attribute not applied to int or long property.")
        };
    }
}