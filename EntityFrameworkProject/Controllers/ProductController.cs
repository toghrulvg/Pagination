using EntityFrameworkProject.Data;
using EntityFrameworkProject.Models;
using EntityFrameworkProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ProductService _productService;
        private readonly LayoutService _layoutService;

        public ProductController(AppDbContext context, ProductService productService, LayoutService layoutService)
        {
            _context = context;
            _productService = productService;
            _layoutService = layoutService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.count = await _context.Products.Where(m => !m.IsDeleted).CountAsync();

            Dictionary<string, string> settingDatas = await _layoutService.GetDatasFromSetting();

            int take = int.Parse(settingDatas["ProductTake"]);

            IEnumerable<Product> products = await _productService.GetAll(take);
            return View(products);
        }


        public async Task<IActionResult> LoadMore(int skip)
        {
            IEnumerable<Product> products = await _context.Products.Where(m => !m.IsDeleted).Include(m=>m.Category).Include(m=>m.ProductImages).Skip(skip).Take(4).ToListAsync();

            return PartialView("_ProductsPartial", products);
        }
    }
}
