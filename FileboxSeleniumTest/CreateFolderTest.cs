using OpenQA.Selenium;

namespace FileboxSeleniumTest
{
    public class CreateFolderTest : IClassFixture<WebDriver>
    {
        private readonly IWebDriver m_driver;
        private readonly string HOME_PAGE = "http://localhost:3000/";

        private readonly string CREATED_TEST_NAME = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        

        private readonly By CREATE_FOLDER_MENU_ITEM = By.Id("create-folder");
        private readonly By CREATE_FOLDER_INPUT = By.Id("create-folder-input");
        private readonly By CREATE_FOLDER_BUTTON = By.Id("create-folder-button");



        private readonly By USERNAME_INPUT = By.Id("floatingInput");
        private readonly By PASSWORD_INPUT = By.Id("floatingPassword");
        private readonly By LOGIN_BUTTON = By.Id("login-button");

        private readonly string VALID_TEST_USERNAME = "nuricanozturk";
        private readonly string VALID_TEST_PASSWORD = "123";
        public CreateFolderTest(WebDriver driver)
        {
            m_driver = driver.m_webDriver;
            m_driver.Navigate().GoToUrl(HOME_PAGE);
        }

        [Fact]
        public void A_CreateFolder_WithGivenFolderName_ShouldReturnEquals()
        {
            var expectedText = "account: [nuricanozturk]";

            m_driver.FindElement(USERNAME_INPUT).SendKeys(VALID_TEST_USERNAME);
            m_driver.FindElement(PASSWORD_INPUT).SendKeys(VALID_TEST_PASSWORD);
            m_driver.FindElement(LOGIN_BUTTON).Click();

            Util.WaitNSecond(m_driver, 5);
            var text = m_driver.FindElement(By.Id("account-info")).Text;

            Assert.NotEmpty(text);
            Assert.Equal(expectedText, text);



            m_driver.FindElement(CREATE_FOLDER_MENU_ITEM).Click();
            m_driver.FindElement(CREATE_FOLDER_INPUT).SendKeys(CREATED_TEST_NAME);
            m_driver.FindElement(CREATE_FOLDER_BUTTON).Click();            
            m_driver.FindElement(CREATE_FOLDER_INPUT).SendKeys(Keys.Escape);
            
            var table = m_driver.FindElement(By.Id("table"));            
            var createdFolder = table.FindElement(By.Id("table-body")).FindElements(By.Id("folder-col"));

            var flag = false;
            foreach (var f in createdFolder)
            {
                var name = f.FindElement(By.Id("folder-name-label")).Text;

                if (name == CREATED_TEST_NAME)
                    flag = true;
            }
            Assert.True(flag);
        }





        [Fact]
        public void B_CreateFolder_WithGivenInvalidFolderName_ShouldReturnEquals()
        {

            m_driver.FindElement(USERNAME_INPUT).SendKeys(VALID_TEST_USERNAME);
            m_driver.FindElement(PASSWORD_INPUT).SendKeys(VALID_TEST_PASSWORD);
            m_driver.FindElement(LOGIN_BUTTON).Click();
            m_driver.FindElement(CREATE_FOLDER_MENU_ITEM).Click();
            m_driver.FindElement(CREATE_FOLDER_INPUT).SendKeys("asdas/?*<>");            

            var expectedText = "You cannot enter the /\\*?<>:\"| characters!";
            var actualText = m_driver.FindElement(By.Id("invalid-created-folder-name")).Text;

            Assert.Equal(expectedText, actualText);

        }
    }
}