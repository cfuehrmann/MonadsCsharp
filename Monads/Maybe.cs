namespace CarstenFuehrmann.Monads
{
    public abstract class Maybe<T>
    {
        public static Maybe<T> Nothing
        {
            get { return new Nothing<T>(); }
        }
    }
}
