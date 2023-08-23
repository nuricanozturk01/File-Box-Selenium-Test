using OpenQA.Selenium;

namespace FileboxSeleniumTest
{
    internal static class Util
    {
        public static void WaitNSecond(IWebDriver driver, double time)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(time);
        }
    }
}
