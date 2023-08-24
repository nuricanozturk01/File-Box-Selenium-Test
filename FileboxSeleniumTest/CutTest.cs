using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileboxSeleniumTest
{
    public class CutTest : IClassFixture<WebDriver>
    {
        private readonly IWebDriver m_driver;
        private readonly string HOME_PAGE = "http://localhost:3000/";
        public CutTest(WebDriver driver)
        {
            m_driver = driver.m_webDriver;
            m_driver.Navigate().GoToUrl(HOME_PAGE);
        }





        [Fact]
        public void MoveFile_WithGivenFolderId_ShouldReturnsEqualAndTrue()
        {
            var random = new Random();
            Util.Login(m_driver);

            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }

            // Files on the page
            var files = Util.GetFiles(m_driver);
            var folders = Util.GetFolders(m_driver);

            //Selecting removing file
            var movedFile = files[random.Next(0, files.Count - 1)];
            var movedFolder = folders[1];

            // Action for right click
            Actions action = new Actions(m_driver);
            action.MoveToElement(movedFile.selectedWebElement).ContextClick().Build().Perform();

            m_driver.FindElement(By.Id("cut-right-click-item-button")).Click();
            movedFolder.selectedWebElement.Click();

            var filesOnFolder = Util.GetFiles(m_driver);

            action.MoveToElement(filesOnFolder[0].selectedWebElement).ContextClick().Build().Perform();

            m_driver.FindElement(By.Id("paste-right-click-item-button")).Click();

            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }

            var afterMovedFileList = Util.GetFiles(m_driver);
            m_driver.Navigate().GoToUrl(HOME_PAGE + "home");
            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }
            var homePageFiles = Util.GetFiles(m_driver);



            Assert.DoesNotContain(homePageFiles, f => f.name == movedFile.name);
            Assert.Contains(afterMovedFileList, f => f.name == movedFile.name);
        }
    }
}
