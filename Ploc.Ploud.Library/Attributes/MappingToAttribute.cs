using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public class MappingToAttribute : Attribute
    {
        public MappingToAttribute(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; private set; }
    }
}
