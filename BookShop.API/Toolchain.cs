using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace BookShop.Tools;

public static class Toolchain
{
    public static string GenerateHash(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            return hash;
        }
    }

    public static byte[] CreateZipArchiveFromFilesList(Dictionary<string, byte[]> fileDataDictionary, string archiveName)
    {
        using (MemoryStream archiveStream = new MemoryStream())
        {
            using (ZipArchive archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
            {
                foreach (var fileData in fileDataDictionary)
                {
                    string fileName = fileData.Key; // you can change the file extension or name as needed
                    ZipArchiveEntry entry = archive.CreateEntry(fileName);
                    using (Stream entryStream = entry.Open())
                    {
                        entryStream.Write(fileData.Value, 0, fileData.Value.Length);
                    }
                }
            }

            return archiveStream.ToArray();
        }
    }
}