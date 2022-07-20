using Microsoft.AspNetCore.Http;
using Pronia.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public ProductInformation ProductInformation { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<ProductTag> ProductTags { get; set; }

        [NotMapped]
        public List<int> CategoryIds { get; set; }

        [NotMapped]
        public List<IFormFile> Photos { get; set; }
        [NotMapped]
        public IFormFile MainFoto { get; set; }
        [NotMapped]
        public IFormFile HoverFoto { get; set; }
    }
}
