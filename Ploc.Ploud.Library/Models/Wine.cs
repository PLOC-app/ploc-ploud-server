using System;
using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    [DataStore("wine")]
    public class Wine : PloudObject
    {
        [JsonPropertyName("cn")]
        [DataStore("country")]
        public String Country { get; set; }

        public Country GetCountry()
        {
            throw new NotImplementedException();
        }

        [JsonPropertyName("rn")]
        [DataStore("region")]
        public String Region { get; set; }

        public Region GetRegion()
        {
            throw new NotImplementedException();
        }

        [JsonPropertyName("ap")]
        [DataStore("appellation")]
        public String Appellation { get; set; }

        public Appellation GetAppellation()
        {
            throw new NotImplementedException();
        }

        [JsonPropertyName("vf")]
        [DataStore("bottleformat")]
        public String BottleFormat { get; set; }

        public BottleFormat GetBottleFormat()
        {
            throw new NotImplementedException();
        }

        [JsonPropertyName("cl")]
        [DataStore("classification")]
        public String Classification { get; set; }

        public Classification GetClassification()
        {
            throw new NotImplementedException();
        }

        [JsonPropertyName("co")]
        [DataStore("color")]
        public String Color { get; set; }

        public Color GetColor()
        {
            throw new NotImplementedException();
        }

        [JsonPropertyName("ow")]
        [DataStore("owner")]
        public String Owner { get; set; }

        public Owner GetOwner()
        {
            throw new NotImplementedException();
        }

        [JsonPropertyName("pid")]
        [DataStore("pid", true)]
        public String PlocIdentifier { get; set; }

        [JsonPropertyName("cs")]
        [DataStore("comments", true)]
        public String Comments { get; set; }

        [JsonPropertyName("cu")]
        [DataStore("cuvee")]
        public String Cuvee { get; set; }

        [JsonPropertyName("tg")]
        [DataStore("tags", true)]
        public String Tags { get; set; }

        [JsonPropertyName("re")]
        [DataStore("reference", true)]
        public String Reference { get; set; }

        [JsonPropertyName("gr")]
        [DataStore("composition")]
        public String Grapes { get; set; }

        [JsonPropertyName("vi")]
        [DataStore("vintage")]
        public int Vintage { get; set; }

        [JsonPropertyName("nt")]
        [DataStore("note")]
        public int Note { get; set; }

        [JsonPropertyName("bo")]
        [DataStore("bottles")]
        public int Bottles { get; set; }

        [JsonPropertyName("ml")]
        [DataStore("food")]
        public Meals Meals { get; set; }

        [JsonPropertyName("fa")]
        [DataStore("favorite")]
        public bool Favorite { get; set; }

        [JsonPropertyName("de")]
        [DataStore("degree")]
        public float Degree { get; set; }

        [JsonPropertyName("ep")]
        [DataStore("estimateprice")]
        public float EstimatePrice { get; set; }

        [JsonPropertyName("it")]
        [DataStore("inventorytype")]
        public int InventoryMode { get; set; }

        [JsonPropertyName("af")]
        [DataStore("apogeefrom")]
        public int ApogeePeriodFrom { get; set; }

        [JsonPropertyName("at")]
        [DataStore("apogeeto")]
        public int ApogeePeriodTo { get; set; }

        [JsonPropertyName("cf")]
        [DataStore("consumefrom")]
        public int DrinkPeriodFrom { get; set; }

        [JsonPropertyName("ct")]
        [DataStore("consumeto")]
        public int DrinkPeriodTo { get; set; }

        [JsonPropertyName("ti")]
        [DataStore("tempmin")]
        public float TemperatureFrom { get; set; }

        [JsonPropertyName("to")]
        [DataStore("tempmax")]
        public float TemperatureTo { get; set; }

        [JsonPropertyName("lt")]
        [DataStore("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("lg")]
        [DataStore("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("ln")]
        [DataStore("ln")]
        public String Language { get; set; }

        [JsonPropertyName("im")]
        [DataStore("image")]
        public String Image { get; set; }

        [JsonPropertyName("iw")]
        [DataStore("imagewidth")]
        public int ImageWidth { get; set; }

        [JsonPropertyName("ih")]
        [DataStore("imageheight")]
        public int ImageHeight { get; set; }

        [JsonPropertyName("ci")]
        [DataStore("coverimage")]
        public String CoverImage { get; set; }

        [JsonPropertyName("cw")]
        [DataStore("coverimagewidth")]
        public int CoverImageWidth { get; set; }

        [JsonPropertyName("ch")]
        [DataStore("coverimageheight")]
        public int CoverImageHeight { get; set; }

        [JsonPropertyName("ac")]
        [DataStore("app")]
        public int AppChoppe { get; set; }

        [JsonPropertyName("sk")]
        [DataStore("appsku")]
        public String Sku { get; set; }

        [JsonPropertyName("pu")]
        [DataStore("producturl", true)]
        public String ProductUrl { get; set; }

        [JsonPropertyName("no")]
        [DataStore("notification")]
        public int RemoteNotification { get; set; }

        [JsonPropertyName("ps")]
        [DataStore("presheet")]
        public WineSheetState SheetState { get; set; }
    }
}
