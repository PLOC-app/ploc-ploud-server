using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public enum PloudObjectType
    {
        Unknown = 0,

        [MappingTo(typeof(Wine))]
        Wine = 100,

        [MappingTo(typeof(Country))]
        Country = 101,

        [MappingTo(typeof(Region))]
        Region = 102,

        [MappingTo(typeof(Appellation))]
        Appellation = 103,

        [MappingTo(typeof(BottleFormat))]
        BottleFormat = 104,

        [MappingTo(typeof(Owner))]
        Owner = 106,

        [MappingTo(typeof(Vendor))]
        Vendor = 107,

        [MappingTo(typeof(Classification))]
        Classification = 108,

        [MappingTo(typeof(Color))]
        Color = 109,

        [MappingTo(typeof(Grapes))]
        Grapes = 110,

        [MappingTo(typeof(Rack))]
        Rack = 111,

        [MappingTo(typeof(RackItem))]
        RackItem = 112,

        [MappingTo(typeof(Order))]
        Order = 113,

        [MappingTo(typeof(TastingNotes))]
        TastingNotes = 114,

        [MappingTo(typeof(Document))]
        Document = 115,

        [MappingTo(typeof(GlobalParameter))]
        GlobalParameter = 117,
    }
}
