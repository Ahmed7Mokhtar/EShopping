using Catalog.Application.DTOs.Brands;
using Catalog.Application.DTOs.Types;
using Catalog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.DTOs.Products
{
    public class CreateProductDTO
    {
        public string Name { get; set; } = null!;
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public decimal Price { get; set; }
        public BrandDTO Brand { get; set; } = null!;
        public TypeDTO Types { get; set; } = null!;
    }
}
