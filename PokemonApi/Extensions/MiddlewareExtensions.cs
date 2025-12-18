using PokemonApi.Midlleware;

namespace PokemonApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<ApiKeyMiddleware>();
            return app;
        }
    }
}
