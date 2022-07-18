using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizeController : Controller
    {
        private readonly AppDbContext _context;

        public SizeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Size> model = _context.Sizes.ToList();
            return View(model);
        }

        public IActionResult CreateSize()
        {
            return View();
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult CreateSize(Size size)
        {
            if(!ModelState.IsValid) return View();
            Size current = _context.Sizes.FirstOrDefault(s=>s.Name.Trim().ToLower() == size.Name.Trim().ToLower());
            if (current != null)
            {
                ModelState.AddModelError("Name", "Eyni ad iki defe daxil edile bilmez");
                return View();
            }

            _context.Sizes.Add(size);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Size size = _context.Sizes.FirstOrDefault(s=>s.Id == id);
            if (size == null) return NotFound();
            return View(size);
        }



        [HttpPost]
        public IActionResult Edit(int? id, Size newsize)
        {
            if (id == null || id == 0) return NotFound();
            Size current = _context.Sizes.FirstOrDefault(s=>s.Id == id);
            if(current == null) return NotFound();
            bool s = _context.Sizes.Any(s => s.Name.Trim().ToLower() == newsize.Name.Trim().ToLower());
            if (s)
            {
                ModelState.AddModelError("Name", "Tekrar ad daxil edildi");
            }
            _context.Entry(current).CurrentValues.SetValues(newsize);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Size size = await _context.Sizes.FindAsync(id);
            if (size == null) return NotFound();
            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


    }
}
