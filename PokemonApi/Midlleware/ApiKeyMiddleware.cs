namespace PokemonApi.Midlleware
{
    public class ApiKeyMiddleware
    {
        private const string ApiKeyHeader = "X-API-KEY";
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("API Key missing");
                return;
            }

            var keys = _config.GetSection("ApiKeys").Get<Dictionary<string, string>>();

            var org = keys?.FirstOrDefault(x => x.Value == apiKey).Key;

            if (string.IsNullOrEmpty(org))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            context.Items["Organization"] = org;
            await _next(context);
        }
    }
}
