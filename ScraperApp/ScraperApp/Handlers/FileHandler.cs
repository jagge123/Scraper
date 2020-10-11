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

        public async Task CreateAndWriteAsync(string document, string folderPath, string fileName)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
            {
                //Check if file is used in another process - probably not the ideal way
                var fileIsLocked = IsFileLocked(filePath);
                if (!fileIsLocked)
                {
                    using (StreamWriter writer = File.CreateText(filePath))
                    {
                        await writer.WriteAsync(document);
                    }
                }
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

        private bool IsFileLocked(string filename)
        {
            bool Locked = false;
            try
            {
                FileStream fs =
                    File.Open(filename, FileMode.OpenOrCreate,
                    FileAccess.ReadWrite, FileShare.None);
                fs.Close();
            }
            catch (IOException)
            {
                Locked = true;
            }
            return Locked;
        }
    }
}
