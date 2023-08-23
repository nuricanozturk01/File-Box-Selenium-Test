﻿using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace FileboxSeleniumTest
{
    public class WebDriver : IDisposable
    {
        public readonly IWebDriver m_webDriver;

        public WebDriver()
        {
            m_webDriver = new EdgeDriver();
        }

        public void Dispose()
        {
            m_webDriver.Quit();
        }
    }
}