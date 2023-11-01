using System;

namespace Ploc.Ploud.Library
{
    public class ExpressionValueAttribute : Attribute
    {
        public ExpressionValueAttribute(String value)
        {
            this.Value = value;
        }

        public String Value { get; private set; }
    }
}
