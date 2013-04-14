namespace CarstenFuehrmann.MyLanguage
{
    public class Plus : Expression
    {
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }

        public Plus(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }
    }
}
