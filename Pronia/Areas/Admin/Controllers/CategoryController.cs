using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {


        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Category> category = _context.Categories.ToList();
            return View(category);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }


        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (!ModelState.IsValid)
                return View();
            Category current = _context.Categories.FirstOrDefault(c => c.Name.ToLower().Trim() == category.Name.ToLower().Trim());
            if (current != null)
            {
                ModelState.AddModelError("Name", "Eyni kateqoriya artıq databazada mövcuddur");
                return View();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Category newcategory)
        {
            if (id == null || id == 0) return NotFound();
            Category current = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (current == null)
                return NotFound();
            bool dublicate = _context.Categories.Any(c =>c.Id!=id && c.Name.Trim().ToLower() == newcategory.Name.Trim().ToLower());
            if (dublicate)
            {
                ModelState.AddModelError("Name", "Eyni adi tekrar daxil ede bilmezsiniz");
                return View();
            }
            Category existed = await _context.Categories.Include(p => p.ProductCategories).ThenInclude(p => p.Category).SingleOrDefaultAsync(p => p.Id == id);
            if(existed == null) return NotFound();
            List<ProductCategory> remove = current.ProductCategories.Where(p => p.Id == newcategory.Id).ToList();
            existed.ProductCategories.RemoveAll(p=>remove.Any(r=> p.Id == r.Id));
            //current.Name = newcategory.Name;
            _context.Entry(current).CurrentValues.SetValues(newcategory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || id == 0) return NotFound();
            Category category = await  _context.Categories.FindAsync(id);
            if(category == null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
