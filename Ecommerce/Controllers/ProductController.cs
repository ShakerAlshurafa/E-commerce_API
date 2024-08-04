using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.mapping_profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork<Product> unitOfWork;
        private readonly IMapper mapper;
        public ApiRespons respons;

        public ProductController(IUnitOfWork<Product> unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            respons = new ApiRespons();
        }

        [HttpGet]
        //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        [ResponseCache(CacheProfileName = ("defaultCache"))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        public async Task<ApiRespons> GetAllProducts(int PageSize = 2, int PageNumber = 1, [FromQuery] string? categoryName = null)
        {
            Expression<Func<Product, bool>> filter = null;
            if(!string.IsNullOrEmpty(categoryName))
            {
                filter = x => x.Category.Name.Contains(categoryName);
            }
            var model = await unitOfWork.ProductRepository.GetAll(page_size : PageSize, page_number : PageNumber, includeProperty : "Category", filter : filter);
            var check = model.Any();
            if(check)
            {
                respons.StatusCode = 200;
                respons.IsSuccess = true;
                respons.Result = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>> (model);
                return respons;
            }
            else
            {
                respons.Message = "no products found";
                respons.StatusCode = 200;
                respons.IsSuccess = false;
                return respons;
            }
        }

        [HttpGet("{get_id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiRespons>> GetById([FromQuery] int id)
        {
            try
            {
                if(id <= 0)
                {
                    return BadRequest(new ApiValidationResponse(new List<string> { "Invalid ID", "Try Positive Integer"}, 400));
                }
                var model = await unitOfWork.ProductRepository.GetById(id);
                if (model == null)
                {
                    return NotFound(new ApiRespons(404, "Product Not Found"));
                }
                else
                {
                    return Ok(new ApiRespons(200, result : model));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ApiValidationResponse(new List<string> { "Internal Server Error", ex.Message }, StatusCodes.Status500InternalServerError));
            }
            
        }

        [HttpPost]
        public async Task<ApiRespons> CreateProduct(Product model)
        {
            if(model != null)
            {
                await unitOfWork.ProductRepository.Create(model);
                await unitOfWork.Save();
                respons.StatusCode = 200;
                respons.Result = model;
                respons.IsSuccess= true;
            }
            else
            {
                respons.StatusCode = 200;
                respons.IsSuccess = false;
                respons.Message = "Create Failed";
            }
            return respons;
        }

        [HttpPut]
        public async Task<ApiRespons> UpdateProduct(Product model)
        {
            if (model != null)
            {
                unitOfWork.ProductRepository.Update(model);
                await unitOfWork.Save();
                respons.StatusCode = 200;
                respons.Result = model;
                respons.IsSuccess = true;
            }
            else
            {
                respons.StatusCode = 200;
                respons.IsSuccess = false;
                respons.Message = "Update Failed";
            }
            return respons;
        }

        [HttpDelete]
        public async Task<ApiRespons> DeleteProduct(int id)
        {
            var check = unitOfWork.ProductRepository.Delete(id);
            if (check)
            {
                await unitOfWork.Save();
                respons.StatusCode = 200;
                respons.IsSuccess = true;
            }
            else
            {
                respons.StatusCode = 200;
                respons.IsSuccess = false;
                respons.Message = "Delete Failed";
            }
            return respons;
        }

        [HttpGet("Product/{categoryId}")]
        public async Task<ActionResult<ApiRespons>> GetAllProductsByCategory(int categoryId)
        {
            var products = await unitOfWork.ProductRepository.GetAllProductsByCategory(categoryId);
            var mappedProducts = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>> (products);
            return Ok(mappedProducts);
        }
    }
}
