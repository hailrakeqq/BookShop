namespace BookShop.API.Repository;

public static class RepositoryServiceExtensions
{
    public static IServiceCollection AddBookService(this IServiceCollection services)
    {
        return services.AddSingleton<IMongoBookRepository, BookService>();
    }

    public static IServiceCollection AddUserService(this IServiceCollection services)
    {
        return services.AddSingleton<IMongoUserRepository, UserService>();
    }
}