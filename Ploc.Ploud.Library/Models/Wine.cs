using System;
using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    [DataStore("wine")]
    public class Wine : PloudObject
    {
        [JsonPropertyName("cn")]
        [DataStore("country")]
        public string Country { get; set; }

        [JsonPropertyName("rn")]
        [DataStore("region")]
        public string Region { get; set; }

        [JsonPropertyName("ap")]
        [DataStore("appellation")]
        public string Appellation { get; set; }

        [JsonPropertyName("vf")]
        [DataStore("bottleformat")]
        public string BottleFormat { get; set; }

        [JsonPropertyName("cl")]
        [DataStore("classification")]
        public string Classification { get; set; }

        [JsonPropertyName("co")]
        [DataStore("color")]
        public string Color { get; set; }

        [JsonPropertyName("ow")]
        [DataStore("owner")]
        public string Owner { get; set; }

        [JsonPropertyName("pid")]
        [DataStore("pid", true)]
        public string PlocIdentifier { get; set; }

        [JsonPropertyName("cs")]
        [DataStore("comments", true)]
        public string Comments { get; set; }

        [JsonPropertyName("cu")]
        [DataStore("cuvee")]
        public string Cuvee { get; set; }

        [JsonPropertyName("tg")]
        [DataStore("tags", true)]
        public string Tags { get; set; }

        [JsonPropertyName("re")]
        [DataStore("reference", true)]
        public string Reference { get; set; }

        [JsonPropertyName("gr")]
        [DataStore("composition")]
        public string Grapes { get; set; }

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
        public string Language { get; set; }

        [JsonPropertyName("im")]
        [DataStore("image")]
        public string Image { get; set; }

        [JsonPropertyName("iw")]
        [DataStore("imagewidth")]
        public int ImageWidth { get; set; }

        [JsonPropertyName("ih")]
        [DataStore("imageheight")]
        public int ImageHeight { get; set; }

        [JsonPropertyName("ci")]
        [DataStore("coverimage")]
        public string CoverImage { get; set; }

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
        public string Sku { get; set; }

        [JsonPropertyName("pu")]
        [DataStore("producturl", true)]
        public string ProductUrl { get; set; }

        [JsonPropertyName("no")]
        [DataStore("notification")]
        public int RemoteNotification { get; set; }

        [JsonPropertyName("ps")]
        [DataStore("presheet")]
        public WineSheetState SheetState { get; set; }

        public Country GetCountry()
        {
            throw new NotImplementedException();
        }

        public Region GetRegion()
        {
            throw new NotImplementedException();
        }

        public Appellation GetAppellation()
        {
            throw new NotImplementedException();
        }

        public BottleFormat GetBottleFormat()
        {
            throw new NotImplementedException();
        }

        public Classification GetClassification()
        {
            throw new NotImplementedException();
        }

        public Color GetColor()
        {
            throw new NotImplementedException();
        }

        public Owner GetOwner()
        {
            throw new NotImplementedException();
        }
    }
}
