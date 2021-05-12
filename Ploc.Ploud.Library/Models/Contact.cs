using System;

namespace Ploc.Ploud.Library
{
    public abstract class Contact : PloudObject
    {
        [DataStore("pid")]
        public String PlocIdentifier { get; set; }

        [DataStore("comments", true)]
        public String Comments { get; set; }

        [DataStore("contact", true)]
        public String ContactName { get; set; }

        [DataStore("phone")]
        public String Phone { get; set; }

        [DataStore("fax")]
        public String Fax { get; set; }

        [DataStore("mobile")]
        public String Mobile { get; set; }

        [DataStore("postalcode")]
        public String PostalCode { get; set; }

        [DataStore("city", true)]
        public String City { get; set; }

        [DataStore("address1")]
        public String Address1 { get; set; }

        [DataStore("address2")]
        public String Address2 { get; set; }

        [DataStore("website")]
        public String Website { get; set; }

        [DataStore("email", true)]
        public String Email { get; set; }

        [DataStore("latitude")]
        public String Latitude { get; set; }

        [DataStore("longitude")]
        public String Longitude { get; set; }
    }
}
