using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Service;
using Pronia.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pronia.Controllers
{
    public class ProductController:Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ProductDetail(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(c=>c.ProductInformation)
                .Include(p=>p.ProductTags)
                .ThenInclude(t=>t.Tag)
                .Include(pc=>pc.ProductCategories)
                .ThenInclude(c=>c.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            ViewBag.Products = await _context.Products
                .Include(p => p.ProductImages)
                .ToListAsync();

            


            if (product is null) return NotFound();


            return View(product);
        }

        public async Task<IActionResult> Partial()
        {
            List<Product> products = await _context.Products.Include(p=>p.ProductImages).ToListAsync();

            return PartialView("_ProductPartialView", products);

        }

        public async Task<IActionResult> GetDetail(int? id)
        {
            Product product = await _context.Products.Include(x => x.ProductImages).Include(i=>i.ProductInformation).FirstOrDefaultAsync(p => p.Id == id);
            return PartialView("_detailpage", product);
        }






        //HttpContext.Response.Cookies.Append("Cart", product.Image);

        //public async Task<IActionResult> AddToCart(int? id)
        //{
            
        //    if (id == null || id == 0) return NotFound();
        //    Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        //    if (product == null) return NotFound();

        //    string basketVMstr = HttpContext.Request.Cookies["Cart"];

        //    BasketVM basket;

        //    if (string.IsNullOrEmpty(basketVMstr))
        //    {
        //        basket = new BasketVM();
        //        CookieItemVM cookieItem = new CookieItemVM
        //        {
        //            Id = product.Id,
        //            Quantity = 1
        //        };
        //        basket.CookieItemVMs.Add(cookieItem);
        //    }
        //    else
        //    {
        //        basket = JsonConvert.DeserializeObject<BasketVM>(basketVMstr);
        //        CookieItemVM current = basket.CookieItemVMs.Find(p => p.Id == id);
        //        if (current == null)
        //        {
        //            CookieItemVM cookieItem = new CookieItemVM
        //            {
        //                Id = product.Id,
        //                Quantity = 1
        //            };
        //            basket.CookieItemVMs.Add(cookieItem);
        //            basket.TotalPrice += product.Price;
        //        }
        //        else
        //        {
        //            basket.TotalPrice += product.Price;
        //            current.Quantity++;
        //        }
        //    }




        //    basketVMstr = JsonConvert.SerializeObject(basket);

        //    HttpContext.Response.Cookies.Append("Cart", basketVMstr);

        //    return RedirectToAction(nameof(ShowCart));
        //}

        //public IActionResult ShowCart()
        //{
        //    BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(HttpContext.Request.Cookies["Cart"]);
        //    return Json(basket);
        //}

        //public IActionResult AddBasket()
        //{
        //    HttpContext.Response.Cookies.Append("Cart", "Hello");
        //    return Json(HttpContext.Request.Cookies["Cart"]);
        //}




        public async Task<IActionResult> AddToCart(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product == null) return NotFound();

            string basketStr = HttpContext.Request.Cookies["Cart"];



            BasketVM basket;
            if (string.IsNullOrEmpty(basketStr))
            {
                basket = new BasketVM();
                BasketCookieItemVM cookieItem = new BasketCookieItemVM
                {
                    Id = product.Id,
                    Quantity = 1
                };
                basket.BasketCookieItemVMs = new List<BasketCookieItemVM>();
                basket.BasketCookieItemVMs.Add(cookieItem);
                basket.TotalPrice = product.Price;
                basket.TotalPrice = (product.Price * cookieItem.Quantity);
            }
            else
            {
                basket = JsonConvert.DeserializeObject<BasketVM>(basketStr);
                BasketCookieItemVM current = basket.BasketCookieItemVMs.Find(p => p.Id == id);
                if(current == null)
                {
                    BasketCookieItemVM cookieItem = new BasketCookieItemVM
                    {
                        Id = product.Id,
                        Quantity = 1
                    };
                    basket.BasketCookieItemVMs.Add(cookieItem);
                    basket.TotalPrice += product.Price;
                    
                }
                else
                {
                    basket.TotalPrice += product.Price;
                    current.Quantity++;
                }

            }
            basketStr = JsonConvert.SerializeObject(basket);
            HttpContext.Response.Cookies.Append("Cart", basketStr);

            return RedirectToAction("Index","Home");

        }

        public IActionResult ShowBasket()
        {
            if (HttpContext.Request.Cookies["Cart"] == null) return NotFound();
            BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(HttpContext.Request.Cookies["Cart"]);
            return Json(basket);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || id == 0) return NotFound();
            Product product =await _context.Products.FirstOrDefaultAsync(p=>p.Id == id);
            if(product == null) return NotFound();
            string basketStr = HttpContext.Request.Cookies["Cart"];
            BasketVM basketVM = JsonConvert.DeserializeObject<BasketVM>(basketStr);
            BasketCookieItemVM current = basketVM.BasketCookieItemVMs.FirstOrDefault(i => i.Id == id);
            basketVM.BasketCookieItemVMs.Remove(current);
            basketStr = JsonConvert.SerializeObject(basketVM);
            HttpContext.Response.Cookies.Append("Cart", basketStr);
            return RedirectToAction("Index", "Home");
        }

    }
}
