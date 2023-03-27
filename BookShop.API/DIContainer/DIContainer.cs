using BookShop.API.Repository;
using BookShop.API.Service;

namespace BookShop.API.DIContainer;

public static class DIContainer
{
    public static IServiceCollection AddSellerService(this IServiceCollection services)
    {
        return services.AddSingleton<ISellerRepository, SellerService>();
    }
    public static IServiceCollection AddBookService(this IServiceCollection services)
    {
        return services.AddSingleton<IBookRepository, BookService>();
    }

    public static IServiceCollection AddUserService(this IServiceCollection services)
    {
        return services.AddSingleton<IUserRepository, UserService>();
    }

    public static IServiceCollection AddFileService(this IServiceCollection services)
    {
        return services.AddSingleton<IFileRepository, FileService>();
    }
    
    public static IServiceCollection AddCommentService(this IServiceCollection services)
    {
        return services.AddSingleton<ICommentRepository, CommentService>();
    }
}