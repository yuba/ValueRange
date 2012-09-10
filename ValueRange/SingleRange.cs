using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueRange
{
    public class SingleRange<T> : Range<T> where T : IComparable<T>
    {
        readonly BoundValue<T> lower, upper;

        private SingleRange(BoundValue<T> lower, BoundValue<T> upper)
        {
            this.lower = lower;
            this.upper = upper;
        }

        public static SingleRange<T> ThatGreaterThan(T value)
        {
            return new SingleRange<T>(BoundValue<T>.UpperBound(value), BoundValue<T>.PositiveInfinity);
        }

        public static SingleRange<T> ThatGreaterThanOrEquals(T value)
        {
            return new SingleRange<T>(BoundValue<T>.LowerBound(value), BoundValue<T>.PositiveInfinity);
        }

        public static SingleRange<T> ThatLessThan(T value)
        {
            return new SingleRange<T>(BoundValue<T>.NegativeInfinity, BoundValue<T>.LowerBound(value));
        }

        public static SingleRange<T> ThatLessThanOrEquals(T value)
        {
            return new SingleRange<T>(BoundValue<T>.NegativeInfinity, BoundValue<T>.UpperBound(value));
        }

        public override bool Includes(T value)
        {
            return lower <= BoundValue<T>.UpperBound(value) && BoundValue<T>.LowerBound(value) <= upper;
        }

        public override bool OverlapsWith(Range<T> other)
        {
            return other.OverlapsWith(this);
        }

        public override bool OverlapsWith(SingleRange<T> other)
        {
            return lower <= other.upper && other.lower <= upper;
        }

        public override Range<T> Add(Range<T> other)
        {
            throw new NotImplementedException();
        }

        public override Range<T> Intersect(Range<T> other)
        {
            throw new NotImplementedException();
        }

        public override Range<T> Complement
        {
            get { throw new NotImplementedException(); }
        }

        public override Range<T> GreaterThan(T value)
        {
           var newBound = BoundValue<T>.UpperBound(value);
            if (upper < newBound) return EmptyRange.Instance;
            return new SingleRange<T>(newBound.Max(lower), upper);
        }

        public override Range<T> GreaterThanOrEquals(T value)
        {
            var newBound = BoundValue<T>.LowerBound(value);
            if (upper < newBound) return EmptyRange.Instance;
            return new SingleRange<T>(newBound.Max(lower), upper);
        }

        public override Range<T> LessThan(T value)
        {
            var newBound = BoundValue<T>.LowerBound(value);
            if (newBound < lower) return EmptyRange.Instance;
            return new SingleRange<T>(lower, newBound.Min(upper));
        }

        public override Range<T> LessThanOrEquals(T value)
        {
            var newBound = BoundValue<T>.UpperBound(value);
            if (newBound < lower) return EmptyRange.Instance;
            return new SingleRange<T>(lower, newBound.Min(upper));
        }

        public override bool IsEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsUniversal
        {
            get { throw new NotImplementedException(); }
        }

        public override string ToString()
        {
            string result = "";

            if (lower is BoundValue<T>.LowerBoundValue)
            {
                result +=  "[" + lower.ToString();
            }
            else
            {
                result += "(" + lower.ToString();
            }

            result += ",";

            if (upper is BoundValue<T>.UpperBoundValue)
            {
                result += upper.ToString() + "]";
            }
            else
            {
                result += upper.ToString() + ")";
            }

            return result;
        }
    }
}
