using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
