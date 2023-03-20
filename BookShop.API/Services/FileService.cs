using BookShop.API.Repository;
using BookShop.Tools;

namespace BookShop.API.Service;

public class FileService : IFileRepository
{
    public byte[] GetAllFileFromDirectory(string pathToDirectory, string outputZipArchiveName)
    {
        Dictionary<string, byte[]> BookCoverCollection = new Dictionary<string, byte[]>();
        string[] filesCollection = Directory.GetFiles(pathToDirectory, "*.jpg");

        foreach (var file in filesCollection)
        {
            var fileName = file.Split('/');
            BookCoverCollection.Add(fileName[1],System.IO.File.ReadAllBytes(file));
        }
        return Toolchain.CreateZipArchiveFromFilesList(BookCoverCollection, outputZipArchiveName);
    }

    public byte[] GetFileByName(string path, string fileName)
    {
        return System.IO.File.ReadAllBytes($"{path}/{fileName}");
    }

    public bool SaveFileToDirectory(string path, IFormCollection files)
    {
        var file = files.Files[0];

        if (file == null || file.Length == 0)
            return false;
        
        var fileName = Path.GetFileName(files.Files[0].FileName);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), path, $"{fileName}.jpg");
        
        //replace file if it already exist in filepath directory
        //TODO: extract it in another method 
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return true;
    }
    
    public void DeleteFileByName(string path, string name)
    {
        System.IO.File.Delete($"{path}/{name}.jpg");
    }
}