using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("wine")]
    public class Wine : PloudObject
    {
        [DataStore("country")]
        public String Country { get; set; }

        public Country GetCountry()
        {
            throw new NotImplementedException();
        }

        [DataStore("region")]
        public String Region { get; set; }

        public Region GetRegion()
        {
            throw new NotImplementedException();
        }

        [DataStore("appellation")]
        public String Appellation { get; set; }

        public Appellation GetAppellation()
        {
            throw new NotImplementedException();
        }

        [DataStore("bottleformat")]
        public String BottleFormat { get; set; }

        public BottleFormat GetBottleFormat()
        {
            throw new NotImplementedException();
        }

        [DataStore("classification")]
        public String Classification { get; set; }

        public Classification GetClassification()
        {
            throw new NotImplementedException();
        }

        [DataStore("color")]
        public String Color { get; set; }

        public Color GetColor()
        {
            throw new NotImplementedException();
        }

        [DataStore("owner")]
        public String Owner { get; set; }

        public Owner GetOwner()
        {
            throw new NotImplementedException();
        }

        [DataStore("pid", true)]
        public String PlocIdentifier { get; set; }

        [DataStore("comments", true)]
        public String Comments { get; set; }

        [DataStore("cuvee")]
        public String Cuvee { get; set; }

        [DataStore("tags", true)]
        public String Tags { get; set; }

        [DataStore("reference", true)]
        public String Reference { get; set; }

        [DataStore("composition")]
        public String Grapes { get; set; }

        [DataStore("vintage")]
        public int Vintage { get; set; }

        [DataStore("note")]
        public int Note { get; set; }

        [DataStore("bottles")]
        public int Bottles { get; set; }

        [DataStore("food")]
        public Meals Meals { get; set; }

        [DataStore("favorite")]
        public bool Favorite { get; set; }

        [DataStore("degree")]
        public float Degree { get; set; }

        [DataStore("estimateprice")]
        public float EstimatePrice { get; set; }

        [DataStore("inventorytype")]
        public int InventoryMode { get; set; }

        [DataStore("apogeefrom")]
        public int ApogeePeriodFrom { get; set; }

        [DataStore("apogeeto")]
        public int ApogeePeriodTo { get; set; }

        [DataStore("consumefrom")]
        public int DrinkPeriodFrom { get; set; }

        [DataStore("consumeto")]
        public int DrinkPeriodTo { get; set; }

        [DataStore("tempmin")]
        public float TemperatureFrom { get; set; }

        [DataStore("tempmax")]
        public float TemperatureTo { get; set; }

        [DataStore("latitude")]
        public double Latitude { get; set; }

        [DataStore("longitude")]
        public double Longitude { get; set; }

        [DataStore("ln")]
        public String Language { get; set; }

        [DataStore("image")]
        public String Image { get; set; }

        [DataStore("imagewidth")]
        public int ImageWidth { get; set; }

        [DataStore("imageheight")]
        public int ImageHeight { get; set; }

        [DataStore("coverimage")]
        public String CoverImage { get; set; }

        [DataStore("coverimagewidth")]
        public int CoverImageWidth { get; set; }

        [DataStore("coverimageheight")]
        public int CoverImageHeight { get; set; }

        [DataStore("app")]
        public int AppChoppe { get; set; }

        [DataStore("appsku")]
        public String Sku { get; set; }

        [DataStore("producturl", true)]
        public String ProductUrl { get; set; }

        [DataStore("notification")]
        public int RemoteNotification { get; set; }

        [DataStore("presheet")]
        public WineSheetState SheetState { get; set; }
    }
}
