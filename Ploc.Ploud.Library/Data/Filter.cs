using System;

namespace Ploc.Ploud.Library
{
    public sealed class Filter
    {
        public String Column { get; set; }

        public FilterType Type { get; set; }

        public ExpressionType Expression { get; set; }

        public Object Value { get; set; }
    }
}
