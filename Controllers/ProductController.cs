using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using u21442453_HW03_API.Models;

namespace u21442453_HW03_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository _repository;

        public ProductController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var result = await _repository.GetAllProductsAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error, please contact support");
            }
        }

        [HttpGet]
        [Route("GetAllProductTypes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllProductTypes()
        {
            try
            {
                var result = await _repository.GetAllProductTypesAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error, please contact support");
            }
        }

        [HttpGet]
        [Route("GetAllBrands")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllBrands()
        {
            try
            {
                var result = await _repository.GetAllBrandsAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error, please contact support");
            }
        }


        [HttpPost]
        [Route("AddProduct")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddProduct(Product model)
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                BrandId = model.BrandId,
                ProductTypeId = model.ProductTypeId,
                Description = model.Description,
                Image = model.Image,
            };
            try
            {
                _repository.Add(product);
                if(await _repository.SaveChangesAsync())
                {
                    return Ok(new { product , status="success"});
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error, please contact support");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error, please contact support");
        }
    }
}
