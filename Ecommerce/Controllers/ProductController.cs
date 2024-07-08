using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork<Product> unitOfWork;
        public ApiRespons respons;

        public ProductController(IUnitOfWork<Product> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            respons = new ApiRespons();
        }

        [HttpGet]
        public async Task<ApiRespons> GetAll()
        {
            var model = await unitOfWork.ProductRepository.GetAll();
            var check = model.Any();
            if(check)
            {
                respons.StatusCode = System.Net.HttpStatusCode.OK;
                respons.IsSuccess = true;
                respons.Result = model;
                return respons;
            }
            else
            {
                respons.ErrorMessages = "no products found";
                respons.StatusCode = System.Net.HttpStatusCode.OK;
                respons.IsSuccess = false;
                return respons;
            }
        }

        [HttpGet("{id}")]
        public async Task<ApiRespons> GetById(int id)
        {
            var model = await unitOfWork.ProductRepository.GetById(id);
            if (model != null)
            {
                respons.StatusCode = System.Net.HttpStatusCode.OK;
                respons.IsSuccess = true;
                respons.Result = model;
            }
            else
            {
                respons.ErrorMessages = "no products found";
                respons.StatusCode = System.Net.HttpStatusCode.OK;
                respons.IsSuccess = false;
            }
            return respons;
        }

        [HttpPost]
        public async Task<ApiRespons> CreateProduct(Product model)
        {
            if(model != null)
            {
                await unitOfWork.ProductRepository.Create(model);
                await unitOfWork.Save();
                respons.StatusCode = System.Net.HttpStatusCode.OK;
                respons.Result = model;
                respons.IsSuccess= true;
            }
            else
            {
                respons.StatusCode = System.Net.HttpStatusCode.OK;
                respons.IsSuccess = false;
                respons.ErrorMessages = "Create Failed";
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
                respons.StatusCode = System.Net.HttpStatusCode.OK;
                respons.Result = model;
                respons.IsSuccess = true;
            }
            else
            {
                respons.StatusCode = System.Net.HttpStatusCode.OK;
                respons.IsSuccess = false;
                respons.ErrorMessages = "Update Failed";
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
                respons.StatusCode = System.Net.HttpStatusCode.OK;
                respons.IsSuccess = true;
            }
            else
            {
                respons.StatusCode = System.Net.HttpStatusCode.OK;
                respons.IsSuccess = false;
                respons.ErrorMessages = "Delete Failed";
            }
            return respons;
        }
    }
}
