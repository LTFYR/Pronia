using Pronia.Models.Base;
using System;
using System.Collections.Generic;

namespace Pronia.Models
{
    public class Order:BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public bool? status { get; set; }
        public string Adress { get; set; }
        public string AppUserId { get; set; }
        public AppUser appUser  { get; set; }
    }
}
