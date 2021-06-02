using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public class SyncObjects
    {
        public PloudObjectCollection<Country> Countries { get; set; }

        public PloudObjectCollection<Region> Regions { get; set; }
        
        public PloudObjectCollection<Appellation> Appellations { get; set; }

        public PloudObjectCollection<Color> Colors { get; set; }
        
        public PloudObjectCollection<Grapes> Grapes { get; set; }

        public PloudObjectCollection<Classification> Classifications { get; set; }

        public PloudObjectCollection<Vendor> Vendors { get; set; }

        public PloudObjectCollection<Owner> Owners { get; set; }

        public PloudObjectCollection<Rack> Racks { get; set; }

        public PloudObjectCollection<RackItem> RackItems { get; set; }

        public PloudObjectCollection<Wine> Wines { get; set; }

        public PloudObjectCollection<TastingNotes> TastingNotes { get; set; }

        public PloudObjectCollection<Document> Documents { get; set; }

        public PloudObjectCollection<Order> Orders { get; set; }

        public PloudObjectCollection<GlobalParameter> GlobalParameters { get; set; }

        public PloudObjectCollection<DeletedObject> DeletedObjects { get; set; }

        public PloudObjectCollection<IPloudObject> AllObjects(ICellar cellar)
        {
            PloudObjectCollection<IPloudObject> allObjects = new PloudObjectCollection<IPloudObject>();
            if((this.Countries != null)
                && (this.Countries.Count > 0))
            {
                allObjects.AddRange(this.Countries);
            }
            if ((this.Regions != null)
                && (this.Regions.Count > 0))
            {
                allObjects.AddRange(this.Regions);
            }
            if ((this.Appellations != null)
                && (this.Appellations.Count > 0))
            {
                allObjects.AddRange(this.Appellations);
            }
            if ((this.Colors != null)
                && (this.Colors.Count > 0))
            {
                allObjects.AddRange(this.Colors);
            }
            if ((this.Classifications != null)
                && (this.Classifications.Count > 0))
            {
                allObjects.AddRange(this.Classifications);
            }
            if ((this.Grapes != null)
                && (this.Grapes.Count > 0))
            {
                allObjects.AddRange(this.Grapes);
            }
            if ((this.Owners != null)
                && (this.Owners.Count > 0))
            {
                allObjects.AddRange(this.Owners);
            }
            if ((this.Vendors != null)
                && (this.Vendors.Count > 0))
            {
                allObjects.AddRange(this.Vendors);
            }
            if ((this.Documents != null)
                && (this.Documents.Count > 0))
            {
                allObjects.AddRange(this.Documents);
            }
            if ((this.GlobalParameters != null)
                && (this.GlobalParameters.Count > 0))
            {
                allObjects.AddRange(this.GlobalParameters);
            }
            if ((this.Racks != null)
                && (this.Racks.Count > 0))
            {
                allObjects.AddRange(this.Racks);
            }
            if ((this.RackItems != null)
                && (this.RackItems.Count > 0))
            {
                allObjects.AddRange(this.RackItems);
            }
            if ((this.Orders != null)
                && (this.Orders.Count > 0))
            {
                allObjects.AddRange(this.Orders);
            }
            if ((this.Wines != null)
                && (this.Wines.Count > 0))
            {
                allObjects.AddRange(this.Wines);
            }
            if ((this.TastingNotes != null)
                && (this.TastingNotes.Count > 0))
            {
                allObjects.AddRange(this.TastingNotes);
            }
            if ((this.DeletedObjects != null)
                && (this.DeletedObjects.Count > 0))
            {
                allObjects.AddRange(this.DeletedObjects);
            }
            foreach(IPloudObject ploudObject in allObjects)
            {
                ploudObject.Cellar = cellar;
                ploudObject.TimeLastModified = DateTime.UtcNow;
            }
            return allObjects;
        }

        internal void RemoveEmptyCollection()
        {
            if(this.Countries.Count == 0)
            {
                this.Countries = null;
            }
            if (this.Regions.Count == 0)
            {
                this.Regions = null;
            }
            if (this.Appellations.Count == 0)
            {
                this.Appellations = null;
            }
            if (this.Colors.Count == 0)
            {
                this.Colors = null;
            }
            if (this.Classifications.Count == 0)
            {
                this.Classifications = null;
            }
            if (this.Grapes.Count == 0)
            {
                this.Grapes = null;
            }
            if (this.Vendors.Count == 0)
            {
                this.Vendors = null;
            }
            if (this.Owners.Count == 0)
            {
                this.Owners = null;
            }
            if (this.Documents.Count == 0)
            {
                this.Documents = null;
            }
            if (this.Racks.Count == 0)
            {
                this.Racks = null;
            }
            if (this.RackItems.Count == 0)
            {
                this.RackItems = null;
            }
            if (this.Wines.Count == 0)
            {
                this.Wines = null;
            }
            if (this.TastingNotes.Count == 0)
            {
                this.TastingNotes = null;
            }
            if (this.GlobalParameters.Count == 0)
            {
                this.GlobalParameters = null;
            }
        }

        [JsonIgnore]
        public int Count
        {
            get
            {
                return GetCount(this.Countries)
                    + GetCount(this.Regions)
                    + GetCount(this.Appellations)
                    + GetCount(this.Grapes)
                    + GetCount(this.Classifications)
                    + GetCount(this.Owners)
                    + GetCount(this.Vendors)
                    + GetCount(this.GlobalParameters)
                    + GetCount(this.Wines)
                    + GetCount(this.TastingNotes)
                    + GetCount(this.Documents)
                    + GetCount(this.Racks)
                    + GetCount(this.RackItems)
                    + GetCount(this.Colors);
            }
        }

        public int GetCount(IList ploudObjects)
        {
            return ploudObjects == null ? 0 : ploudObjects.Count;
        }
    }
}
