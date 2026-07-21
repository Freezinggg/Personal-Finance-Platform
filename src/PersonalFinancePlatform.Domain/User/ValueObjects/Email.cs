using PersonalFinancePlatform.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Domain.User.ValueObjects
{
    public sealed class Email
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvariantViolationException("[Email] cannot be empty.");

            value = value.Trim();

            if (value.Length > 320)
                throw new InvariantViolationException("[Email] is too long.");

            // TODO:
            // Validate format

            Value = value.ToLowerInvariant();
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
        {
            //Validate if object is Email or not, make it secure
            if (obj is not Email other)
                return false;

            return Value == other.Value;
        }

        public override int GetHashCode() => Value.GetHashCode();
    }
}
