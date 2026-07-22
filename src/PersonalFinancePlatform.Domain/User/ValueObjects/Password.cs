using PersonalFinancePlatform.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Domain.User.ValueObjects
{
    public sealed class Password : ValueObject
    {
        public string Value { get; }

        public Password(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new InvariantViolationException("[Password] cannot be empty.");

            value = value.Trim();

            if(value.Length < 8)
                throw new InvariantViolationException("[Password] too short.");

            if (value.Length > 25)
                throw new InvariantViolationException("[Password] too long.");

            if (!value.Any(char.IsUpper))
                throw new InvariantViolationException("[Password] must at least have 1 Upper Case.");

            if (!value.Any(char.IsLower))
                throw new InvariantViolationException("[Password] must at least have 1 Lower Case.");

            if (!value.Any(char.IsDigit))
                throw new InvariantViolationException("[Password] must at least have 1 Number.");

            if (!value.Any(c => !char.IsLetterOrDigit(c)))
                throw new InvariantViolationException("[Password] must at least have 1 Letter or Digit.");

            Value = value;
        }


        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
        
    }
}
