using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using Xunit;
using System.Diagnostics;
using System.Threading;
using Xunit.Abstractions;

namespace cross_platform_test_uiautomation
{
    public class Day3AppUiTests : IDisposable
    {
        private WindowsDriver<WindowsElement> _driver;
        private readonly ITestOutputHelper _output;
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        public Day3AppUiTests(ITestOutputHelper output)
        {
            _output = output;
            // Start WinAppDriver
            Process.Start(@"C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe");
            Thread.Sleep(3000);

            var options = new AppiumOptions();

            options.AddAdditionalCapability("app",@"C:\WIPs\ShareLink-windows-3.0.0.1765\ShareLinkMauiApp.exe");

            _driver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), options);
            Thread.Sleep(10000);
        }

        [Fact]
        public void AppLaunch_SettingsBtnShouldBeFocused()
        {
            _driver.SwitchTo().ActiveElement().SendKeys(Keys.Space);
            Thread.Sleep(3000);
            var settingsPage = _driver.FindElement(By.Name("Settings"));
            Assert.True(settingsPage.Displayed);
        }

        [Fact]
        public void AppLaunch_TapTabButtonFiveTimes_SettingsButtonShouldBeFocused()
        {
            for (int i = 0; i <= 5; i++)
            {
                _driver.SwitchTo().ActiveElement().SendKeys(Keys.Tab);
                Thread.Sleep(500);
            }
            
            _driver.SwitchTo().ActiveElement().SendKeys(Keys.Space);
            Thread.Sleep(3000);
            var settingsPage = _driver.FindElement(By.Name("Settings"));
            Assert.True(settingsPage.Displayed);
        }

        [Fact]
        public void SharePage_RunTime_MemorizedPasswordNoCodeConnection()
        {
            var deviceItem = _driver.FindElement(By.Name("ShareLink-Pro-16-45-2C"));
            deviceItem.Click();
            Thread.Sleep(3000);
            var sharePage = _driver.FindElement(By.Name("Share"));
            Assert.True(sharePage.Displayed);
        }

        [Fact]
        public void ParticipantPage_ClickParticipantsTab_ShowsConnectedList()
        {
            var deviceItem = _driver.FindElement(By.Name("ShareLink-Pro-16-45-2C"));
            deviceItem.Click();
            Thread.Sleep(3000);

            var participantIcon = _driver.FindElement(By.Name("Participants"));
            participantIcon.Click();
            Thread.Sleep(1000);
            participantIcon.Click();
            Thread.Sleep(1000);

            var connectedList = _driver.FindElementByAccessibilityId("ConnectedList");
            Assert.True(connectedList.Displayed);
        }

        [Fact]
        public void MorePage_ClickMoreTab_ShowWebViewAndDisplayPowerSettings()
        {
            var deviceItem = _driver.FindElement(By.Name("ShareLink-Pro-16-45-2C"));
            deviceItem.Click();
            Thread.Sleep(3000);

            var moreIcon = _driver.FindElement(By.Name("More"));
            moreIcon.Click();
            Thread.Sleep(1000);
            moreIcon.Click();
            Thread.Sleep(1000);
            var webView = _driver.FindElementByName("WebView");
            Assert.True(webView.Displayed);
            var disPlayPower = _driver.FindElementByName("Display Power");
            Assert.True(disPlayPower.Displayed);
        }

        public void Dispose()
        {
            _driver?.Quit();
        }
    }
}