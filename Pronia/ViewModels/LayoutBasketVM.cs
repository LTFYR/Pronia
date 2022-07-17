using Pronia.Models;
using System.Collections.Generic;

namespace Pronia.ViewModels
{
    public class LayoutBasketVM
    {
        public List<BasketItemVM> BasketItemVMs { get; set; }
        public decimal TotalPRice { get; set; }
    }
}
