using CarstenFuehrmann.Monads;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CarstenFuehrmann.MonadTests
{
    [TestClass]
    public class ComputationTests
    {
        [TestMethod]
        public void TestBind()
        {
            var c1 = new Computation<int>
                 (() =>
                 {
                     Console.WriteLine("a");
                     return 3;
                 });

            Func<int, Computation<int>> c2 = x =>
                 new Computation<int>
                    (() =>
                    {
                        Console.WriteLine("b");
                        return x + 4;
                    });

            Computation<int> c =
                c1.Bind(x =>
                c2(x).Bind(y =>
                (x + y).ToComputation()
                ));

            var result = c.Eval();
        }

        [TestMethod]
        public void TestBind2()
        {
            var c1 = new Computation<int>
                 (() =>
                 {
                     Console.WriteLine("a");
                     return 3;
                 });

            Func<int, Computation<int>> c2 = x =>
                 new Computation<int>
                    (() =>
                    {
                        Console.WriteLine("b");
                        return x + 4;
                    });

            Computation<int> c = c1.Bind(x => c2(x)).Bind(y => (42 + y).ToComputation());
            Computation<int> d = c1.Bind(x => c2(x).Bind(y => (42 + y).ToComputation()));

            Assert.AreEqual(c.Eval(), d.Eval());
        }

        [TestMethod]
        public void TestQueryExpressions()
        {
            var c1 = new Computation<int>
                (() =>
                {
                    Console.WriteLine("a");
                    return 3;
                });

            // This needs Select to be defined:
            Computation<int> c =
                from x in c1
                select x;

            var result = c.Eval();
        }

        [TestMethod]
        public void TestQueryExpressions2()
        {
            var c1 = new Computation<int>
                (() =>
                {
                    Console.WriteLine("a");
                    return 3;
                });

            Func<int, Computation<int>> c2 = x =>
                 new Computation<int>
                    (() =>
                    {
                        Console.WriteLine("b");
                        return x + 4;
                    });

            // This needs SelectMany to be defined
            Computation<int> c =
                from x in c1
                from y in c2(x)
                select x + y;

            var result = c.Eval();
        }
    }
}
