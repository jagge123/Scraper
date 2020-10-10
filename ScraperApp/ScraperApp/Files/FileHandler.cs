using ScraperApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ScraperApp.Files
{
    public class FileHandler
    {
        public string RootFolder { get; private set; }

        public FileHandler(string rootFolder)
        {
            RootFolder = rootFolder;
        }
        public string CreateFolderPath(string path)
        {
            var withOutHtmlEnding = path.RemoveHtmlEnding();
            var withoutStartingSlash = withOutHtmlEnding.RemoveStartingSlash();
            var cleanPath = withoutStartingSlash.ForwardSlashToDoubleBack();
            var folderPath = Path.Combine(RootFolder, cleanPath);
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
