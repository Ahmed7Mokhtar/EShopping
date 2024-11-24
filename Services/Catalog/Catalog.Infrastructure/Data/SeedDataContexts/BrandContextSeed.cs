using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;

namespace Catalog.Infrastructure.Data.SeedDataContexts
{
    public static class BrandContextSeed
    {
        public const string ResourceName = "Catalog.Infrastructure.Data.SeedData.brands.json";
        public static async Task SeedData(IMongoCollection<ProductBrand> brandCollection)
        {
            bool checkBrands = brandCollection.Find(b => true).Any();
            if (!checkBrands)
            {
                //string baseDirectory = Directory.GetCurrentDirectory();
                //string path = Path.Combine(baseDirectory, "Data", "SeesData", "brands.json");
                //var data = File.ReadAllText(path);
                var data = string.Empty;
                using (Stream stream = Assembly.GetAssembly(typeof(CatalogContext))!.GetManifestResourceStream(ResourceName)!)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        data = reader.ReadToEnd();
                    }
                }

                var brands = JsonSerializer.Deserialize<IEnumerable<ProductBrand>>(data);
                if (brands != null)
                {
                    await brandCollection.InsertManyAsync(brands);
                }
            }
        }
    }
}
