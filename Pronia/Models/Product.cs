using Pronia.Models.Base;
using System.Collections.Generic;

namespace Pronia.Models
{
    public class Product:BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int ProductInformationId { get; set; }
        public ProductInformation ProductDetail { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
