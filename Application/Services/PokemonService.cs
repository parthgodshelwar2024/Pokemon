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
        private readonly IAiStoryService _aiStoryService;

        public PokemonService(
            IPokemonRepository repository,
            ICacheService cache,
            IAiStoryService aiStoryService)
        {
            _repository = repository;
            _cache = cache;
            _aiStoryService = aiStoryService;
        }

        public async Task<string> GetPokemonStoryAsync(
            string organization,
            string name)
        {
            var cacheKey = $"org:{organization}:pokemon:{name}:story";

            var cached = await _cache.GetAsync<string>(cacheKey);
            if (cached != null)
                return cached;

            var story = await _aiStoryService.GenerateStoryAsync(name);

            await _cache.SetAsync(
                cacheKey,
                story,
                TimeSpan.FromHours(1));

            return story;
        }

        public async Task<IEnumerable<PokemonDetailDto>> GetPokemonsAsync(string organization, int pageNumber)
        {
            var cacheKey = $"org:{organization}:pokemons:list-page{pageNumber}";

            var cached = await _cache.GetAsync<IEnumerable<PokemonDetailDto>>(cacheKey);
            if (cached != null)
                return cached;
            
            var pokemons = await _repository.GetPokemonsAsync(MaxLimit, pageNumber);

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
