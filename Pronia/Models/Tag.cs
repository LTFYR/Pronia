using Pronia.Models.Base;
using System.Collections.Generic;

namespace Pronia.Models
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; }
        public List<ProductTag> ProductTags { get; set; }
    }
}
