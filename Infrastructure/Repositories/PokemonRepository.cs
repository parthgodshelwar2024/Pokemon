using Application.DTOs;
using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{    
    public class PokemonRepository : IPokemonRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PokemonRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<PokemonDetailDto>> GetPokemonsAsync(int limit, int? pageNumber=0)
        {
            var client = _httpClientFactory.CreateClient("PokeApi");
            var offset = limit * pageNumber;

            var response = await client.GetAsync($"pokemon?limit={limit}&offset={offset}");
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            var json = await JsonDocument.ParseAsync(stream);

            var results = json.RootElement.GetProperty("results");

            var tasks = results.EnumerateArray().Select(async item =>
            {
                var detailResponse = await client.GetAsync($"pokemon/{item.GetProperty("name").GetString()}");
                detailResponse.EnsureSuccessStatusCode();

                using var detailStream = await detailResponse.Content.ReadAsStreamAsync();
                var detailJson = await JsonDocument.ParseAsync(detailStream);

                return MapToDetailDto(detailJson);
            });

            return await Task.WhenAll(tasks);
        }

        public async Task<PokemonSearchDto?> GetPokemonAsync(string idOrName)
        {
            var client = _httpClientFactory.CreateClient("PokeApi");

            var response = await client.GetAsync($"pokemon/{idOrName}");
            if (!response.IsSuccessStatusCode)
                return null;

            using var stream = await response.Content.ReadAsStreamAsync();
            var json = await JsonDocument.ParseAsync(stream);

            return new PokemonSearchDto
            {
                Name = json.RootElement.GetProperty("name").GetString(),
                Img_Url = json.RootElement
                    .GetProperty("sprites")
                    .GetProperty("back_default")
                    .GetString()
            };
        }

        private static PokemonDetailDto MapToDetailDto(JsonDocument json)
        {
            var root = json.RootElement;

            var abilities = string.Join(", ",
                root.GetProperty("abilities")
                    .EnumerateArray()
                    .Select(a => a.GetProperty("ability").GetProperty("name").GetString()));

            var type = root.GetProperty("types")
                .EnumerateArray()
                .First()
                .GetProperty("type")
                .GetProperty("name")
                .GetString();

            return new PokemonDetailDto
            {
                Name = root.GetProperty("name").GetString(),
                Order = root.GetProperty("order").GetInt32(),
                Abilities = abilities,
                Type = type
            };
        }
    }
}
