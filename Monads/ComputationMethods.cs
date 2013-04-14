using System;
using System.Linq;

namespace CarstenFuehrmann.Monads
{
    public static class ComputationMethods
    {
        public static Computation<TResult> Bind<TSource, TResult>(
            this Computation<TSource> source, 
            Func<TSource, Computation<TResult>> function)
        {
            Func<TResult> code = () =>
            {
                TSource value = source.Eval();
                return function(value).Eval();
            };

            return new Computation<TResult>(code);
        }

        public static Computation<TSource> ToComputation<TSource>(this TSource source)
        {
            return new Computation<TSource>(() => source);
        }

        public static Computation<TResult> Select<TSource, TResult>(
            this Computation<TSource> source, 
            Func<TSource, TResult> function)
            where TResult : struct
            where TSource : struct
        {
            return source.Bind(value => function(value).ToComputation());
        }

        public static Computation<TResult> SelectMany<TSource, TOther, TResult>(
            this Computation<TSource> source,
            Func<TSource, Computation<TOther>> monadFunction,
            Func<TSource, TOther, TResult> resultFunction)
        {
            return
                source.Bind(sourceValue =>
                monadFunction(sourceValue).Bind(otherValue =>
                resultFunction(sourceValue, otherValue).ToComputation()));
        }

        public static T Eval<T>(this Computation<T> c)
        {
            return c.Code();
        }

        // The "big" version of SelectMany above avoids building up brackets:
        public static Computation<Result> SelectMany<TSource, TOther1, TOther2, Result>(
            this Computation<TSource> source,
            Func<TSource, Computation<TOther1>> monadFunction1,
            Func<TSource, TOther1, Computation<TOther2>> monadFunction2,
            Func<TSource, TOther1, TOther2, Result> resultFunction)
        {
            return
                source
                .SelectMany(
                    monadFunction1,
                    (sourceValue, otherValue1) => new { sourceValue, otherValue1 })
                .SelectMany(
                    pair => monadFunction2(pair.sourceValue, pair.otherValue1),
                    (p, otherValue2) => resultFunction(p.sourceValue, p.otherValue1, otherValue2));
        }
    }
}
