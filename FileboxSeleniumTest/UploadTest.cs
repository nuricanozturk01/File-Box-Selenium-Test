using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FileboxSeleniumTest
{
    public class UploadTest : IClassFixture<WebDriver>
    {
        private readonly IWebDriver m_driver;
        private readonly string HOME_PAGE = "http://localhost:3000/";
        public UploadTest(WebDriver driver)
        {
            m_driver = driver.m_webDriver;
            m_driver.Navigate().GoToUrl(HOME_PAGE);
        }


        [Fact]
        public void UploadFile_WithGivenFolderId_ShouldReturnsEqualAndTrue()
        {
            var random = new Random();

            Util.Login(m_driver);

            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("upload-file")).Enabled);
            }

            m_driver.FindElement(By.Id("upload-file")).Click();
//            m_driver.FindElement(By.Id("upload-file-options-navbar")).Click();
            m_driver.FindElement(By.Id("upload-file-options-navbar")).SendKeys("C:\\Users\\hp\\Downloads\\NodeJs.pdf");
            m_driver.FindElement(By.Id("upload-file-button")).Click();
          

            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(80));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }

            m_driver.Navigate().GoToUrl(HOME_PAGE + "home");
            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }
            var filesOnRootPath = Util.GetFiles(m_driver);

            {
                WebDriverWait wait = new WebDriverWait(m_driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("table")).Enabled);
            }
            Assert.Contains(filesOnRootPath, f => f.name == "NodeJs.pdf");
        }
    }
}
