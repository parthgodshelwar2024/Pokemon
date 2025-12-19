using Application.Common;
using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;



namespace PokemonApi.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class PokemonsController : BaseController
    {
        private readonly IPokemonService _pokemonService;
        private readonly ILogger<PokemonsController> _logger;

        public PokemonsController(
        IPokemonService pokemonService,
        ILogger<PokemonsController> logger)
        {
            _pokemonService = pokemonService;
            _logger = logger;
        }

        /// <summary>
        /// Get list of pokemons (max 10)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PokemonDetailDto>>), 200)]
        public async Task<IActionResult> GetPokemons([FromQuery] int pageNumber=0)
        {
            _logger.LogInformation(
                "Fetching pokemon list for org {Org}", Organization);

            var result = await _pokemonService.GetPokemonsAsync(Organization, pageNumber);
            //var result = new List<PokemonDetailDto>();

            return Ok(ApiResponse<IEnumerable<PokemonDetailDto>>
                .SuccessResponse(
                    result,
                    "Pokemons fetched successfully",
                    CorrelationId));
        }

        /// <summary>
        /// Search pokemon by id or name
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PokemonSearchDto>>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SearchPokemon(
            [FromQuery] string? id,
            [FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(ApiResponse<string>.Failure(
                    "Either id or name must be provided",
                    CorrelationId));
            }

            var result = await _pokemonService.SearchPokemonAsync(
                Organization, id, name);

            return Ok(ApiResponse<IEnumerable<PokemonSearchDto>>
                .SuccessResponse(
                    result,
                    "Search completed",
                    CorrelationId));
        }
    }
}
