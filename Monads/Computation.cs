using System;

namespace CarstenFuehrmann.Monads
{
    public class Computation<T>
    {
        public Func<T> Code { get; private set; }

        public Computation(Func<T> code)
        {
            Code = code;
        }
    }
}

