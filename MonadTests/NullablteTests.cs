using CarstenFuehrmann.Monads;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarstenFuehrmann.MonadTests
{
    [TestClass]
    public class NullableTests
    {
        [TestMethod]
        public void BindBothHaveValue()
        {
            var actual =
                new int?(42).Bind(x =>
                new double?(x * 1.5));

            var expected = new double?(63);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BindFirstIsNull()
        {
            var actual =
                default(int?).Bind(x =>
                new double?(x * 1.5));

            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void BindSecondIsNull()
        {
            var actual =
                new int?(42).Bind(x =>
                default(double?));

            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void BindBothAreNull()
        {
            var actual =
                 default(int?).Bind(x =>
                 default(double?));

            Assert.AreEqual(null, actual);
        }

        // When the argument of Bind has a side effect, we leave the behavior of Bind undefined

        [TestMethod]
        public void JustBehavior()
        {
            var actual = 42.Just();
            var expected = new int?(42);
            Assert.AreEqual(expected, actual);
        }

        // The definition of Select and SelectMany in terms of Bind and Just looks the same
        // for all monads and could be generated automatically. So writing unit tests for 
        // Select and SelectMany seems conceptually wrong. The following tests are just
        // for show.

        [TestMethod]
        public void SelectValue()
        {
            var actual =
// ReSharper disable RedundantExplicitNullableCreation
                from x in new int?(42)
// ReSharper restore RedundantExplicitNullableCreation
                select x;

            var expected = new int?(42);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FromNull()
        {
            var actual =
                from x in default(int?)
                select x;

            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void FromBothHaveValue()
        {
            var actual =
// ReSharper disable RedundantExplicitNullableCreation
                from x in new int?(42)
// ReSharper restore RedundantExplicitNullableCreation
                from y in new double?(x * 1.5)
                select x + y;

            Assert.AreEqual(105, actual);
        }

        [TestMethod]
        public void FromThreeDoesNotCompile()
        {
            // this does not work, because the type parameter of Nullable must be a value type,
            // and the translation into a chain of SelectMany's contains a "new { a, b }"!
            //var actual =
            //    from x in new int?(42)
            //    from y in new double?(x * 1.5)
            //    from z in new double?(x * 1.5)
            //    select x + y + z;
        }

        // Todo: 
        // - Introduce the MayBe-monad to fix the FromThree-problem
        // - Introduce the possibility of casts between Nullable and MayBe
        // - Are there methods missing. That is, do Bind and Just cover all
        // - things that one might want to do with an element of MayBe<T>?
        // - Introduce version control
    }
}
