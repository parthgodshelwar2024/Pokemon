using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PokemonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected string Organization =>
            HttpContext.Items["Organization"]?.ToString()!;

        protected string CorrelationId =>
            HttpContext.Items["CorrelationId"]?.ToString()!;
    }
}
