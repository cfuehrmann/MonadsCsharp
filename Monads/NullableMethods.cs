using System;

namespace CarstenFuehrmann.Monads
{
    public static class NullableMethods
    {
        public static Nullable<TResult> Bind<TSource, TResult>(
            this Nullable<TSource> source,
            Func<TSource, Nullable<TResult>> function)
            where TSource : struct
            where TResult : struct
        {
            return source.HasValue ? function(source.Value) : null;
        }

        public static Nullable<TSource> Just<TSource>(this TSource source) where TSource : struct
        {
            return source;
        }

        public static TResult? SelectMany<TSource, TOther, TResult>(
            this TSource? source, Func<TSource, TOther?> monadFunction,
            Func<TSource, TOther, TResult> resultFunction)
            where TSource : struct
            where TOther : struct
            where TResult : struct
        {
            return
                source.Bind(sourceValue =>
                monadFunction(sourceValue).Bind(otherValue =>
                resultFunction(sourceValue, otherValue).Just()));
        }

        public static TResult? Select<TSource, TResult>
            (this TSource? source, Func<TSource, TResult> function)
            where TResult : struct
            where TSource : struct
        {
            return
                source.Bind(sourceValue =>
                function(sourceValue).Just());
        }
    }
}
