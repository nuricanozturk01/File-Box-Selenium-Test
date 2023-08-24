using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace FileboxSeleniumTest
{
    public class RemoveFileTest : IClassFixture<WebDriver>
    {
        private readonly IWebDriver m_driver;
        private readonly string HOME_PAGE = "http://localhost:3000/";
        public RemoveFileTest(WebDriver driver)
        {
            m_driver = driver.m_webDriver;
            m_driver.Navigate().GoToUrl(HOME_PAGE);
        }

        [Fact]
        public void RemoveFile_WithGivenFileIdAndUserId_ShouldReturnEqual()
        {
            var random = new Random();
            Util.Login(m_driver);

            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }

            // Files on the page
            var files = Util.GetFiles(m_driver);

            //Selecting removing file
            var removingFile = files[random.Next(0, files.Count - 1)];


            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }

            var table = m_driver.FindElement(By.Id("table"));
            var filesOnTable = table.FindElement(By.Id("table-body")).FindElements(By.Id("file-col"));

            // Right Clicked element
            IWebElement deneme = null;

            foreach (var file in filesOnTable)
            {
                var fileName = file.FindElement(By.Id("file-name-label")).Text;

                if (fileName == removingFile.name)
                    deneme = file.FindElement(By.Id("file-name-label"));
            }

            // Action for right click
            Actions action = new Actions(m_driver);
            action.MoveToElement(deneme).ContextClick().Build().Perform();

            m_driver.FindElement(By.Id("remove-right-click-item-button")).Click();


            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }

            var afterRemoveFiles = Util.GetFiles(m_driver);

            Assert.NotEqual(files.Count, afterRemoveFiles.Count);
            Assert.Equal(files.Count - 1, afterRemoveFiles.Count);
            Assert.True(!afterRemoveFiles.Contains(removingFile));
        }




        [Fact]
        public void RemoveFolder_WithGivenFolderIdAndUserId_ShouldReturnEqual()
        {
            var random = new Random();
            Util.Login(m_driver);

            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }

            // Files on the page
            var folders = Util.GetFolders(m_driver);

            //Selecting removing file
            var removingFolder = folders[random.Next(0, folders.Count - 1)];


            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }

            var table = m_driver.FindElement(By.Id("table"));
            var folderOnTable = table.FindElement(By.Id("table-body")).FindElements(By.Id("folder-col"));

            // Right Clicked element
            IWebElement webelement = null;

            foreach (var folder in folderOnTable)
            {
                var folderName = folder.FindElement(By.Id("folder-name-label")).Text;

                if (folderName == removingFolder.name)
                    webelement = folder.FindElement(By.Id("folder-name-label"));
            }

            // Action for right click
            Actions action = new Actions(m_driver);
            action.MoveToElement(webelement).ContextClick().Build().Perform();

            m_driver.FindElement(By.Id("remove-right-click-item-button")).Click();
            m_driver.Navigate().Refresh();

            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }

            var afterRemoveFolders = Util.GetFolders(m_driver);

            Assert.NotEqual(folders.Count, afterRemoveFolders.Count);
            Assert.Equal(folders.Count - 1, afterRemoveFolders.Count);
            Assert.True(!afterRemoveFolders.Contains(removingFolder));
        }
    }
}
