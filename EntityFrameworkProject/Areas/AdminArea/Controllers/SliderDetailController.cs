using EntityFrameworkProject.Data;
using EntityFrameworkProject.Helpers;
using EntityFrameworkProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Controllers
{
    [Area("AdminArea")]
    public class SliderDetailController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderDetailController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            SliderDetail sliderDetail = await _context.SliderDetails.Where(m => !m.IsDeleted).FirstOrDefaultAsync();
            ViewBag.count = await _context.SliderDetails.Where(m => !m.IsDeleted).CountAsync();
            return View(sliderDetail);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return BadRequest();

            SliderDetail sliderDetail = await _context.SliderDetails.FindAsync(id);

            if (sliderDetail == null) return NotFound();

            return View(sliderDetail);
        }

        public async Task<IActionResult> Delete(int id)
        {
            SliderDetail sliderDetail = await _context.SliderDetails.FindAsync(id);

            if (sliderDetail == null) return NotFound();
                                

            _context.SliderDetails.Remove(sliderDetail);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                SliderDetail sliderDetail = await _context.SliderDetails.FirstOrDefaultAsync(m => m.Id == id);

                if (sliderDetail is null) return NotFound();

                return View(sliderDetail);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SliderDetail sliderDetail)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(sliderDetail);
                }

                if (sliderDetail.Photo != null)
                {
                    if (!sliderDetail.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View();
                    }

                    if (!sliderDetail.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image size");
                        return View();
                    }

                    string fileName = Guid.NewGuid().ToString() + "_" + sliderDetail.Photo.FileName;

                    SliderDetail dbSliderDetail = await _context.SliderDetails.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                    if (dbSliderDetail is null) return NotFound();

                    if (dbSliderDetail.Header.Trim().ToLower() == sliderDetail.Header.Trim().ToLower()
                        && dbSliderDetail.Description.Trim().ToLower() == sliderDetail.Description.Trim().ToLower()
                        && dbSliderDetail.Photo == sliderDetail.Photo)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    string path = Helper.GetFilePath(_env.WebRootPath, "img", fileName);

                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await sliderDetail.Photo.CopyToAsync(stream);
                    }

                    sliderDetail.Image = fileName;

                    _context.SliderDetails.Update(sliderDetail);

                    await _context.SaveChangesAsync();

                    string dbPath = Helper.GetFilePath(_env.WebRootPath, "img", dbSliderDetail.Image);

                    Helper.DeleteFile(dbPath);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderDetail sliderDetail)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                if (!sliderDetail.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image type");
                    return View();
                }

                if (!sliderDetail.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image size");
                    return View();
                }

                string fileName = Guid.NewGuid().ToString() + "_" + sliderDetail.Photo.FileName;

                string path = Helper.GetFilePath(_env.WebRootPath, "img", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await sliderDetail.Photo.CopyToAsync(stream);
                }

                sliderDetail.Image = fileName;

                await _context.SliderDetails.AddAsync(sliderDetail);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

    }
}
