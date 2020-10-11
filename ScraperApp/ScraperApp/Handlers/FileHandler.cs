using ScraperApp.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ScraperApp.Handlers
{
    public class FileHandler : IFileHandler
    {
        public FileHandler() { }

        public string CreateFolderPath(string path, string rootFolder)
        {
            var withOutHtmlEnding = path.RemoveHtmlEnding();
            var withoutStartingSlash = withOutHtmlEnding.RemoveStartingSlash();
            var cleanPath = withoutStartingSlash.ForwardSlashToDoubleBack();
            var folderPath = Path.Combine(rootFolder, cleanPath);
            return folderPath;
        }
        
        public string CreateFileName(string path)
        {
            var fileName = path.Substring(path.LastIndexOf("\\") + 1);
            if (!fileName.Contains(".html"))
                return $"{fileName}.html";

            return fileName;
        }

        public void CreateAndWrite(string document, string folderPath, string fileName)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, document);
            }
        }

        public static string CreateRootFolder(string folderName)
        {
            var workingDir = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(workingDir).Parent.Parent.FullName;
            var rootPath = Path.Combine(projectDirectory, folderName);
            Directory.CreateDirectory(rootPath);
            return rootPath;
        }
    }
}
