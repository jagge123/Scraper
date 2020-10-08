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
            //Remove potential .html endings
            var withOutHtml = Regex.Replace(path, ".html", "");
            //Remove starting "/"
            int index = withOutHtml.IndexOf("/");
            var withoutStartingSlash = (index < 0)
                ? withOutHtml
                : withOutHtml.Remove(index, 1);
 
            var cleanPath = withoutStartingSlash.Replace("/", "\\");

            var folderPath = Path.Combine(RootFolder, cleanPath);
            return folderPath;
        }

        public void CreateAndWrite(string document, string folderPath, string newName)
        {
            var existingName = Path.Combine(folderPath, newName);
            if (!File.Exists(existingName))
            {
                File.WriteAllText(existingName, document);
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
