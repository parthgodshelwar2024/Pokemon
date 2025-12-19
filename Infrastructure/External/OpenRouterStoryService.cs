using Application.Interfaces.Services;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Infrastructure.External;

public class OpenRouterStoryService : IAiStoryService
{
    private readonly HttpClient _httpClient;
    private readonly AiOptions _options;

    public OpenRouterStoryService(
        HttpClient httpClient,
        IOptions<AiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

        _httpClient.BaseAddress =
            new Uri("https://openrouter.ai/api/v1/");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _options.OpenRouterApiKey);
    }

    public async Task<string> GenerateStoryAsync(string pokemonName)
    {
        var request = new
        {
            model = _options.Model,
            messages = new[]
            {
                new
                {
                    role = "user",
                    //content = $"Write a short Pokémon story (max 500 chars) about {pokemonName}"
                    content = $"Create a short, imaginative and friendly story " +  
                              $"about the Pokémon '{pokemonName}'. Max 500 characters."
    }
            },
            reasoning = new { enabled = true }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(
            "chat/completions",
            content);

        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception("OpenRouter request failed");

        using var json = JsonDocument.Parse(body);

        var story = json.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return story!.Length > 500 ? story[..500] : story;
    }
}
