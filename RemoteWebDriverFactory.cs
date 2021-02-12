using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace ConsoleApp4
{
    public interface IWebDriverFactory
    {
        RemoteWebDriver CreateWebDriver();
    }

    public class ChromeFactory : IWebDriverFactory
    {
        public RemoteWebDriver CreateWebDriver()
        {
            return new ChromeDriver();
        }
    }
    public class FireFoxFactory : IWebDriverFactory
    {
        public RemoteWebDriver CreateWebDriver()
        {
            return new FirefoxDriver();
        }
    }
}
