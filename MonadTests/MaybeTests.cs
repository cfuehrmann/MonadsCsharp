using System;
using CarstenFuehrmann.Monads;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CarstenFuehrmann.MonadTests
{
    [TestClass]
    public class MaybeTests
    {
        private readonly Func<int, double> _failFunction = _ =>
            {
                Assert.Fail();
                return Math.E;
            };

        private readonly Func<double> _failThunk = () =>
            {
                Assert.Fail();
                return Math.E;
            };

        private readonly Func<int, Maybe<double>> _failMonadFunction = _ =>
            {
                Assert.Fail();
                return Math.E.Just();
            };

        public interface IFunction
        {
            Maybe<string> Evaluate(double x);
        }

        [TestMethod]
        public void BindJust()
        {
            Maybe<double> actual =
                42.Just().Bind(x =>
                x == 42 ? Math.PI.Just() : Math.E.Just());

            var f = Substitute.For<IFunction>();
            actual.Bind(f.Evaluate);
            f.Received().Evaluate(Math.PI);
        }

        [TestMethod]
        public void BindNothing()
        {
            Maybe<double> actual = Maybe<int>.Nothing.Bind(_failMonadFunction);

            var f = Substitute.For<IFunction>();
            actual.Bind(f.Evaluate);
            f.DidNotReceive().Evaluate(Arg.Any<int>());
        }

        [TestMethod]
        public void BindSourceNull()
        {
            Action test = () => (null as Maybe<int>).Bind(_failMonadFunction);
            var e = Throws<ArgumentNullException>(test);

            Assert.AreEqual("source", e.ParamName);
        }

        [TestMethod]
        public void BindFunctionNull()
        {
            Func<int, Maybe<int>> nullFunc = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Action test = () => Maybe<int>.Nothing.Bind(nullFunc);
            // ReSharper restore ExpressionIsAlwaysNull
            var e = Throws<ArgumentNullException>(test);

            Assert.AreEqual("function", e.ParamName);
        }

        [TestMethod]
        public void MatchWithJust()
        {
            double actual = 42.Just().Match(_ => Math.PI, _failThunk);

            Assert.AreEqual(actual, Math.PI);
        }

        [TestMethod]
        public void MatchWithNothing()
        {
            double actual = Maybe<int>.Nothing.Match(_failFunction, () => Math.PI);

            Assert.AreEqual(actual, Math.PI);
        }

        [TestMethod]
        public void MatchWithNullSource()
        {
            Action test = () => (null as Maybe<int>).Match(_failFunction, _failThunk);
            var e = Throws<ArgumentNullException>(test);

            Assert.AreEqual("source", e.ParamName);
        }

        [TestMethod]
        public void MatchWithNullJustBranch()
        {
            Action test = () => Maybe<int>.Nothing.Match(null, _failThunk);
            var e = Throws<ArgumentNullException>(test);

            Assert.AreEqual("justBranch", e.ParamName);
        }

        [TestMethod]
        public void MatchWithNullNothingBranch()
        {
            Action test = () => 42.Just().Match(_failFunction, null);
            var e = Throws<ArgumentNullException>(test);

            Assert.AreEqual("nothingBranch", e.ParamName);
        }

        public interface IBranches<in T>
        {
            void JustBranch(T arg);
            void NothingBranch();
        }

        [TestMethod]
        public void IfWithJust()
        {
            var b = Substitute.For<IBranches<int>>();

            42.Just().If(b.JustBranch, b.NothingBranch);

            b.Received().JustBranch(42);
            b.DidNotReceive().NothingBranch();
        }

        [TestMethod]
        public void IfWithNothing()
        {
            var b = Substitute.For<IBranches<int>>();

            Maybe<int>.Nothing.If(b.JustBranch, b.NothingBranch);

            b.DidNotReceive().JustBranch(Arg.Any<int>());
            b.Received().NothingBranch();
        }

        [TestMethod]
        public void IfWithNullSource()
        {
            var b = Substitute.For<IBranches<int>>();

            Action test = () => (null as Maybe<int>).If(b.JustBranch, b.NothingBranch);
            var e = Throws<ArgumentNullException>(test);

            Assert.AreEqual("source", e.ParamName);
            b.DidNotReceive().JustBranch(Arg.Any<int>());
            b.DidNotReceive().NothingBranch();
        }

        [TestMethod]
        public void IfWithNullJustBranch()
        {
            var b = Substitute.For<IBranches<int>>();

            Action test = () => Maybe<int>.Nothing.If(null, b.NothingBranch);
            var e = Throws<ArgumentNullException>(test);

            Assert.AreEqual("justBranch", e.ParamName);
            b.DidNotReceive().NothingBranch();
        }

        [TestMethod]
        public void IfWithNullNothingBranch()
        {
            var b = Substitute.For<IBranches<int>>();

            Action test = () => 42.Just().If(b.JustBranch, null);
            var e = Throws<ArgumentNullException>(test);

            Assert.AreEqual("nothingBranch", e.ParamName);
            b.DidNotReceive().JustBranch(Arg.Any<int>());
        }

        private T Throws<T>(Action test) where T : Exception
        {
            try
            {
                test();
            }
            catch (T e)
            {
                return e;
            }

            Assert.Fail("Expected {0}, but none was thrown!", typeof(T).Name);
            return null;
        }
    }
}
