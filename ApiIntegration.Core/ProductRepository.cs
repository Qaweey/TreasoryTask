using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Core
{
    public class ProductRepository : IProductRepository
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ProductRepository> _logger;
        private readonly ApiCredential _settings;

        public ProductRepository(IApiService apiService, IOptions<ApiCredential> settings, ILogger<ProductRepository> logger)
        {
            _apiService = apiService;
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<BaseServiceResponse<object>> CreateProduct(CreateProductRequestDto createObjectRequestDto)
        {
            _logger.LogInformation("Creating product: {ProductName}", createObjectRequestDto.Name);

            try
            {
                var result = await _apiService.SendRequestAsync<CreateProductResponseDto>(
                    HttpMethod.Post,
                    _settings.BaseUrl,
                    requestBody: createObjectRequestDto);

                if (result == null)
                {
                    _logger.LogWarning("API returned null response while creating product: {ProductName}", createObjectRequestDto.Name);
                    return new BaseServiceResponse<object>
                    {
                        Code = StatusCodes.BadRequest,
                        Message = StatusMessages.BadRequest
                    };
                }

                result.Name = createObjectRequestDto.Name;
                result.Data = createObjectRequestDto.Data;

                _logger.LogInformation("Product created successfully: {ProductName}", result.Name);
                return new BaseServiceResponse<object>
                {
                    Code = StatusCodes.Created,
                    Message = StatusMessages.Created,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating product: {ProductName}", createObjectRequestDto.Name);
                return new BaseServiceResponse<object>
                {
                    Code = StatusCodes.InternalServerError,
                    Message = StatusMessages.ServerError
                };
            }
        }

        public async Task<BaseServiceResponse<object>> GetProduct(string? search, int pageSize, int pageNumber)
        {
            _logger.LogInformation("Fetching products with search='{Search}', pageSize={PageSize}, pageNumber={PageNumber}", search, pageSize, pageNumber);

            try
            {
                var result = await _apiService.SendRequestAsync<List<GetProductResponse>>(
                    HttpMethod.Get,
                    _settings.BaseUrl);

                if (result == null)
                {
                    _logger.LogWarning("API returned null when fetching products.");
                    return new BaseServiceResponse<object>
                    {
                        Code = StatusCodes.BadRequest,
                        Message = StatusMessages.BadRequest
                    };
                }

                if (!string.IsNullOrEmpty(search))
                {
                    result = result.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                var pagedResult = result.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                _logger.LogInformation("Successfully fetched {Count} products.", pagedResult.Count);
                return new BaseServiceResponse<object>
                {
                    Code = StatusCodes.Success,
                    Message = StatusMessages.Success,
                    Data = pagedResult
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products.");
                return new BaseServiceResponse<object>
                {
                    Code = StatusCodes.InternalServerError,
                    Message = StatusMessages.ServerError
                };
            }
        }

        public async Task<BaseServiceResponse<object>> RemoveProduct(string Id)
        {
            _logger.LogInformation("Attempting to delete product with ID = {ProductId}", Id);

            try
            {
                var result = await _apiService.SendRequestAsync<GetProductResponse>(
                    HttpMethod.Get,
                    $"{_settings.BaseUrl}/{Id}");

                if (result == null || result.Id == null)
                {
                    _logger.LogWarning("Product with ID = {ProductId} not found.", Id);
                    return new BaseServiceResponse<object>
                    {
                        Code = StatusCodes.NotFound,
                        Message = StatusMessages.NotFound
                    };
                }

                _logger.LogInformation("Product found. Proceeding to delete product with ID = {ProductId}", Id);

                var delResult = await _apiService.SendRequestAsync<DeleteProductResponse>(
                    HttpMethod.Delete,
                    $"{_settings.BaseUrl}/{Id}");

                if (delResult == null)
                {
                    _logger.LogWarning("API returned null while deleting product with ID = {ProductId}", Id);
                    return new BaseServiceResponse<object>
                    {
                        Code = StatusCodes.BadRequest,
                        Message = StatusMessages.BadRequest
                    };
                }

                _logger.LogInformation("Product with ID = {ProductId} deleted successfully.", Id);
                return new BaseServiceResponse<object>
                {
                    Code = StatusCodes.Success,
                    Message = StatusMessages.Success,
                    Data = delResult
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID = {ProductId}", Id);
                return new BaseServiceResponse<object>
                {
                    Code = StatusCodes.InternalServerError,
                    Message = StatusMessages.ServerError
                };
            }
        }
    }
}
