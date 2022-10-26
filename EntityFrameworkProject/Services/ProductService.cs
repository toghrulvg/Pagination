using EntityFrameworkProject.Data;
using EntityFrameworkProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAll(int take)
        {
            return await _context.Products.Where(m => !m.IsDeleted).Include(m => m.ProductImages).Take(take).OrderBy(m => m.Id).ToListAsync();
        }
    }
}
