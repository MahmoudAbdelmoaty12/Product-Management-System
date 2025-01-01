
using AutoMapper;
using Microsoft.AspNetCore.Http;

using Product_Management_System.Application.Contract;
using Product_Management_System.Application.Dtos;
using Product_Management_System.Application.Service;
using Product_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _mapper = mapper;
            _productRepository = productRepository;

        }
        private async Task<List<string>> SaveProductImages(List<IFormFile> images)
        {
            var imagePaths = new List<string>();
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            foreach (var image in images)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(uploadDirectory, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                imagePaths.Add(fileName);  
            }

            return imagePaths;
        }

        public async Task<ResultView<ProductDto>> Create(CreateOrUpdateProductDtos productDto)
        {
            try
            {
                var OldProduct = (await _productRepository.GetAllAsync())
                                 .Where(p => p.Name == productDto.Name).FirstOrDefault();
                if (OldProduct != null)
                {
                    return new ResultView<ProductDto>
                    {
                        Entity = null,
                        IsSuccess = false,
                        Message = "Product Already Exists !"
                    };
                }

                var createorupdateproduct = new CreateOrUpdateProductDtos
                {

                    Description = productDto.Description,
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Images = productDto.Images,



                };

                var product = _mapper.Map<Product>(createorupdateproduct);

                var imagePaths = await SaveProductImages(productDto.Images);
                product.Images = new List<Image>();
                foreach (var imagePath in imagePaths)
                {
                    product.Images.Add(new Image
                    {
                        Name = imagePath,
                        ProductId = product.Id  
                    });
                }


                //add product
                var AddedProduct = await _productRepository.CreateAsync(product);
                await _productRepository.SaveChangesAsync();


                var newProduct = _mapper.Map<ProductDto>(AddedProduct);

                return new ResultView<ProductDto>
                {
                    Entity = newProduct,
                    IsSuccess = true,
                    Message = "Product Added Successfully !"
                };
            }catch (Exception ex)
            {
                return new ResultView<ProductDto>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResultView<ProductDto>> Update(CreateOrUpdateProductDtos productDto)
        {
            try
            {
                var OldProduct = await _productRepository.GetByIdAsync(productDto.Id);

              

                if (OldProduct == null)
                {
                    return new ResultView<ProductDto>
                    {
                        Entity = null,
                        IsSuccess = false,
                        Message = "Product doesn't exist & failed to update!"
                    };
                }

                
                _mapper.Map(productDto, OldProduct);

              
          var UpdatedProductDto=await _productRepository.UpdateAsync(OldProduct);
                await _productRepository.SaveChangesAsync();

                var NewUpdatedProductDto = _mapper.Map<ProductDto>(UpdatedProductDto);

                return new ResultView<ProductDto>
                {
                    Entity = NewUpdatedProductDto,
                    IsSuccess = true,
                    Message = "Product updated successfully!"
                };
            }
            catch (Exception ex)
            {
                return new ResultView<ProductDto>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ResultDataList<GetAllProductsDtos>> GetAllPagination(int ItemsPerPage, int PageNumber)
        {
            if (PageNumber > 0)
            {

                var products = (await _productRepository.GetAllAsync())
                               .Where(p => p.IsDeleted == false)
                               .Skip(ItemsPerPage * (PageNumber - 1))
                               .Take(ItemsPerPage)
                               .Select(p => new GetAllProductsDtos
                               {
                                   Id = p.Id,
                                   Name= p.Name,
                                   Description = p.Description,
                                   Price = p.Price,
                                   CreatedDate=p.CreatedAt,
                                   IsDeleted = p.IsDeleted,
                                   Images = p.Images.Select(i => i.Name).ToList()
                               }).ToList();

                var productscount = (await _productRepository.GetAllAsync())
                               .Where(p => p.IsDeleted == false).Count();
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = products,
                    Count = productscount
                };
                return resultDataList;
            }
            else
            {
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
                return resultDataList;
            }

        }

        public async Task<ResultDataList<GetAllProductsDtos>> SearchProduct(string Name, int ItemsPerPage, int PageNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    throw new ArgumentException("Name cannot be empty or whitespace.");
                }

                if (PageNumber <= 0)
                {
                    throw new ArgumentException("Page number must be greater than zero");
                }

                var products = (await _productRepository.SearchProduct(Name))
                               .Where(p => p.IsDeleted == false)
                               .Skip(ItemsPerPage * (PageNumber - 1)).Take(ItemsPerPage)
                               .Select(p => new GetAllProductsDtos
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   Description = p.Description,
                                   Price = p.Price,
                                   CreatedDate=p.CreatedAt,
                                   IsDeleted = p.IsDeleted,
                                   Images = p.Images.Select(x => x.Name).ToList()
                               }).ToList();
                var totalCount = (await _productRepository.SearchProduct(Name))
                               .Where(p => !p.IsDeleted)
                               .Count();

                var ProductsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = ProductsDto,
                    Count = totalCount
                };
                return resultDataList;
            }
            catch (Exception ex)
            {
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
                return resultDataList;
            }

        }

        public async Task<ResultView<ProductDto>> SoftDelete(int productId)
        {
            var OldProduct = (await _productRepository.GetByIdAsync(productId));
            if (OldProduct != null)
            {
                OldProduct.IsDeleted = true;
                var product = await _productRepository.UpdateAsync(OldProduct);
                await _productRepository.SaveChangesAsync();



                var DeletedProductDto = _mapper.Map<ProductDto>(product);




                return new ResultView<ProductDto>
                {
                    Entity = DeletedProductDto,
                    IsSuccess = true,
                    Message = "Product Deleted Successfully !"

                };
            }

            return new ResultView<ProductDto>
            {
                Entity = null,
                IsSuccess = false,
                Message = "failed to delete this product !"

            };
        }

        public async Task<ResultView<ProductDto>> HardDelete(int productId)
        {
            var OldProduct = (await _productRepository.GetByIdAsync(productId));
            if (OldProduct != null)
            {

                var product = await _productRepository.DeleteAsync(OldProduct);
                await _productRepository.SaveChangesAsync();



                var DeletedProductDto = _mapper.Map<ProductDto>(product);




                return new ResultView<ProductDto>
                {
                    Entity = DeletedProductDto,
                    IsSuccess = true,
                    Message = "Product Deleted Successfully !"

                };
            }

            return new ResultView<ProductDto>
            {
                Entity = null,
                IsSuccess = false,
                Message = "failed to delete this product !"

            };
        }

       

    }
}
