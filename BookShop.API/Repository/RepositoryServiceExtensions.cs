using BookShop.API.Service;

namespace BookShop.API.Repository;

public static class RepositoryServiceExtensions
{
    public static IServiceCollection AddBookService(this IServiceCollection services)
    {
        return services.AddSingleton<IBookRepository, BookService>();
    }

    public static IServiceCollection AddUserService(this IServiceCollection services)
    {
        return services.AddSingleton<IUserRepository, UserService>();
    }
}