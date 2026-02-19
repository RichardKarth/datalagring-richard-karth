
using System.Text.RegularExpressions;

namespace CourseHub.Domain.Students.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    private static readonly Regex EmailRegex =
    new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email is required.", nameof (value));
        }
        var trimmed = value.Trim();
        if(!EmailRegex.IsMatch(trimmed))
        {
            throw new ArgumentException("Invalid email format.", nameof(trimmed));
        }
        Value = trimmed.ToLowerInvariant();
    }
    





}
