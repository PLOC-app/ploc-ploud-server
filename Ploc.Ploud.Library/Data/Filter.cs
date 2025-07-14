namespace Ploc.Ploud.Library
{
    public sealed class Filter
    {
        public string Column { get; set; }

        public FilterType Type { get; set; }

        public ExpressionType Expression { get; set; }

        public object Value { get; set; }
    }
}
