using Application.Interfaces.Services;
using Infrastructure.Configuration;
using Infrastructure.External;

namespace PokemonApi.Extensions
{
    public static class OpenAiExtensions
    {
        public static IServiceCollection AddOpenAi(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<OpenAiOptions>(
                configuration.GetSection("OpenAI"));

            services.AddHttpClient<IAiStoryService, OpenAiStoryService>();

            return services;
        }
    }
}
