using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueRange
{
    public abstract class Range<T> where T : IComparable<T>
    {
        public abstract bool Includes(T value);
        public abstract bool OverlapsWith(Range<T> other);
        public abstract bool OverlapsWith(SingleRange<T> other);
        public abstract Range<T> Add(Range<T> other);
        public virtual Range<T> Subtract(Range<T> other) { return other.Complement; }
        public abstract Range<T> Intersect(Range<T> other);
        public abstract Range<T> Complement { get; }
        public abstract Range<T> GreaterThan(T value);
        public abstract Range<T> GreaterThanOrEquals(T value);
        public abstract Range<T> LessThan(T value);
        public abstract Range<T> LessThanOrEquals(T value);

        public abstract bool IsEmpty { get; }
        public abstract bool IsUniversal { get; }

        protected class EmptyRange : Range<T>
        {
            public static readonly EmptyRange Instance = new EmptyRange();
            private EmptyRange() { }

            public override bool Includes(T value) { return false; }
            public override bool OverlapsWith(Range<T> other) { return false; }
            public override bool OverlapsWith(SingleRange<T> other) { return false; }
            public override Range<T> Add(Range<T> other) { return other; }
            public override Range<T> Subtract(Range<T> other) { return this; }
            public override Range<T> Intersect(Range<T> other) { return this; }
            public override Range<T> Complement { get { return UniversalRange.Instance; } }

            public override Range<T> GreaterThan(T value) { return this; }
            public override Range<T> GreaterThanOrEquals(T value) { return this; }
            public override Range<T> LessThan(T value) { return this; }
            public override Range<T> LessThanOrEquals(T value) { return this; }

            public override bool IsEmpty { get { return true; } }
            public override bool IsUniversal { get { return false; } }
        }

        protected class UniversalRange : Range<T>
        {
            public static readonly UniversalRange Instance = new UniversalRange();
            private UniversalRange() { }

            public override bool Includes(T value) { return true; }
            public override bool OverlapsWith(Range<T> other) { return true; }
            public override bool OverlapsWith(SingleRange<T> other) { return true; }
            public override Range<T> Add(Range<T> other) { return this; }
            public override Range<T> Intersect(Range<T> other) { return other; }
            public override Range<T> Complement { get { return EmptyRange.Instance; } }

            public override Range<T> GreaterThan(T value) { return SingleRange<T>.ThatGreaterThan(value); }
            public override Range<T> GreaterThanOrEquals(T value) { return SingleRange<T>.ThatGreaterThanOrEquals(value); }
            public override Range<T> LessThan(T value) { return SingleRange<T>.ThatLessThan(value); }
            public override Range<T> LessThanOrEquals(T value) { return SingleRange<T>.ThatLessThanOrEquals(value); }

            public override bool IsEmpty { get { return false; } }
            public override bool IsUniversal { get { return true; } }
        }
    }
}
