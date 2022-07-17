using Pronia.Models;

namespace Pronia.ViewModels
{
    public class BasketItemVM
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int SubTotal { get; set; }
    }
}
