using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using Xunit;
using System.Diagnostics;
using System.Threading;

namespace cross_platform_test_uiautomation
{
    public class Day3AppUITests : IDisposable
    {
        private WindowsDriver<WindowsElement> _driver;
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        public Day3AppUITests()
        {
            // Start WinAppDriver
            Process.Start(@"C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe");
            Thread.Sleep(3000);

            var options = new AppiumOptions();

            options.AddAdditionalCapability("app",@"C:\WIPs\ShareLink-windows-3.0.0.1765\ShareLinkMauiApp.exe");

            _driver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), options);
            Thread.Sleep(10000);
        }

        [Fact]
        public void ClickLeaveButtonTest()
        {
            var deviceItem = _driver.FindElement(By.Name("ShareLink-Pro-16-45-2C"));
            Assert.True(deviceItem.Displayed);
            deviceItem.Click();
            Thread.Sleep(5000);
            var leaveBtn = _driver.FindElement(By.Name("Leave"));
            Thread.Sleep(5000);
        }

        public void Dispose()
        {
            _driver?.Quit();
        }
    }
}