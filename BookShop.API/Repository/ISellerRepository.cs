using BookShop.API.Model.Entity;

namespace BookShop.API.Repository;

public interface ISellerRepository
{
    SellerStats GetSellerStats(string id);
    void DeleteSellerStats(string id);
    void UpdateCountOfProductOnAdd(string sellerId);
    void UpdateCountOfProductOnDelete(string sellerId);
    void UpdateCountOfSoldProduct(string sellerId, int countOfProduct);
}