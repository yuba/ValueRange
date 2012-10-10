using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueRange
{
    public partial class Range<T> where T : IComparable<T>
    {
        protected class SingleRange : ContiguousRange
        {
            readonly BoundValue<T> lower, upper;

            private SingleRange(BoundValue<T> lower, BoundValue<T> upper)
            {
                this.lower = lower;
                this.upper = upper;
            }

            public static SingleRange ThatGreaterThan(T value)
            {
                return new SingleRange(BoundValue<T>.UpperBound(value), BoundValue<T>.PositiveInfinity);
            }

            public static SingleRange ThatGreaterThanOrEquals(T value)
            {
                return new SingleRange(BoundValue<T>.LowerBound(value), BoundValue<T>.PositiveInfinity);
            }

            public static SingleRange ThatLessThan(T value)
            {
                return new SingleRange(BoundValue<T>.NegativeInfinity, BoundValue<T>.LowerBound(value));
            }

            public static SingleRange ThatLessThanOrEquals(T value)
            {
                return new SingleRange(BoundValue<T>.NegativeInfinity, BoundValue<T>.UpperBound(value));
            }

            public override bool Includes(T value)
            {
                return lower <= BoundValue<T>.UpperBound(value) && BoundValue<T>.LowerBound(value) <= upper;
            }

            public override bool OverlapsWith(Range<T> other)
            {
                return other.OverlapsWith(this);
            }

            protected override bool OverlapsWith(SingleRange other)
            {
                return lower <= other.upper && other.lower <= upper;
            }

            public override Range<T> Add(Range<T> other)
            {
				return other.Add (new [] {this});
			}
			
			protected override Range<T> Add (SingleRange[] otherElements)
			{
				return ComplexRange.Create(Add (new [] {this}, otherElements));
			}

            public override Range<T> Intersect (Range<T> other)
			{
                return other.Intersect(new[] { this });
			}

            protected override Range<T> Intersect(SingleRange[] otherElements)
            {
                return ComplexRange.Create(Intersect(new[] { this }, otherElements));
            }

            public override Range<T> Complement {
				get {
					Range<T> result = EmptyRange.Instance;
					if (!(lower is BoundValue<T>.NegativeInfiniteValue)) {
						result = new SingleRange (BoundValue<T>.NegativeInfinity, lower);
					}
					if (!(upper is BoundValue<T>.PositiveInfiniteValue)) {
						result = result.Add (new SingleRange (upper, BoundValue<T>.PositiveInfinity));
					}
					return result;
				}
			}

            public override Range<T> GreaterThan(T value)
            {
                var newBound = BoundValue<T>.UpperBound(value);
                if (upper < newBound) return EmptyRange.Instance;
                return new SingleRange(newBound.Max(lower), upper);
            }

            public override Range<T> GreaterThanOrEquals(T value)
            {
                var newBound = BoundValue<T>.LowerBound(value);
                if (upper < newBound) return EmptyRange.Instance;
                return new SingleRange(newBound.Max(lower), upper);
            }

            public override Range<T> LessThan(T value)
            {
                var newBound = BoundValue<T>.LowerBound(value);
                if (newBound < lower) return EmptyRange.Instance;
                return new SingleRange(lower, newBound.Min(upper));
            }

            public override Range<T> LessThanOrEquals(T value)
            {
                var newBound = BoundValue<T>.UpperBound(value);
                if (newBound < lower) return EmptyRange.Instance;
                return new SingleRange(lower, newBound.Min(upper));
            }

            public override bool IsEmpty
            {
                get { return false; }
            }

            public override bool IsUniversal
            {
                get { return lower is BoundValue<T>.NegativeInfiniteValue && upper is BoundValue<T>.PositiveInfiniteValue; }
            }

            public override string ToString()
            {
                string result = "";

                if (lower is BoundValue<T>.LowerBoundValue)
                {
                    result += "[" + lower.ToString();
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

			public static SingleRange Merge (SingleRange a, SingleRange b)
			{
				BoundValue<T> newLower = a.lower < b.lower ? a.lower : b.lower;
				BoundValue<T> newUpper = a.upper < b.upper ? b.upper : a.upper;

				return new SingleRange(newLower, newUpper);
			}

			public static IEnumerable<SingleRange> Add (SingleRange[] a, SingleRange[] b)
			{
				SingleRange onStage = null;
				int indexA = 0, indexB = 0;
				
				while (true) {
					if (onStage == null) {

						if (b.Length <= indexB)
						{
							// aにしか残り要素がない
							for(;indexA < a.Length; indexA++) yield return a[indexA];
							break;
						}
						if (a.Length <= indexA)
						{
							// bにしか残り要素がない
							for(;indexB < b.Length; indexB++) yield return b[indexB];
							break;
						}

						// a,bとも残り要素あり
						if (a [indexA].upper < b [indexB].lower)	// aの先頭の方が小さければ、それを流す
						{
							yield return a [indexA];
							indexA++;
						}
						else if (b [indexB].upper < a [indexA].lower)	// bの先頭の方が小さければ、それを流す
						{
							yield return b [indexB];
							indexB++;
						}
						else
						{
							onStage = Merge(a[indexA], b[indexB]);
							indexA++;
							indexB++;
						}
					}
					else 
					{
						if (indexA < a.Length && a[indexA].OverlapsWith(onStage))
						{
							onStage = Merge(a[indexA], onStage);
							indexA++;
						}
						else if (indexB < b.Length && b[indexB].OverlapsWith(onStage))
						{
							onStage = Merge(b[indexB], onStage);
							indexB++;
						}
						else
						{
							yield return onStage;
							onStage = null;
						}
					}
				}

				if (onStage != null) yield return onStage;
			}

            public static SingleRange Intersect(SingleRange a, SingleRange b)
            {
                BoundValue<T> newLower = a.lower < b.lower ? b.lower : a.lower;
                BoundValue<T> newUpper = a.upper < b.upper ? a.upper : b.upper;

                return new SingleRange(newLower, newUpper);
            }

            public static IEnumerable<SingleRange> Intersect(SingleRange[] a, SingleRange[] b)
            {
                int indexA = 0, indexB = 0;

                while (true)
                {
                    if (b.Length <= indexB)
                    {
                        // aにしか残り要素がない
                        break;
                    }
                    if (a.Length <= indexA)
                    {
                        // bにしか残り要素がない
                        break;
                    }

                    // a,bとも残り要素あり
                    if (a[indexA].upper < b[indexB].lower)	// aの先頭の方が小さければ、それを捨てる
                    {
                        indexA++;
                    }
                    else if (b[indexB].upper < a[indexA].lower)	// bの先頭の方が小さければ、それを捨てる
                    {
                        indexB++;
                    }
                    else
                    {
                        yield return Intersect(a[indexA], b[indexB]);   // 双方の先頭の共通部分を返し、
                        if (a[indexA].upper < b[indexB].upper) indexA++; else indexB++; // 背の低い方を捨てる(背が同じなら、残ったA側も次のループで捨てられる)
                    }

                }
            }
        }
	}
}
