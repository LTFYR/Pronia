using Pronia.Models.Base;

namespace Pronia.Models
{
    public class BasketItem:BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string AppUserId { get; set; }
        public AppUser appUser { get; set; }
    }
}
