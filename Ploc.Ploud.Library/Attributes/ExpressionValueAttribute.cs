using System;

namespace Ploc.Ploud.Library
{
    public class ExpressionValueAttribute : Attribute
    {
        public string Value { get; private set; }

        public ExpressionValueAttribute(string value)
        {
            this.Value = value;
        }
    }
}
