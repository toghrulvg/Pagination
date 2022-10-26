using EntityFrameworkProject.Data;
using EntityFrameworkProject.Helpers;
using EntityFrameworkProject.Models;
using EntityFrameworkProject.ViewModels.CategoryVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {            
            List<Category> categories = await _context.Categories
                .Where(m=>m.IsDeleted)
                .AsNoTracking()
                .Skip((page * take) - take)
                .Take(take)
                .OrderByDescending(m=>m.Id).ToListAsync();

            List<CategoryVM> mapDatas = GetMapDatas(categories);

            int count = await GetPageCount(take);

            Paginate<CategoryVM> result = new Paginate<CategoryVM>(mapDatas, page, count);
            return View(result);
        }

        public async Task<int> GetPageCount(int take)
        {
            int categoryCount = await _context.Categories.Where(m => !m.IsDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)categoryCount / take);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }


                bool isExist = await _context.Categories.AnyAsync(m => m.Name.Trim() == category.Name.Trim());

                if (isExist)
                {
                    ModelState.AddModelError("Name", "Category already exist");
                    return View();
                }

                //change exist data is delete

                //var data = await _context.Categories.Where(m => m.IsDeleted == true).FirstOrDefaultAsync(m=>m.Name.Trim() == category.Name.Trim());

                //if(data is null)
                //{
                //    await _context.Categories.AddAsync(category);
                //}
                //else
                //{
                //    data.IsDeleted = false;
                //}

                await _context.Categories.AddAsync(category);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
      
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories.FindAsync(id);

            if (category == null) return NotFound();

            return View(category);
        }

        //delete from database

        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

        //    _context.Categories.Remove(category);

        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            category.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

                if (category is null) return NotFound();

                return View(category);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }

                Category dbCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                if (dbCategory is null) return NotFound();

                if(dbCategory.Name.ToLower().Trim() == category.Name.ToLower().Trim())
                {
                    return RedirectToAction(nameof(Index));
                }

                //dbCategory.Name = category.Name;

                _context.Categories.Update(category);

                await _context.SaveChangesAsync();

                return  RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }

        private List<CategoryVM> GetMapDatas(List<Category> categories)
        {
            List<CategoryVM> categoryList = new List<CategoryVM>();
            foreach (var category in categories)
            {
                CategoryVM newCategory = new CategoryVM
                {
                    Id = category.Id,
                    Name = category.Name,
                 

                };
                categoryList.Add(newCategory);
            }
            return categoryList;
        }
    }
}
