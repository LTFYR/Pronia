using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;

        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Color> color = _context.Colors.ToList();
            return View(color);
        }

        public IActionResult CreateColor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateColor(Color color)
        {
            if (!ModelState.IsValid) return View();
            Color current = _context.Colors.FirstOrDefault(x => x.Name.ToLower().Trim() == color.Name.ToLower().Trim());
            if(current != null)
            {
                ModelState.AddModelError("Name", "Besdi de");
                return View();
            }
            _context.Colors.Add(color);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Color color = _context.Colors.FirstOrDefault(x => x.Id == id);
            if (color == null) return NotFound();
            return View(color);
        }


        [HttpPost]
        public IActionResult Edit(int? id, Color newcolor)
        {
            if (id == null || id == 0) return NotFound();
            Color current = _context.Colors.FirstOrDefault(x => x.Id == id);
            if(current == null) return NotFound();
            bool c = _context.Colors.Any(c=>c.Name.ToLower().Trim() == newcolor.Name.ToLower().Trim());
            if (c)
            {
                ModelState.AddModelError("Name", "Bezzidridddddin");
                return View();
            }
            _context.Entry(current).CurrentValues.SetValues(newcolor);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Color color =await _context.Colors.FindAsync(id);
            if(color == null) return NotFound();
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
