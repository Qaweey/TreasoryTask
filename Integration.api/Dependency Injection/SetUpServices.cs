using ApiIntegration.Core;

namespace Integration.api.Dependency_Injection
{
    public static  class SetUpServices
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services,WebApplicationBuilder builder, IConfiguration configuration)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            //services.AddScoped<IApiService, ApiService>();
            services.AddHttpClient<IApiService, ApiService>();
            services.AddConfigurationSettings(configuration);
            return services;
        }

        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<ApiCredential>(configuration.GetSection("ApiCredential"));
            return services;
        }
       
    }
}
