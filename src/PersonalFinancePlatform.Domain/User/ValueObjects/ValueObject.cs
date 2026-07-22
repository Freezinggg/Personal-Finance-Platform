using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Domain.User.ValueObjects
{
    public abstract class ValueObject
    {
        //Get the value of the object
        protected abstract IEnumerable<object?> GetEqualityComponents();


        //Compare the object type first, then compare the value, not the HASHCODE.
        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            var other = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        //To get value faster and easier so when finding it in dictionary its faster. if theres hash collision (assuming theres 2 hash same), then we use Equals()
        public override int GetHashCode()
        {
            return GetEqualityComponents().Aggregate(0, HashCode.Combine);
        }

        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject? left, ValueObject? right)
        {
            return !Equals(left, right);
        }
    }
}
