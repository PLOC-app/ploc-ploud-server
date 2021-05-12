using System;

namespace Ploc.Ploud.Library
{
    public class DataStoreAttribute : Attribute
    {
        public DataStoreAttribute(String name)
            : this(name, false)
        {
        }

        public DataStoreAttribute(String name, bool encrypted)
            : this(name, encrypted, false)
        {
        }

        public DataStoreAttribute(String name, bool encrypted, bool primaryKey)
        {
            this.Name = name;
            this.IsEncrypted = encrypted;
            this.IsPrimaryKey = primaryKey;
        }

        public String Name { get; private set; }

        public bool IsEncrypted { get; private set; }

        public bool IsPrimaryKey { get; private set; }
    }
}
