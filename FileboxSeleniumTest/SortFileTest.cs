using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FileboxSeleniumTest
{
    public class SortFileTest : IClassFixture<WebDriver>
    {
        private readonly IWebDriver m_driver;

        private readonly string HOME_PAGE = "http://localhost:3000/";

        public SortFileTest(WebDriver driver)
        {
            m_driver = driver.m_webDriver;
            m_driver.Navigate().GoToUrl(HOME_PAGE);
        }
    

        [Fact]
        public void SortFileByFileSize_WithGivenFolderId_ShouldReturnTrue()
        {

            Util.Login(m_driver);

            {
                WebDriverWait wait = new WebDriverWait(m_driver, System.TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("sort-by")).Enabled);
            }

            m_driver.FindElement(By.Id("sort-by")).Click();
            m_driver.FindElement(By.Id("sort-file_size")).Click();


            var table = m_driver.FindElement(By.Id("table"));
            var files = table.FindElement(By.Id("table-body")).FindElements(By.Id("file-col"));


            var mbList = new List<double>();
            var gbList = new List<double>();
            var byteList = new List<double>();
            var kbList = new List<double>();

            string pattern = @"(\d+\.\d+)\s*(MB|GB|Byte|KB)";
            foreach (var f in files)
            {
                var size = f.FindElement(By.Id("file-size-ref")).Text;
                Match match = Regex.Match(size, pattern);
                string extractedNumber = match.Groups[1].Value;
                string unit = match.Groups[2].Value;

                if (unit == "MB")
                    mbList.Add(double.Parse(extractedNumber));

                if (unit == "GB")
                    gbList.Add(double.Parse(extractedNumber));

                if (unit == "KB")
                    kbList.Add(double.Parse(extractedNumber));

                if (unit == "Byte")
                    byteList.Add(double.Parse(extractedNumber));
            }


            var flag = true;

            if (mbList is not null && mbList.Count != 0)
                flag = CompareNumbers(mbList);

            if (gbList is not null && gbList.Count != 0)
                flag = CompareNumbers(gbList);

            if (kbList is not null && kbList.Count != 0)
                flag = CompareNumbers(kbList);

            if (byteList is not null && byteList.Count != 0)
                flag = CompareNumbers(byteList);

            Assert.True(flag);
        }





        [Fact]
        public void SortFileByCreationDate_WithGivenFolderId_ShouldReturnTrue()
        {
            Util.Login(m_driver);

            {
                WebDriverWait wait = new WebDriverWait(m_driver, System.TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.Id("sort-by")).Enabled);
            }

            m_driver.FindElement(By.Id("sort-by")).Click();
            m_driver.FindElement(By.Id("sort-date")).Click();


            var table = m_driver.FindElement(By.Id("table"));
            var files = table.FindElement(By.Id("table-body")).FindElements(By.Id("file-col"));


            var dateList = new List<DateTime>();
            var format = "dd/MM/yyyy HH:mm:ss";
            foreach (var f in files)
            {
                var dateString = f.FindElement(By.Id("file-creation_date-ref")).Text;

                var dateTimeObject = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None);

                dateList.Add(dateTimeObject);
            }


            var flag = true;

            for(int i = 0; i < dateList.Count - 1; ++i)
                if (dateList[i] > dateList[i + 1])
                {
                    flag = false;
                    break;
                }

            Assert.True(flag);
        }





        private bool CompareNumbers(List<double> list)
        {
            for (var i = 0; i < list.Count - 1; ++i)
                if (list[i] < list[i + 1])
                    return false;

            return true;
        }
    }
}
