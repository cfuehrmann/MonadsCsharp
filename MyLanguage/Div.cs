namespace CarstenFuehrmann.MyLanguage
{
    public class Div : Expression
    {
        public Expression Numerator { get; private set; }
        public Expression Denominator { get; private set; }

        public Div(Expression numerator, Expression denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }
    }
}
