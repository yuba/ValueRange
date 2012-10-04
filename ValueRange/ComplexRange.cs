using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueRange
{
    public partial class Range<T> where T : IComparable<T>
    {
        /// <summary>
        /// 2つ以上の単純範囲をORした範囲を表現するクラス
        /// </summary>
        protected class ComplexRange : Range<T>
        {
            /// <summary>
            /// 長さ2以上に保たれます。
            /// </summary>
            private SingleRange[] elements;

            /// <summary>
            /// 2つ以上の単純範囲からオブジェクトを生成します
            /// </summary>
            /// <param name="elements">
            /// 必ず長さ2以上の配列を指定してください。
            /// </param>
            public ComplexRange(SingleRange[] elements)
            {
                this.elements = new SingleRange[elements.Length];
                Array.Copy(elements, this.elements, elements.Length);
            }

            public override bool Includes(T value)
            {
                return elements.Any(element => element.Includes(value));
            }

            public override bool OverlapsWith(Range<T> other)
            {
                return elements.Any(element => other.OverlapsWith(element));
            }

            protected override bool OverlapsWith(SingleRange other)
            {
                return elements.Any(element => other.OverlapsWith(element));
            }

            public override Range<T> Add(Range<T> other)
            {
                return other.Add(elements);
            }

			protected override Range<T> Add (SingleRange[] otherElements)
			{
				return Create(SingleRange.Add(this.elements, otherElements));
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
                throw new NotImplementedException();
            }

            public override Range<T> GreaterThanOrEquals(T value)
            {
                throw new NotImplementedException();
            }

            public override Range<T> LessThan(T value)
            {
                throw new NotImplementedException();
            }

            public override Range<T> LessThanOrEquals(T value)
            {
                throw new NotImplementedException();
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
                return string.Join(",", elements.Select(element => element.ToString()).ToArray());
            }

			public static Range<T> Create (IEnumerable<SingleRange> ranges)
			{
				var elements = ranges.ToArray ();
				switch (elements.Length) {
				case 0:
					return EmptyRange.Instance;
				case 1:
					return elements [0].IsUniversal ? UniversalRange.Instance as Range<T> : elements[0];
				default:
					return new ComplexRange (elements);
				}
			}
        }
    }
}