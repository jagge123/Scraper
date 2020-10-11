using System.Threading.Tasks;

namespace ScraperApp.Handlers
{
    public interface IFileHandler
    {
        string CreateFolderPath(string path, string rootFolder);
        string CreateFileName(string path);
        void CreateAndWrite(string document, string folderPath, string fileName);
        Task CreateAndWriteAsync(string document, string folderPath, string fileName);
    }
}
