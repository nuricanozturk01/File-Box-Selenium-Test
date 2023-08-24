using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace FileboxSeleniumTest
{
    public class RenameTest : IClassFixture<WebDriver>
    {
        private readonly IWebDriver m_driver;
        private readonly string HOME_PAGE = "http://localhost:3000/";
        public RenameTest(WebDriver driver) 
        {
            m_driver = driver.m_webDriver;
            m_driver.Navigate().GoToUrl(HOME_PAGE);
        }

        [Fact]
        public void RenameFile_WithGivenNewFileName_ShouldReturnsEqual()
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
            var renamingFile = files[random.Next(0, files.Count - 1)];

            // Action for right click
            Actions action = new Actions(m_driver);
            action.MoveToElement(renamingFile.selectedWebElement).ContextClick().Build().Perform();
            var newFileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(renamingFile.name);
            m_driver.FindElement(By.Id("rename-right-click-item-button")).Click();
            m_driver.FindElement(By.Id("rename-file-input")).SendKeys(newFileName);
            m_driver.FindElement(By.Id("rename-file-button")).Click();

            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }

            var afterRenamingFileList = Util.GetFiles(m_driver);
            
            Assert.Equal(files.Count, afterRenamingFileList.Count);
            Assert.DoesNotContain(afterRenamingFileList, f => f.name == renamingFile.name);
            Assert.Contains(afterRenamingFileList, f => f.name == newFileName);
        }
        [Fact]
        public void RenameFolder_WithGivenNewFolderName_ShouldReturnsEqual()
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
            var renamingFolder = folders[random.Next(0, folders.Count - 1)];

            // Action for right click
            Actions action = new Actions(m_driver);
            action.MoveToElement(renamingFolder.selectedWebElement).ContextClick().Build().Perform();

            var newFolderName = Guid.NewGuid().ToString();
            
            m_driver.FindElement(By.Id("rename-right-click-item-button")).Click();
            m_driver.FindElement(By.Id("rename-folder-input")).SendKeys(newFolderName);
            m_driver.FindElement(By.Id("rename-folder-button")).Click();

            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }

            var afterRenamingFolderList = Util.GetFolders(m_driver);

            Assert.Equal(folders.Count, afterRenamingFolderList.Count);
            Assert.DoesNotContain(afterRenamingFolderList, f => f.name == renamingFolder.name);
            Assert.Contains(afterRenamingFolderList, f => f.name == newFolderName);
        }
    }
}
