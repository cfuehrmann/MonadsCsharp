namespace CarstenFuehrmann.MyLanguage
{
    public class Literal : Expression
    {
        public int Value { get; private set; }

        public Literal(int value)
        {
            Value = value;
        }
    }
}
