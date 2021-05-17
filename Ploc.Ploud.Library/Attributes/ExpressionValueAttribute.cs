using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
