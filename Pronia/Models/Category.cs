using Pronia.Models.Base;
using System.Collections.Generic;

namespace Pronia.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }    
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
