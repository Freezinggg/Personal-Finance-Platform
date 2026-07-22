using PersonalFinancePlatform.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Domain.User.ValueObjects
{
    public sealed class PasswordHash : ValueObject
    {
        public string Value { get; }

        public PasswordHash(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvariantViolationException("[Password] cannot be empty.");

            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
