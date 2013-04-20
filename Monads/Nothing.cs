namespace CarstenFuehrmann.Monads
{
    internal sealed class Nothing<T> : Maybe<T>
    {
        public override bool Equals(object obj)
        {
            return obj is Nothing<T>;
        }

        public override int GetHashCode()
        {
            return 42;
        }
    }
}
