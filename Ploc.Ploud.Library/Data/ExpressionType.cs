namespace Ploc.Ploud.Library
{
    public enum ExpressionType
    {
        [ExpressionValue("=")]
        Equal,

        [ExpressionValue("<>")]
        NotEqual,

        [ExpressionValue("<")]
        LessThan,

        [ExpressionValue("<=")]
        LessThanOrEqual,

        [ExpressionValue(">")]
        GreaterThan,

        [ExpressionValue(">=")]
        GreaterThanOrEqual,
    }
}
