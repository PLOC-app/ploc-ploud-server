using System;

namespace Ploc.Ploud.Library
{
    public class MappingToAttribute : Attribute
    {
        public Type Type { get; private set; }

        public MappingToAttribute(Type type)
        {
            this.Type = type;
        }
    }
}
