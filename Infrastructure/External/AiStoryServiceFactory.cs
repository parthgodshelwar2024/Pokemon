using Application.Interfaces.Services;
using Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Infrastructure.External
{
    public class AiStoryServiceFactory : IAiStoryService
    {
        private readonly IServiceProvider _provider;
        private readonly AiOptions _options;

        public AiStoryServiceFactory(
            IServiceProvider provider,
            IOptions<AiOptions> options)
        {
            _provider = provider;
            _options = options.Value;
        }

        public Task<string> GenerateStoryAsync(string pokemonName)
        {
            return _options.Provider switch
            {
                "OpenRouter" when !string.IsNullOrEmpty(_options.OpenRouterApiKey)
                    => _provider.GetRequiredService<OpenRouterStoryService>()
                                .GenerateStoryAsync(pokemonName),

                "OpenAI" when !string.IsNullOrEmpty(_options.OpenAiApiKey)
                    => _provider.GetRequiredService<OpenAiStoryService>()
                                .GenerateStoryAsync(pokemonName),

                _ => throw new InvalidOperationException(
                    "AI provider is not configured correctly")
            };
        }
    }
}
