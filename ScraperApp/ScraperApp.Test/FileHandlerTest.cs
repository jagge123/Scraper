using ScraperApp.Files;
using System;
using Xunit;

namespace ScraperApp.Test
{
    public class FileHandlerTest
    {
        [Theory]
        [InlineData("tretton37", "/blog/something.html", "C:\\src\\Scraper\\ScraperApp\\ScraperApp.Test\\tretton37\\blog\\something")]
        [InlineData("tretton37", "/blog/whatever", "C:\\src\\Scraper\\ScraperApp\\ScraperApp.Test\\tretton37\\blog\\whatever")]
        [InlineData("tretton37", "/blog", "C:\\src\\Scraper\\ScraperApp\\ScraperApp.Test\\tretton37\\blog")]
        public void CreatFolderPath_Should_Return_Expected_Path(string rootFolderName,
            string folderPath, string expected)
        {
            var rootFolder = FileHandler.CreateRootFolder(rootFolderName);

            var fileHandler = new FileHandler(rootFolder);

            var actual = fileHandler.CreateFolderPath(folderPath);

            Assert.Equal(expected, actual);
        }
    }
}
