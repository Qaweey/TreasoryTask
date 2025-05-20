using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Core
{
    public interface IProductRepository
    {
       Task <BaseServiceResponse<object>> CreateProduct(CreateProductRequestDto createObjectRequestDto);

        Task<BaseServiceResponse<object>> GetProduct( string search, int pageSize, int pageNumber);
        Task<BaseServiceResponse<object>> RemoveProduct(string Id);
    }
}
