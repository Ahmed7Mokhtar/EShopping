using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;

namespace Catalog.Infrastructure.Data.SeedDataContexts
{
    public static class TypeContextSeed
    {
        public const string ResourceName = "Catalog.Infrastructure.Data.SeedData.types.json";

        public static async Task SeedData(IMongoCollection<ProductType> typeCollection)
        {
            bool checkTypes = typeCollection.Find(b => true).Any();
            if (!checkTypes)
            {
                //string baseDirectory = Directory.GetCurrentDirectory();
                //string path = Path.Combine(baseDirectory, "Data", "SeesData", "types.json");
                //var data = File.ReadAllText(path);
                var data = string.Empty;
                using (Stream stream = Assembly.GetAssembly(typeof(CatalogContext))!.GetManifestResourceStream(ResourceName)!)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        data = reader.ReadToEnd();
                    }
                }

                var types = JsonSerializer.Deserialize<IEnumerable<ProductType>>(data);
                if (types != null)
                {
                    await typeCollection.InsertManyAsync(types);
                }
            }
        }
    }
}
