using Application.Interfaces.Services;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.External
{
    //public class OpenAiStoryService : IAiStoryService
    //{
    //    private readonly HttpClient _httpClient;
    //    private readonly OpenAiOptions _options;

    //    public OpenAiStoryService(
    //        HttpClient httpClient,
    //        IOptions<OpenAiOptions> options)
    //    {
    //        _httpClient = httpClient;
    //        _options = options.Value;

    //        _httpClient.DefaultRequestHeaders.Authorization =
    //            new AuthenticationHeaderValue("Bearer", _options.ApiKey);
    //    }

    //    public async Task<string> GenerateStoryAsync(string pokemonName)
    //    {
    //        var prompt =
    //            $"Create a short, imaginative and friendly story " +
    //            $"about the Pokémon '{pokemonName}'. " +
    //            $"The story must be maximum 500 characters.";

    //        var request = new
    //        {
    //            model = _options.Model,
    //            messages = new[]
    //            {
    //            new { role = "user", content = prompt }
    //        },
    //            max_tokens = _options.MaxTokens
    //        };

    //        var content = new StringContent(
    //            JsonSerializer.Serialize(request),
    //            Encoding.UTF8,
    //            "application/json");

    //        var response = await _httpClient.PostAsync(
    //            "https://api.openai.com/v1/chat/completions",
    //            content);

    //        response.EnsureSuccessStatusCode();

    //        using var stream = await response.Content.ReadAsStreamAsync();
    //        var json = await JsonDocument.ParseAsync(stream);

    //        var story = json.RootElement
    //            .GetProperty("choices")[0]
    //            .GetProperty("message")
    //            .GetProperty("content")
    //            .GetString();

    //        return story!.Length > 500
    //            ? story[..500]
    //            : story;
    //    }
    //}

    public class OpenAiStoryService : IAiStoryService
    {
        private readonly HttpClient _httpClient;
        private readonly AiOptions _options;

        public OpenAiStoryService(
            HttpClient httpClient,
            IOptions<AiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            //_httpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", _options.ApiKey);
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _options.OpenAiApiKey);
        }

        public async Task<string> GenerateStoryAsync(string pokemonName)
        {
            var prompt =
                $"Create a short, imaginative and friendly story " +
                $"about the Pokémon '{pokemonName}'. Max 500 characters.";

            var request = new
            {
                model = _options.Model,
                messages = new[]
                {
                new { role = "user", content = prompt }
            },
                max_tokens = _options.MaxTokens
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"AiApi Error: {response.StatusCode}");

            using var stream = await response.Content.ReadAsStreamAsync();
            using var json = await JsonDocument.ParseAsync(stream);

            var story = json.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return story!.Length > 500 ? story[..500] : story;
        }
    }

}
