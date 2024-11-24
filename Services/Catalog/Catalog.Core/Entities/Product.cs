using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities
{
    public class Product : BaseEntity
    {
        [BsonElement("Name")]
        public string Name { get; set; } = null!;
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public ProductBrand Brand { get; set; } = null!;
        public ProductType Type { get; set; } = null!;
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal Price { get; set; }
    }
}
