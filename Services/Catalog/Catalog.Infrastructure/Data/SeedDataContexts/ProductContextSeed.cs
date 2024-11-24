using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;

namespace Catalog.Infrastructure.Data.SeedDataContexts
{
    public static class ProductContextSeed
    {
        public const string ResourceName = "Catalog.Infrastructure.Data.SeedData.products.json";
        public static async Task SeedData(IMongoCollection<Product> productCollection)
        {
            bool checkProducts = productCollection.Find(b => true).Any();
            if (!checkProducts)
            {
                //string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //string path = Path.Combine(baseDirectory, "Data", "SeesData", "products.json");
                //var data = File.ReadAllText(path);
                var data = string.Empty;
                using (Stream stream = Assembly.GetAssembly(typeof(CatalogContext))!.GetManifestResourceStream(ResourceName)!)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        data = reader.ReadToEnd();
                    }
                }

                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(data);
                if (products != null)
                {
                    await productCollection.InsertManyAsync(products);
                }
            }
        }
    }
}
