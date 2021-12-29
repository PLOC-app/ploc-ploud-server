using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public enum RackLegendType
    {
        None = 1,

        BothLetter = 2,

        BothNumeric = 4,

        LetterOnXNumericOnY = 8,

        NumericOnXLetterOnY = 16
    }
}
