namespace BookShop.API.Authorization;

public static class AuthenticationServiceExtensions
{
    public static IServiceCollection AddTokenService(this IServiceCollection services)
    {
        // Wire up the token service
        return services.AddSingleton<ITokenService, TokenService>();
    }

    public static IServiceCollection AddLoginResponce(this IServiceCollection services)
    {
        return services.AddSingleton<LoginResponse>();
    }
}