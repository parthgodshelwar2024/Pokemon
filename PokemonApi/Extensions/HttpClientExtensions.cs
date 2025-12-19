namespace PokemonApi.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddHttpClients(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpClient("PokeApi", client =>
            {
                client.BaseAddress = new Uri(configuration["PokeApi:BaseUrl"]!);
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            return services;
        }
    }
}
