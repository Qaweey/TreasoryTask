using ApiIntegration.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Integration.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpPost]
        public async  Task< IActionResult> CreateProduct([FromBody]CreateProductRequestDto dto)
        {
           var result=  await _productRepository.CreateProduct(dto);
          return  result.Code!= ApiIntegration.Core.StatusCodes.Created?BadRequest(result):Ok(result);
                    
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(string Id)
        {
            var result = await _productRepository.RemoveProduct(Id);
            return result.Code != ApiIntegration.Core.StatusCodes.Success ? BadRequest(result) : Ok(result);

        }
        [HttpGet]   
        public async Task<IActionResult> GetProducts(string? search, int pageSize=10, int pageNumber=1)
        {
            var result = await _productRepository.GetProduct(search,pageSize,pageNumber);
            return result.Code != ApiIntegration.Core.StatusCodes.Success ? BadRequest(result) : Ok(result);

        }
    }
}
