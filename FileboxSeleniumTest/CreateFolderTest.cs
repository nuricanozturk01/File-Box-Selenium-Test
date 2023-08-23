using OpenQA.Selenium;

namespace FileboxSeleniumTest
{
    public class CreateFolderTest : IClassFixture<WebDriver>
    {
        private readonly IWebDriver m_driver;

        public CreateFolderTest(WebDriver driver)
        {
            m_driver = driver.m_webDriver;
        }

        [Fact]
        public void CreateFolder_WithGivenFolderName_ShouldReturnEquals()
        {
           
        }
    }
}
