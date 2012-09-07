using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueRange;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BoundValue<int> u1 = new BoundValue<int>.UpperBoundValue(1);
            BoundValue<int> u2 = new BoundValue<int>.UpperBoundValue(2);
            BoundValue<int> u3 = new BoundValue<int>.UpperBoundValue(3);
            BoundValue<int> l2 = new BoundValue<int>.LowerBoundValue(2);
            BoundValue<int> pi = new BoundValue<int>.PositiveInfiniteValue();
            BoundValue<int> ni = new BoundValue<int>.NegativeInfiniteValue();

            Console.Out.WriteLine("^1 < v2 == " + (u1 < l2));
            Console.Out.WriteLine("^2 < v2 == " + (u2 < l2));
            Console.Out.WriteLine("^3 < v2 == " + (u3 < l2));
            Console.Out.WriteLine();

            Console.Out.WriteLine("^1 > v2 == " + (u1 > l2));
            Console.Out.WriteLine("^2 > v2 == " + (u2 > l2));
            Console.Out.WriteLine("^3 > v2 == " + (u3 > l2));
            Console.Out.WriteLine();

            Console.Out.WriteLine("^1 >= v2 == " + (u1 >= l2));
            Console.Out.WriteLine("^2 >= v2 == " + (u2 >= l2));
            Console.Out.WriteLine("^3 >= v2 == " + (u3 >= l2));
            Console.Out.WriteLine();

            Console.Out.WriteLine("^1 == v2 == " + (u1 == l2));
            Console.Out.WriteLine("^2 == v2 == " + (u2 == l2));
            Console.Out.WriteLine("^3 == v2 == " + (u3 == l2));
            Console.Out.WriteLine();

            Console.Out.WriteLine("+∞ > v2 == " + (pi > l2));
            Console.Out.WriteLine("^2 > +∞ == " + (u2 > pi));
            Console.Out.WriteLine("-∞ > v2 == " + (ni > l2));
            Console.Out.WriteLine("-∞ > +∞ == " + (ni > pi));
            Console.Out.WriteLine("-∞ < +∞ == " + (ni < pi));
            Console.Out.WriteLine();

            Console.In.ReadLine();
        }
    }
}
