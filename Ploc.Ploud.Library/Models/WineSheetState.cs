using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public enum WineSheetState
    {
        None = 0,
        
        Pending = 1,
        
        Cancelling = 2,
        
        Canceled = 4,
        
        Completed = 8,
        
        Sending = 16
    }
}
