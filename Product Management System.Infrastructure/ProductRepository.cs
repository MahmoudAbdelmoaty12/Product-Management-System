using Microsoft.EntityFrameworkCore;
using Product_Management_System.Application.Contract;
using Product_Management_System.Context;
using Product_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Management_System.Infrastructure
{
    public class ProductRepository : Repository<Product, int>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<Image>> GetImagesByProductId(int ProductId)
        {

            var images = _context.Products
                        .Where(p => p.Id == ProductId)
                        .SelectMany(p => p.Images);
            return images;
        }
       
        public Task<IQueryable<Product>> SearchProduct(string Name)
        {
            Name = Name.ToLower();
            return Task.FromResult(_entities.Where(p => p.Name.Contains(Name)));
        }
    }
}
