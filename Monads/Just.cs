namespace CarstenFuehrmann.Monads
{
    internal sealed class Just<T> : Maybe<T>
    {
        private readonly T _value;

        public T Value
        {
            get { return _value; }
        }

        public Just(T value)
        {
            _value = value;
        }
    }
}