﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueRange
{
    public class ComplexRange<T> : Range<T> where T:IComparable<T>
    {
        readonly SingleRange<T>[] elements;

        public override bool Includes(T value)
        {
            return elements.Any(element => element.Includes(value));
        }

        public override bool OverlapsWith(Range<T> other)
        {
            return elements.Any(element => other.OverlapsWith(element));
        }

        public override bool OverlapsWith(SingleRange<T> other)
        {
            return elements.Any(element => other.OverlapsWith(element));
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
    }
}
