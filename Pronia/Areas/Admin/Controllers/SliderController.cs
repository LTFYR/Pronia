using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SliderController(AppDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            List<Slider> slider = _context.Sliders.ToList();
            return View(slider);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            if(slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Choose at least 1 image");
                return View();
            }
            if (!slider.Photo.IsImageOk(2))
            {
                ModelState.AddModelError("Photo", "You have chosen invalid size of image");
                return View();
            }




            slider.Image = await slider.Photo.FileCreate(_environment.WebRootPath, "assets/images/website-images");


            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Slider slider =await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if(slider == null) return NotFound();
            return View(slider);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, Slider slider)
        {
            if(!ModelState.IsValid) return View();
            Slider current = await _context.Sliders.FindAsync(id);
            if (current == null) return NotFound();

            if(slider.Photo == null)
            {
                string fileName = current.Image;
                _context.Entry(current).CurrentValues.SetValues(slider);
                current.Image = fileName;
            }
            else
            {
                if (!slider.Photo.IsImageOk(2))
                {
                    ModelState.AddModelError("Photo", "You have chosen invalid size of image");
                    return View();
                }
                _context.Entry(current).CurrentValues.SetValues(slider);
                FileValidator.FileDelete(_environment.WebRootPath, "assets/images/website-images", current.Image);
                current.Image = await slider.Photo.FileCreate(_environment.WebRootPath, "assets/images/website-images");

            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
