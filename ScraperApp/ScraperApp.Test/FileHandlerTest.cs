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
        [Theory] //C:\src\Scraper\ScraperApp\ScraperApp\tretton37
        [InlineData("C:\\what\\Scraper\\App\\Something\\whatever")]
        [InlineData("C:\\what\\whatever")]
        [InlineData("C:\\what\\Scraper\\App\\Something\\whatever.html")]
        public void CreateFileName_Should_Return_FileName_With_Html_Ending(string input)
        {
            var fileHandler = new FileHandler(null);
            var expected = "whatever.html";
            var actual = fileHandler.CreateFileName(input);

            Assert.Equal(expected, actual);
        }
    }
}
