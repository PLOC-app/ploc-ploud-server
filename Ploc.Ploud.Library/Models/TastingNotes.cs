using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("tastingnotes")]
    public class TastingNotes : PloudObject
    {
        [DataStore("wine")]
        public String Wine;

        public Wine GetWine()
        {
            throw new NotImplementedException();
        }

        [DataStore("note")]
        public int Note { get; set; }

        [DataStore("vintage")]
        public int Vintage { get; set; }

        [DataStore("latitude")]
        public double Latitude { get; set; }

        [DataStore("longitude")]
        public double Longitude { get; set; }

        [DataStore("lid")]
        public String LocalTemplateIdentifier { get; set; }

        [DataStore("date")]
        public DateTime When { get; set; }

        [DataStore("pid")]
        public String PlocIdentifier { get; set; }

        [DataStore("wpid")]
        public String WinePlocIdentifier { get; set; }

        [DataStore("source")]
        public int Source { get; set; }

        [DataStore("sourcename")]
        public String SourceName { get; set; }

        [DataStore("spid")]
        public String SourcePlocIdentifier { get; set; }

        [DataStore("ln")]
        public String Language { get; set; }

        [DataStore("image")]
        public String Image { get; set; }

        [DataStore("imagewidth")]
        public int ImageWidth { get; set; }

        [DataStore("imageheight")]
        public int ImageHeight { get; set; }

        [DataStore("r")]
        public int Red { get; set; }

        [DataStore("g")]
        public int Green { get; set; }

        [DataStore("b")]
        public int Blue { get; set; }

        [DataStore("occolor")]
        public int OcColor { get; set; }

        [DataStore("mood")]
        public MoodType Mood { get; set; }

        [DataStore("fields", true)]
        public String Fields { get; set; }
    }
}
