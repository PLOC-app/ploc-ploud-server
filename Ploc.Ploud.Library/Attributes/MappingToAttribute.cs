using System;

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
