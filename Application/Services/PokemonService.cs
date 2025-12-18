using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;


namespace Application.Services
{
    public class PokemonService : IPokemonService
    {
        private const int MaxLimit = 10;

        private readonly IPokemonRepository _repository;
        private readonly ICacheService _cache;

        public PokemonService(
            IPokemonRepository repository,
            ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<IEnumerable<PokemonDetailDto>> GetPokemonsAsync(string organization)
        {
            var cacheKey = $"org:{organization}:pokemons:list";

            var cached = await _cache.GetAsync<IEnumerable<PokemonDetailDto>>(cacheKey);
            if (cached != null)
                return cached;

            var pokemons = await _repository.GetPokemonsAsync(MaxLimit);

            await _cache.SetAsync(
                cacheKey,
                pokemons,
                TimeSpan.FromMinutes(5));

            return pokemons;
        }

        public async Task<IEnumerable<PokemonSearchDto>> SearchPokemonAsync(
            string organization,
            string? id,
            string? name)
        {
            var identifier = id ?? name;
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Either id or name must be provided");

            var cacheKey = $"org:{organization}:pokemon:{identifier}";

            var cached = await _cache.GetAsync<IEnumerable<PokemonSearchDto>>(cacheKey);
            if (cached != null)
                return cached;

            var pokemon = await _repository.GetPokemonAsync(identifier);
            if (pokemon == null)
                return Enumerable.Empty<PokemonSearchDto>();

            var result = new List<PokemonSearchDto> { pokemon };

            await _cache.SetAsync(
                cacheKey,
                result,
                TimeSpan.FromMinutes(30));

            return result;
        }
    }
}
