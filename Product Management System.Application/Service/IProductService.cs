using Product_Management_System.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Management_System.Application.Service
{
    public interface IProductService
    {
        Task<ResultView<ProductDto>> Create(CreateOrUpdateProductDtos productDto);
        Task<ResultView<ProductDto>> Update(CreateOrUpdateProductDtos productDto);
        Task<ResultDataList<GetAllProductsDtos>> GetAllPagination(int ItemsPerPage, int PageNumber);
        Task<ResultDataList<GetAllProductsDtos>> SearchProduct(string Name, int ItemsPerPage, int PageNumber);
        Task<ResultView<ProductDto>> SoftDelete(int productId);
        Task<ResultView<ProductDto>> HardDelete(int productId);
    }
}
