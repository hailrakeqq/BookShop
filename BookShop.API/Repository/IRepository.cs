namespace BookShop.API.Repository;

public interface IRepository<T> : IDisposable where T : class
{
    IEnumerable<T> GetList();
    T GetItem(string id);
    bool CheckIfExist(T item);
    void Create(T item);
    void Update(T item);
    void Delete(string id);
    void Save();
}