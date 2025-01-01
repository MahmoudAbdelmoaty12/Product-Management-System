
using Product_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Management_System.Application.Contract
{
    public interface IProductRepository : IRepository<Product, int>
    {
        Task<IQueryable<Product>> SearchProduct(string Name);
        Task<IQueryable<Image>> GetImagesByProductId(int ProductId);
      
    }
}
