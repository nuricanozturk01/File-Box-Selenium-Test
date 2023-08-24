using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;

namespace FileboxSeleniumTest
{
    public class FilterTest : IClassFixture<WebDriver>
    {
        private readonly IWebDriver m_driver;
        private readonly By USERNAME_INPUT = By.Id("floatingInput");
        private readonly By PASSWORD_INPUT = By.Id("floatingPassword");
        private readonly By LOGIN_BUTTON = By.Id("login-button");

        private readonly string VALID_TEST_USERNAME = "nuricanozturk";
        private readonly string VALID_TEST_PASSWORD = "123";
        private readonly string HOME_PAGE = "http://localhost:3000/";
        
        public FilterTest(WebDriver driver) 
        {
            m_driver = driver.m_webDriver;
            m_driver.Navigate().GoToUrl(HOME_PAGE);
        }

        [Fact]
        public void FilterByFileExtension_WithGivenFileExtensionAndFolderId_ShouldReturnEquals()
        {
            Login();

            {
                WebDriverWait wait = new WebDriverWait(m_driver, System.TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("filter-by")).Enabled);
            }

            m_driver.FindElement(By.Id("filter-by")).Click();
            m_driver.FindElement(By.Id("filter-by-extension")).Click();
            m_driver.FindElement(By.Id("filter-by")).Click();
            m_driver.FindElement(By.LinkText("Apply Filter")).Click();

            var table = m_driver.FindElement(By.Id("table"));
            var files = table.FindElement(By.Id("table-body")).FindElements(By.Id("file-col"));


            var fileExtensions = new List<string>();
           

            
            foreach (var f in files)
            {
                var fileName = f.FindElement(By.Id("file-name-label")).Text;
                
                var extension = Path.GetExtension(fileName);
               
                fileExtensions.Add(extension);
            }


            var flag = true;

           for (int i = 0; i < fileExtensions.Count; ++i)
                if (fileExtensions[i] != ".png")
                {
                    flag = false; 
                    break;
                }

            Assert.True(flag);
        }




        private void Login()
        {
            m_driver.FindElement(USERNAME_INPUT).SendKeys(VALID_TEST_USERNAME);
            m_driver.FindElement(PASSWORD_INPUT).SendKeys(VALID_TEST_PASSWORD);
            m_driver.FindElement(LOGIN_BUTTON).Click();
        }
    }
}
