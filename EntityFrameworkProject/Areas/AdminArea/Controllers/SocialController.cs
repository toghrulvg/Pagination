using EntityFrameworkProject.Data;
using EntityFrameworkProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SocialController : Controller
    {

        private readonly AppDbContext _context;

        public SocialController(AppDbContext context)
        {
            _context = context;
        }
        // GET: SocialController
        public async Task<IActionResult> Index()
        {
            List<Social> socials = await _context.Socials.Where(m => !m.IsDeleted).AsNoTracking().OrderByDescending(m => m.Id).ToListAsync();
            return View(socials);
        }

        // GET: SocialController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return BadRequest();

            Social social = await _context.Socials.FindAsync(id);

            if (social == null) return NotFound();

            return View(social);
        }

        // GET: SocialController/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SocialController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Social social)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View();
                }

                bool isExist = await _context.Socials.AnyAsync(m => m.Name.Trim() == social.Name.Trim() && m.Url.Trim() == social.Url.Trim());
                if (isExist)
                {
                    ModelState.AddModelError("Name And Url", "Category already exist");
                    return View();
                }

                await _context.Socials.AddAsync(social);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

        // GET: SocialController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                Social social = await _context.Socials.FirstOrDefaultAsync(m => m.Id == id);

                if (social is null) return NotFound();

                return View(social);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

        // POST: SocialController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Social social)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(social);
                }

                Social dbSocial = await _context.Socials.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                if (dbSocial is null) return NotFound();

                if (dbSocial.Name.ToLower().Trim() == social.Name.ToLower().Trim() && dbSocial.Url.ToLower().Trim() == social.Url.ToLower().Trim())
                {
                    return RedirectToAction(nameof(Index));
                }

                //dbCategory.Name = category.Name;

                _context.Socials.Update(social);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

       

        // POST: SocialController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Social social = await _context.Socials.FirstOrDefaultAsync(m => m.Id == id);

            social.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
