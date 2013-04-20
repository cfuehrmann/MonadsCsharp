using System;

namespace CarstenFuehrmann.Monads
{
    public static class Maybe
    {
        public static Maybe<TResult> Bind<TSource, TResult>(
           this Maybe<TSource> source,
           Func<TSource, Maybe<TResult>> function)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (function == null) throw new ArgumentNullException("function");

            var just = source as Just<TSource>;

            return just != null ? function(just.Value) : new Nothing<TResult>();
        }

        public static Maybe<T> Just<T>(this T value)
        {
            return new Just<T>(value);
        }

        public static Maybe<TResult> SelectMany<TSource, TOther, TResult>(
            this Maybe<TSource> source, 
            Func<TSource, Maybe<TOther>> monadFunction,
            Func<TSource, TOther, TResult> resultFunction)
        {
            return
                source.Bind(sourceValue =>
                monadFunction(sourceValue).Bind(otherValue =>
                resultFunction(sourceValue, otherValue).Just()));
        }

        public static Maybe<TResult> Select<TSource, TResult>
            (this Maybe<TSource> source, Func<TSource, TResult> function)
        {
            return
                source.Bind(sourceValue =>
                function(sourceValue).Just());
        }

        public static TResult Match<TSource, TResult>(
           this Maybe<TSource> source, Func<TSource, TResult> justBranch,
           Func<TResult> nothingBranch)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (justBranch == null) throw new ArgumentNullException("justBranch");
            if (nothingBranch == null) throw new ArgumentNullException("nothingBranch");

            var just = source as Just<TSource>;

            return just != null ? justBranch(just.Value) : nothingBranch();
        }

        public static void If<T>(this Maybe<T> source, Action<T> justBranch, Action nothingBranch)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (justBranch == null) throw new ArgumentNullException("justBranch");
            if (nothingBranch == null) throw new ArgumentNullException("nothingBranch");

            var just = source as Just<T>;

            if (just != null)
                justBranch(just.Value);
            else
                nothingBranch();
        }
    }
}
