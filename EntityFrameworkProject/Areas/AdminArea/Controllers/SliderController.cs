using EntityFrameworkProject.Data;
using EntityFrameworkProject.Helpers;
using EntityFrameworkProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _context.Sliders.Where(m=>!m.IsDeleted).ToListAsync();
            return View(sliders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            if (!slider.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }


            if (!slider.Photo.CheckFileSize(2000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "img", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await slider.Photo.CopyToAsync(stream);
            }

            slider.Image = fileName;

            await _context.Sliders.AddAsync(slider);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            Slider slider = await GetByIdAsync(id);

            if (slider == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "img", slider.Image);

            Helper.DeleteFile(path);

            _context.Sliders.Remove(slider);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

                if (slider is null) return NotFound();

                return View(slider);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Slider slider)
        {
            try
            {
                Slider dbSlider = await GetByIdAsync(id);
                if (!ModelState.IsValid)
                {
                    return View(slider);
                }

                if (!slider.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image type");
                    return View(dbSlider);
                }

               

                if (dbSlider is null) return NotFound();

                if (dbSlider.Photo == slider.Photo)
                {
                    return RedirectToAction(nameof(Index));
                }


                string oldPath = Helper.GetFilePath(_env.WebRootPath, "img", dbSlider.Image);

                Helper.DeleteFile(oldPath);

                string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;

                string newPath = Helper.GetFilePath(_env.WebRootPath, "img", fileName);

                using (FileStream stream = new FileStream(newPath, FileMode.Create))
                {
                    await slider.Photo.CopyToAsync(stream);
                }

                dbSlider.Image = fileName;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return BadRequest();

            Slider slider = await _context.Sliders.FindAsync(id);

            if (slider == null) return NotFound();

            return View(slider);
        }


        private async Task<Slider> GetByIdAsync(int id)
        {
            return await _context.Sliders.FindAsync(id);
        }   
    
    }
}
