using CarstenFuehrmann.Monads;
using System;

namespace CarstenFuehrmann.MyLanguage
{
    public static class Compiler
    {
        public static Computation<int> Compile(this Expression expression)
        {
            var plus = expression as Plus;

            if (plus != null)
            {
                return
                    from l in plus.Left.Compile()
                    from r in plus.Right.Compile()
                    select l + r;
            }

            var div = expression as Div;

            if (div != null)
            {
                return
                    from n in div.Numerator.Compile()
                    from d in div.Denominator.Compile()
                    select n / d;
            }

            var literal = expression as Literal;

            if (literal != null)
                return literal.Value.ToComputation();

            var message = string.Format("Unrecognized expression of type {0}!", expression.GetType().Name);
            throw new Exception(message);
        }
    }
}