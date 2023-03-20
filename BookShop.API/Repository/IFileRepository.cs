namespace BookShop.API.Repository;

public interface IFileRepository
{
    byte[] GetAllFileFromDirectory(string pathToDirectory, string outputZipArchiveName);
    byte[] GetFileByName(string path, string fileName);
    bool SaveFileToDirectory(string path, IFormCollection file);
    void DeleteFileByName(string path, string name);
}