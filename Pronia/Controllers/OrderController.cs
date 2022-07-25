using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pronia.Controllers
{
    [Authorize(Roles = "Member")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CheckOut(Order order)
        {
            if(!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<BasketItem> item = await _context.BasketItems.Include(b=>b.appUser).Include(b=>b.Product).Where(b=>b.AppUserId == user.Id)
                .ToListAsync();
            if(item == null) return NotFound();
            order.BasketItems = item;
            order.appUser = user;
            order.Date = DateTime.Now;
            order.status = null;
            order.TotalPrice = default;
            foreach (var tp in item)
            {
                order.TotalPrice += tp.Price;
            }
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
