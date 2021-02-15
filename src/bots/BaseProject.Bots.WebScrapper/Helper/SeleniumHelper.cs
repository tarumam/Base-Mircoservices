using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BaseProject.SearchProducts.Helper
{
    public class SeleniumHelper
    {
        private readonly IWebDriver CD;

        public static SeleniumHelper _instance;
        public WebDriverWait Wait;

        protected SeleniumHelper()
        {
            CD = new ChromeDriver();
            Wait = new WebDriverWait(CD, TimeSpan.FromSeconds(30));
        }

        public static SeleniumHelper Instance()
        {
            return _instance ?? (_instance = new SeleniumHelper());
        }

        public bool ValidateContent(string content)
        {
            return Wait.Until(ExpectedConditions.UrlContains(content));
        }

        public string NavigateToUrl(string url)
        {
            CD.Navigate().GoToUrl(url);
            return CD.Url;
        }

        public void ClickOnLinkByText(string text)
        {
            var link = Wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText(text)));
            link.Click();
        }

        public void ClickOnButtonById(string id)
        {
            var button = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
            button.Click();
        }

        public void FillTextboxById(string text, string id)
        {
            var field = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
            field.SendKeys(text);
        }

        public IWebElement GetElementByClass(string className)
        {
            var element = Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(className)));
            return element;
        }

        public IWebElement GetElementById(string id)
        {
            var element = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
            return element;
        }

        public IEnumerable<IWebElement> GetListOfElementsByClass(string className)
        {
            var elements = Wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName(className)));
            return elements;
        }

        public IEnumerable<IWebElement> GetListOfElementsByTag(string className)
        {
            var elements = Wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName(className)));
            return elements;
        }
    }
}
