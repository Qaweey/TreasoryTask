using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Core
{
    public  class ProductRepository : IProductRepository
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ProductRepository> _logger;
        private readonly ApiCredential _settings;
        public ProductRepository(IApiService apiService, IOptions<ApiCredential> settings,ILogger<ProductRepository> logger)
        {
            _apiService = apiService;
            _logger = logger;
            _settings = settings.Value;
        }
        public async  Task<BaseServiceResponse<object>> CreateProduct(CreateProductRequestDto createObjectRequestDto)
        {
            try
            {
                var result = await _apiService.SendRequestAsync<CreateProductResponseDto>(
    HttpMethod.Post,
    _settings.BaseUrl,
    requestBody: createObjectRequestDto);

                if(result == null)
                {
                    return new BaseServiceResponse<object>()
                    {
                        Code = StatusCodes.BadRequest,
                        Message = StatusMessages.BadRequest,
                    };
                }
                result.Name= createObjectRequestDto.Name;   
                result.Data=createObjectRequestDto.Data;
                return new BaseServiceResponse<object>()
                {

                    Code = StatusCodes.Created,
                    Message = StatusMessages.Created,
                    Data = result
                };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BaseServiceResponse<object>()
                {

                    Code = StatusCodes.InternalServerError,
                    Message = StatusMessages.ServerError,
                };

            }
        }

        public async  Task<BaseServiceResponse<object>> GetProduct(string? search, int pageSize, int pageNumber)
        {
            try
            {
                var result = await _apiService.SendRequestAsync<List<GetProductResponse>>(
 HttpMethod.Get,
 _settings.BaseUrl);

                if (result == null)
                {
                    return new BaseServiceResponse<object>()
                    {
                        Code = StatusCodes.BadRequest,
                        Message = StatusMessages.BadRequest,
                    };
                }
                if(result.Count == 0)
                {
                    return new BaseServiceResponse<object>()
                    {

                        Code = StatusCodes.Success,
                        Message = StatusMessages.Success,
                        Data = result
                    };
                }
                if (!string.IsNullOrEmpty(search))
                {
                    result = result.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                result=result.Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToList();


                return new BaseServiceResponse<object>()
                {

                    Code = StatusCodes.Success,
                    Message = StatusMessages.Success,
                    Data = result
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BaseServiceResponse<object>()
                {

                    Code = StatusCodes.InternalServerError,
                    Message = StatusMessages.ServerError,
                };
            }
        }

        public async  Task<BaseServiceResponse<object>> RemoveProduct(string Id)
        {
            try
            {
                var result = await _apiService.SendRequestAsync<GetProductResponse>(
 HttpMethod.Get,
 $"{_settings.BaseUrl}/{Id}");
                if (result==null)
                {
                    return new BaseServiceResponse<object>()
                    {

                        Code = StatusCodes.InternalServerError,
                        Message = StatusMessages.ServerError,
                    };
                }

                if (result.Id == null)
                {
                    return new BaseServiceResponse<object>()
                    {
                        Code = StatusCodes.NotFound,
                        Message = StatusMessages.NotFound,
                    };
                }

                var delResult = await _apiService.SendRequestAsync<DeleteProductResponse>(
HttpMethod.Delete,
$"{_settings.BaseUrl}/{Id}");

                if (delResult == null)
                {
                    return new BaseServiceResponse<object>()
                    {
                        Code = StatusCodes.BadRequest,
                        Message = StatusMessages.BadRequest,
                    };
                }


                return new BaseServiceResponse<object>()
                {

                    Code = StatusCodes.Success,
                    Message = StatusMessages.Success,
                    Data = delResult
                };


            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new BaseServiceResponse<object>()
                {

                    Code = StatusCodes.InternalServerError,
                    Message = StatusMessages.ServerError,
                };
            }
        }
    }
}
