using PersonalFinancePlatform.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PersonalFinancePlatform.Domain.User.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvariantViolationException("[Email] cannot be empty.");

            value = value.Trim();

            if (value.Length > 320)
                throw new InvariantViolationException("[Email] is too long.");

            if (!EmailRegex.IsMatch(value))
                throw new InvariantViolationException("[Email] format is invalid.");

            Value = value.ToLowerInvariant();
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
