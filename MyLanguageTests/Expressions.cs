using CarstenFuehrmann.Monads;
using CarstenFuehrmann.MyLanguage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CarstenFuehrmann.MyLanguageTests
{
    [TestClass]
    public class ExpressionTests
    {
        [TestMethod]
        public void TestCompilerLiteral()
        {
            int result = new Literal(42).Compile().Eval();

            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void TestCompilerPlus()
        {
            int result = new Plus(new Literal(3), new Plus(new Literal(1), new Literal(2))).Compile().Eval();

            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void TestCompilerDiv()
        {
            Computation<int> c = new Div(new Literal(42), new Literal(0)).Compile();

            try
            {
                c.Eval();
            }
            catch (DivideByZeroException)
            {
                return;
            }

            Assert.Fail("Missing DivideByZeroException!");
        }
    }
}
