using Application.Interfaces.Services;
using Infrastructure.Configuration;
using Infrastructure.External;

namespace PokemonApi.Extensions
{
    public static class AiExtensions
    {
        public static IServiceCollection AddAi(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<AiOptions>(
                configuration.GetSection("AI"));

            services.AddHttpClient<OpenAiStoryService>();
            services.AddHttpClient<OpenRouterStoryService>();

            services.AddScoped<IAiStoryService, AiStoryServiceFactory>();

            return services;
        }
    }
}
