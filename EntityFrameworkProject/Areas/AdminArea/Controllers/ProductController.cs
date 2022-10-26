using EntityFrameworkProject.Data;
using EntityFrameworkProject.Helpers;
using EntityFrameworkProject.Models;
using EntityFrameworkProject.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Product> products = await _context.Products
                .Where(m => !m.IsDeleted)
                .Include(m => m.ProductImages)
                .Include(m => m.Category)
                .Skip((page*take)-take)
                .Take(take)
                .OrderByDescending(m => m.Id)   
                .ToListAsync();

            List<ProductListVM> mapDatas = GetMapDatas(products);

            int count = await GetPageCount(take);

            Paginate<ProductListVM> result = new Paginate<ProductListVM>(mapDatas, page, count);
            return View(result);
        }

        public async Task<int> GetPageCount(int take)
        {
            int productCount = await _context.Products.Where(m =>!m.IsDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)productCount / take);
        }

        private List<ProductListVM> GetMapDatas(List<Product> products)
        {
            List<ProductListVM> productList = new List<ProductListVM>();
            foreach (var product in products)
            {
                ProductListVM newProduct = new ProductListVM {
                    Id = product.Id,
                    Title = product.Title,
                    Description = product.Description,
                    MainImage = product.ProductImages.Where(m=>m.IsMain).FirstOrDefault()?.Image,
                    CategoryName = product.Category.Name,
                    Price = product.Price                
                };
                productList.Add(newProduct);
            }
            return productList;
        }
    }
}
