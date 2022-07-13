using Pronia.Models.Base;
using System.Collections.Generic;

namespace Pronia.Models
{
    public class ProductInformation:BaseEntity
    {
        public string Shipping { get ; set; }
        public string About { get; set; }
        public string Guarantee { get; set; }
        public string Description { get; set; }
        public List<Product> Products { get; set; }
    }
}
