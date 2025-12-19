using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface IPokemonService
    {
        Task<IEnumerable<PokemonDetailDto>> GetPokemonsAsync(string organization, int pageNumber);
        Task<IEnumerable<PokemonSearchDto>> SearchPokemonAsync(
            string organization,
            string? id,
            string? name);
    }
}
