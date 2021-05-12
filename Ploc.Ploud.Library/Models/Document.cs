using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("document")]
    public class Document : PloudObject
    {
        [DataStore("keywords")]
        public String Keywords { get; set; }

        [DataStore("parent")]
        public String Parent { get; set; }

        [DataStore("parenttype")]
        public int ParentType { get; set; }

        [DataStore("tags")]
        public String Tags { get; set; }

        [DataStore("contenttype")]
        public String ContentType { get; set; }

        [DataStore("originalpath")]
        public String OriginalPath { get; set; }

        [DataStore("length")]
        public long Length { get; set; }

        [DataStore("width")]
        public int Width { get; set; }

        [DataStore("height")]
        public int Height { get; set; }

        [DataStore("ordr")]
        public int Order { get; set; }

        [DataStore("type")]
        public int Type { get; set; }

        public byte[] Data { get; set; }

        public void LoadData()
        {
            if(this.Data != null)
            {
                return;
            }
            throw new NotImplementedException();
        }
    }
}
