using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;


namespace ConsoleApp4
{

    class Program
    {
        private static IWebDriverFactory _factory;
        private static RemoteWebDriver _browserDriver;
        private static TestHelper _th;
        private static readonly TimeSpan WaitTime = new TimeSpan(0, 0, 5);
        private const string BaseUrl = "https://mail.ru/";
        private const string TestLogin = "testermim@mail.ru";
        private const string TestPassword = "05042018MIM";
        private const string MailTo = "phstas2018@rambler.ru";

        public static void Main(string[] args)
        {
            SetBaseSettings();
            // 1 test open start web page
            // 2 test login account user
            // 3 test post email
            var isTrueTest1 = _th.OpeningStartPage(_browserDriver, BaseUrl);
            var isTrueTest2 = _th.LoginWData(_browserDriver, TestLogin, TestPassword);
            var isTrueTest3 = _th.PostNewEmail(_browserDriver, MailTo);
            _th.CloseDriver(_browserDriver);
            var result = isTrueTest1 && isTrueTest2 && isTrueTest3 ? "Тестирование прошло успешно" : "Тестирование прошло с ошибкой";
            Console.Clear();
            Console.WriteLine(result);
            Console.ReadLine();
        }

        private static void SetBaseSettings()
        {
            var rnd = new Random();
            var testMode = rnd.Next(0,100);
            Console.WriteLine(testMode);

            if (testMode>49)
                _factory = new ChromeFactory();
            else
                _factory = new FireFoxFactory();

            _browserDriver = _factory.CreateWebDriver();
            _browserDriver.Manage().Timeouts().ImplicitWait = WaitTime;
            _th = new TestHelper();
        }
    }

    #region TestWrapper
    internal class TestHelper
    {
        private const int ThreadTimeout = 1800;

        public bool OpeningStartPage(IWebDriver chrome, string baseUrl)
        {
            bool result;
            try
            {
                chrome.Manage().Window.Maximize();
                chrome.Navigate().GoToUrl(baseUrl);
                Console.WriteLine("Корректное открытие страницы " + baseUrl);
                result = true;
            }
            catch
            {
                Console.WriteLine("Не корректное открытие страницы " + baseUrl);
                result = false;
            }

            return result;
        }

        public bool LoginWData(IWebDriver chrome, string login, string pass)
        {
            bool result;
            try
            {
                chrome.FindElement(By.Name("login")).SendKeys(login);
                chrome.FindElement(By.CssSelector("button.svelte-1kjxz49")).Click();
                System.Threading.Thread.Sleep(ThreadTimeout);
                chrome.FindElement(By.Name("password")).SendKeys(pass);
                System.Threading.Thread.Sleep(ThreadTimeout);
                chrome.FindElement(By.CssSelector("button.second-button.svelte-1kjxz49")).Click();
                System.Threading.Thread.Sleep(ThreadTimeout);
                if (chrome.FindElement(By.Id("PH_user-email")).Text.Contains(login))
                {
                    result = true;
                    Console.WriteLine("Удачная авторизация");
                }
                else
                {
                    Console.WriteLine("Не удачная авторизация");
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine($"Не удалось заполнить поля для авторизации: {ex.Message}");
            }

            return result;
        }

        public bool PostNewEmail(IWebDriver chrome, string toAddress)
        {
            var result = true;
            try
            {
                chrome.FindElement(By.ClassName("compose-button__txt")).Click();
                chrome.FindElement(By.ClassName("container--H9L5q"))
                    .SendKeys(toAddress); // 
                chrome.FindElement(By.Name("Subject"))
                    .SendKeys("Тестовый заголовок письма");
                chrome.FindElement(By.XPath("/html/body/div[15]/div[2]/div/div[1]/div[2]/div[3]/div[5]/div/div/div[2]/div[1]"))
                    .SendKeys("Тестовое письмо тело");

                var element = chrome.FindElement(By.XPath("/html/body/div[15]/div[2]/div/div[2]/div[1]/span[1]"));
                var actions = new Actions(chrome);
                actions.MoveToElement(element).Click().Build().Perform();
            }
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine("Не удалось заполнить письмо: " + ex);
            }

            return result;
        }

        public void CloseDriver(IWebDriver chrome)
        {
            chrome.Quit();
        }
    }
    #endregion
}


