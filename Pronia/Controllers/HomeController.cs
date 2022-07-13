using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Sliders = _context.Sliders.ToList(),
                Products = _context.Products.Include(p=>p.ProductImages).ToList(),
            };
            return View(homeVM);
        }
        public IActionResult ProductDetail()
        {
            return View();
        }
    }
}
