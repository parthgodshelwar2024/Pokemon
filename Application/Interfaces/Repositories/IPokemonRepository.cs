using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IPokemonRepository
    {
        Task<IEnumerable<PokemonDetailDto>> GetPokemonsAsync(int limit,int? pageNumber);
        Task<PokemonSearchDto?> GetPokemonAsync(string idOrName);
    }
    
}
