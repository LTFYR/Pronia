using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductController(AppDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            List<Product> product = _context.Products
                .Include(p => p.ProductInformation)
                .Include(p => p.ProductCategories)
                .ThenInclude(c => c.Category)
                .Include(p => p.ProductImages)
                .ToList();
            return View(product);
        }

        public IActionResult Create()
        {
            ViewBag.Info = _context.ProductInformations.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Info = _context.ProductInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                return View();
            }
            if (product.MainFoto == null || product.HoverFoto == null || product.Photos == null)
            {
                ViewBag.Info = _context.ProductInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ModelState.AddModelError(string.Empty, "Main photo,Normal Photo and Hover photo sections can't be empty.");
                return View();
            }
            if (!product.MainFoto.IsImageOk(2) || !product.HoverFoto.IsImageOk(2))
            {
                ViewBag.Info = _context.ProductInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ModelState.AddModelError(string.Empty, "Invalid image format selected");
                return View();
            }

            product.ProductImages = new List<ProductImage>();
            TempData["Filename"] = "";
            List<IFormFile> removeFile = new List<IFormFile>();
            foreach (var picture in product.Photos)
            {
                if (!picture.IsImageOk(2))
                {
                    removeFile.Add(picture);
                    product.Photos.Remove(picture);
                    TempData["Filename"] += picture.FileName + ",";
                }
                ProductImage another = new ProductImage
                {
                    Image = await picture.FileCreate(_environment.WebRootPath, "assets/images/website-images"),
                    IsMain = false,
                    Alt = picture.Name,
                    Product = product
                };
                product.ProductImages.Add(another);
            }
            product.ProductImages.RemoveAll(p => removeFile.Any(r => r.FileName == r.FileName));

            ProductImage main = new ProductImage
            {
                Image = await product.MainFoto.FileCreate(_environment.WebRootPath, "assets/images/website-images"),
                IsMain = true,
                Alt = product.Name,
                Product = product
            };

            ProductImage hover = new ProductImage
            {
                Image = await product.HoverFoto.FileCreate(_environment.WebRootPath, "assets/images/website-images"),
                IsMain = null,
                Alt = product.Name,
                Product = product
            };

            product.ProductImages.Add(main);
            product.ProductImages.Add(hover);

            product.ProductCategories = new List<ProductCategory>();
            foreach (int item in product.CategoryIds)
            {
                ProductCategory pcategory = new ProductCategory
                {
                    CategoryId = item,
                    Product = product
                };

                product.ProductCategories.Add(pcategory); 
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Product product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
