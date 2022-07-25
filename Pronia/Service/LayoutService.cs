using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Pronia.Service
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;

        public LayoutService(AppDbContext context,IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }
        
        public List<Setting> GetSettings()
        {
            List<Setting> settings = _context.Settings.ToList();
            return settings;
        }

        public LayoutBasketVM GetBasket()
        { 

            string BasketStr = _http.HttpContext.Request.Cookies["Cart"];
            if (!string.IsNullOrEmpty(BasketStr))
            {
                BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(BasketStr);
                LayoutBasketVM layoutBasketVM = new LayoutBasketVM();
                layoutBasketVM.BasketItemVMs = new List<BasketItemVM>();
                foreach (BasketCookieItemVM item in basket.BasketCookieItemVMs)
                {
                    Product current = _context.Products.Include(i=>i.ProductImages).FirstOrDefault(p => p.Id == item.Id);
                    if (current == null)
                    {
                        basket.BasketCookieItemVMs.Remove(item);
                        continue;
                    }
                    BasketItemVM itemVM = new BasketItemVM
                    {
                        Product = current,
                        Quantity = item.Quantity
                    };
                    layoutBasketVM.BasketItemVMs.Add(itemVM);
                    
                }
                layoutBasketVM.TotalPRice = basket.TotalPrice;
                return layoutBasketVM;
            }
            return null;
        }

        //public void DeleteProduct(int? id)
        //{
        //    string[] myCookies =  _http.HttpContext.Request.Cookies["Cart"];
        //    foreach (string cookie in myCookies)
        //    {
        //        Response.Cookies.Delete(cookie);
        //    }
        //}

    }
}
