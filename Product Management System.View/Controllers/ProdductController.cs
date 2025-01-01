using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product_Management_System.Application.Dtos;
using Product_Management_System.Application.Service;

namespace Product_Management_System.View.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(int pageItem = 10, int pageNumber = 1)
        {
            try
            {
                if (pageNumber < 1)
                {
                    return NoContent();
                }
                var products = await _productService.GetAllPagination(pageItem, pageNumber);

                if (products.Count == 0)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(products);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CreateOrUpdateProductDtos createOrUpdateProduct)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.Create(createOrUpdateProduct);
                return Ok(res);
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> HardDeleteProduct(int id)
        {
            if (id > 0)
            {
                var DeletedProduct = await _productService.HardDelete(id);
                if (DeletedProduct.IsSuccess)
                {
                    return Ok(DeletedProduct.Message);
                }
                return BadRequest(DeletedProduct.Message);
            }
            return BadRequest("Enter Valid Id !");
        }


        [HttpDelete("SoftDeleteProduct")]
        public async Task<IActionResult> SoftDeleteProduct(int ProductId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var DeletedProduct = await _productService.SoftDelete(ProductId);
            if (DeletedProduct.IsSuccess)
            {
                return Ok(DeletedProduct.Entity);
            }
            return BadRequest(DeletedProduct.Message);
        }


        [HttpPut]
        public async Task<ActionResult> Update([FromForm] CreateOrUpdateProductDtos createOrUpdateProduct)
        {
            if (ModelState.IsValid)
            {
                var Product = await _productService.Update(createOrUpdateProduct);
                return Ok("Updated Successfully!");
            }
            return BadRequest(ModelState);
        }
        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName(string Name, int ItemsPerPage, int PageNumber)
        {
            if (ModelState.IsValid)
            {
                var products = await _productService.SearchProduct(Name, ItemsPerPage, PageNumber);
                return Ok(products);
            }
            return BadRequest(ModelState);
        }
    }
}
