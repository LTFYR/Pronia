using Pronia.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Category:BaseEntity
    {
        [Required,StringLength(maximumLength:30)]
        public string Name { get; set; }    
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
