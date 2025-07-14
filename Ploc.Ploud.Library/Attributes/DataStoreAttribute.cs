using System;

namespace Ploc.Ploud.Library
{
    public class DataStoreAttribute : Attribute
    {
        public string Name { get; private set; }

        public bool IsEncrypted { get; private set; }

        public bool IsPrimaryKey { get; private set; }

        public DataStoreAttribute(string name)
            : this(name, false)
        {
        }

        public DataStoreAttribute(string name, bool encrypted)
            : this(name, encrypted, false)
        {
        }

        public DataStoreAttribute(string name, bool encrypted, bool primaryKey)
        {
            this.Name = name;
            this.IsEncrypted = encrypted;
            this.IsPrimaryKey = primaryKey;
        }
    }
}
