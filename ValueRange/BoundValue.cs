using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueRange
{
    public abstract class BoundValue<T> : IComparable<BoundValue<T>> where T : IComparable<T>
    {
        public static BoundValue<T> UpperBound(T value) { return new UpperBoundValue(value); }
        public static BoundValue<T> LowerBound(T value) { return new LowerBoundValue(value); }
        public static readonly BoundValue<T> PositiveInfinity = new PositiveInfiniteValue();
        public static readonly BoundValue<T> NegativeInfinity = new NegativeInfiniteValue();

        public abstract int CompareTo(BoundValue<T> other);

        public static bool operator <(BoundValue<T> left, BoundValue<T> right) { return left.CompareTo(right) < 0; }
        public static bool operator >(BoundValue<T> left, BoundValue<T> right) { return left.CompareTo(right) > 0; }
        public static bool operator >=(BoundValue<T> left, BoundValue<T> right) { return left.CompareTo(right) >= 0; }
        public static bool operator <=(BoundValue<T> left, BoundValue<T> right) { return left.CompareTo(right) <= 0; }
        public static bool operator ==(BoundValue<T> left, BoundValue<T> right) { return left.CompareTo(right) == 0; }
        public static bool operator !=(BoundValue<T> left, BoundValue<T> right) { return left.CompareTo(right) != 0; }

        public BoundValue<T> Max(BoundValue<T> right) { return this > right ? this : right; }
        public BoundValue<T> Min(BoundValue<T> right) { return this < right ? this : right; }

        public abstract override bool Equals(object obj);
        public abstract override int GetHashCode();

        public abstract int CompareWith(UpperBoundValue other);
        public abstract int CompareWith(LowerBoundValue other);
        public virtual int CompareWith(NegativeInfiniteValue other) { return -1; }
        public virtual int CompareWith(PositiveInfiniteValue other) { return 1; }

        /// <summary>
        /// ある数【以下】か【より大きい】かを表す、下方向とつながった数
        /// </summary>
        public class UpperBoundValue : BoundValue<T>
        {
            public readonly T value;
            public UpperBoundValue(T value) { this.value = value; }
            public override int CompareTo(BoundValue<T> other) { return other.CompareWith(this); }

            public override int CompareWith(UpperBoundValue other) { return other.value.CompareTo(value); }
            public override int CompareWith(LowerBoundValue other) { return other.value.CompareTo(value) > 0 ? 1 : -1; }

            public override bool Equals(object obj) { return obj is UpperBoundValue && ((UpperBoundValue)obj).value.Equals(value); }
            public override int GetHashCode() { return value.GetHashCode(); }

            public override string ToString() { return value.ToString(); }
        }

        /// <summary>
        /// ある数【以上】か【未満】かを表す、上方向とつながった数
        /// </summary>
        public class LowerBoundValue : BoundValue<T>
        {
            public readonly T value;
            public LowerBoundValue(T value) { this.value = value; }
            public override int CompareTo(BoundValue<T> other) { return other.CompareWith(this); }

            public override int CompareWith(UpperBoundValue other) { return other.value.CompareTo(value) >= 0 ? 1 : -1; }
            public override int CompareWith(LowerBoundValue other) { return other.value.CompareTo(value); }

            public override bool Equals(object obj) { return obj is LowerBoundValue && ((LowerBoundValue)obj).value.Equals(value); }
            public override int GetHashCode() { return ~value.GetHashCode(); }

            public override string ToString() { return value.ToString(); }
        }

        public class NegativeInfiniteValue : BoundValue<T>
        {
            public override int CompareTo(BoundValue<T> other) { return other.CompareWith(this); }

            public override int CompareWith(UpperBoundValue other) { return 1; }
            public override int CompareWith(LowerBoundValue other) { return 1; }
            public override int CompareWith(NegativeInfiniteValue other) { return 0; }

            public override bool Equals(object obj) { return obj is NegativeInfiniteValue; }
            public override int GetHashCode() { return typeof(NegativeInfiniteValue).GetHashCode(); }

            public override string ToString() { return "-∞"; }
        }


        public class PositiveInfiniteValue : BoundValue<T>
        {
            public override int CompareTo(BoundValue<T> other) { return other.CompareWith(this); }

            public override int CompareWith(UpperBoundValue other) { return -1; }
            public override int CompareWith(LowerBoundValue other) { return -1; }
            public override int CompareWith(PositiveInfiniteValue other) { return 0; }

            public override bool Equals(object obj) { return obj is PositiveInfiniteValue; }
            public override int GetHashCode() { return typeof(PositiveInfiniteValue).GetHashCode(); }

            public override string ToString() { return "∞"; }
        }
    }
}