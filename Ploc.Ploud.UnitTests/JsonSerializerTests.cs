using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System.Text.Json;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class JsonSerializerTests
    {
        [TestMethod]
        public void CheckUniqueJsonProperties()
        {
            JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
            };
            JsonSerializer.Serialize(new Country(), serializerOptions);
            JsonSerializer.Serialize(new Region(), serializerOptions);
            JsonSerializer.Serialize(new Appellation(), serializerOptions);
            JsonSerializer.Serialize(new Classification(), serializerOptions);
            JsonSerializer.Serialize(new Color(), serializerOptions);
            JsonSerializer.Serialize(new Grapes(), serializerOptions);
            JsonSerializer.Serialize(new Document(), serializerOptions);
            JsonSerializer.Serialize(new Order(), serializerOptions);
            JsonSerializer.Serialize(new Wine(), serializerOptions);
            JsonSerializer.Serialize(new TastingNotes(), serializerOptions);
            JsonSerializer.Serialize(new Rack(), serializerOptions);
            JsonSerializer.Serialize(new RackItem(), serializerOptions);
            JsonSerializer.Serialize(new GlobalParameter(), serializerOptions);
            JsonSerializer.Serialize(new Vendor(), serializerOptions);
            JsonSerializer.Serialize(new Owner(), serializerOptions);
            JsonSerializer.Serialize(new DeletedObject(), serializerOptions);
        }
    }
}
