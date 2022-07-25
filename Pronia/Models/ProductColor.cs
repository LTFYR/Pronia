using Pronia.Models.Base;

namespace Pronia.Models
{
    public class ProductColor : BaseEntity
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public Product Product { get; set; }
    }
}
