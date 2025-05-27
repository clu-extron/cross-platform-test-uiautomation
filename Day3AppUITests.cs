using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using Xunit;
using System.Diagnostics;
using System.Threading;
using Xunit.Abstractions;
using OpenQA.Selenium.Support.UI;

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
        public void Add_NewDevice_ToDeviceList()
        {
            var addDeviceBtn = _driver.FindElementByName("ADD DEVICE");
            addDeviceBtn.Click();
            Thread.Sleep(1000);
            _driver.SwitchTo().ActiveElement().SendKeys("10.113.159.12");
            Thread.Sleep(500);
            _driver.FindElementByName("Next").Click();
            Thread.Sleep(1000);
            _driver.SwitchTo().ActiveElement().SendKeys("2");
            Thread.Sleep(500);
            _driver.FindElementByName("Next").Click();
            Thread.Sleep(1000);
            _driver.SwitchTo().ActiveElement().SendKeys("2222");
            Thread.Sleep(500);
            _driver.FindElementByName("Connect").Click();
            Thread.Sleep(2000);
            var sharePage = _driver.FindElement(By.Name("Share"));
            Assert.True(sharePage.Displayed);
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
            for (var i = 0; i <= 5; i++)
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
        public void SharePage_ShareOneImage()
        {
            var deviceItem = _driver.FindElement(By.Name("ShareLink-Pro-16-45-2C"));
            deviceItem.Click();
            Thread.Sleep(10000);

            for (var i = 0; i <= 5; i++)
            {
                _driver.SwitchTo().ActiveElement().SendKeys(Keys.Tab);
                Thread.Sleep(500);
            }
            _driver.SwitchTo().ActiveElement().SendKeys(Keys.Space);
            Thread.Sleep(3000);
            var shareImage = _driver.FindElementByName("Share Image or Video");
            shareImage.Click();
            Thread.Sleep(3000);
            _driver.SwitchTo().ActiveElement().SendKeys("extron.png");
            Thread.Sleep(2000);
            _driver.SwitchTo().ActiveElement().SendKeys(Keys.Enter);
            Thread.Sleep(3000);
            var shareBtn = _driver.FindElementByName("SHARE");
            shareBtn.Click();
            Thread.Sleep(8000);

            string screenshotDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "screenshots", "share");
            Directory.CreateDirectory(screenshotDir); // Creates the directory if it doesn't exist

            var filePath = Path.Combine(screenshotDir, "share.png");

            // Delete old screenshots if they exist
            if (File.Exists(filePath)) File.Delete(filePath);

            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);

            // Introduce outside python script for further comparizon then assert
            var psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "shared_image.py",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                _output.WriteLine(output);

                // Assertion based on output content
                Assert.True(
                    output.Contains("The Image is shared successfully"),
                    "Expected Image was not detected."
                );
            }
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

        [Fact]
        public void PreferenceSetting_SetUpAndSaveUsername()
        {

            _driver.SwitchTo().ActiveElement().SendKeys(Keys.Space);
            Thread.Sleep(3000);
            var userName = _driver.FindElement(By.Name("Username"));
            userName.Click();
            var inputBox = _driver.FindElementByAccessibilityId("AutoIdMeEntry");
            inputBox.Clear();

            // Wait until the input box is empty
            Thread.Sleep(3000);
            Assert.True(inputBox.Text == "");

            inputBox.SendKeys("CLU-LT-TEST");
            // Wait until the text is updated
            Thread.Sleep(3000);

            Assert.True(inputBox.Text == "CLU-LT-TEST");
        }

        [Fact]
        public void PreferenceSetting_SelectPreferedTheme()
        {
            _driver.SwitchTo().ActiveElement().SendKeys(Keys.Space);
            Thread.Sleep(3000);
            var preferences = _driver.FindElement(By.Name("Preferences"));
            preferences.Click();
            Thread.Sleep(3000);

            string screenshotDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "screenshots", "theme");
            Directory.CreateDirectory(screenshotDir); // Creates the directory if it doesn't exist
            
            var beforePath = Path.Combine(screenshotDir, "before.png");
            var afterPath = Path.Combine(screenshotDir, "after.png");

            // Delete old screenshots if they exist
            if (File.Exists(beforePath)) File.Delete(beforePath);
            if (File.Exists(afterPath)) File.Delete(afterPath);

            var screenshot1 = ((ITakesScreenshot)_driver).GetScreenshot();
            screenshot1.SaveAsFile(beforePath, ScreenshotImageFormat.Png);

            var darkRadioBtn = _driver.FindElementByName("Dark");
            darkRadioBtn.Click();
            Thread.Sleep(3000);

            var screenshot2 = ((ITakesScreenshot)_driver).GetScreenshot();
            screenshot2.SaveAsFile(afterPath, ScreenshotImageFormat.Png);

            // Introduce outside python script for further comparizon then assert
            var psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "compare_themes.py",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi)) 
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                _output.WriteLine(output);

                // Assertion based on output content
                Assert.True(
                    output.Contains("Theme changed from Light to Dark."),
                    "Expected theme change did not occur or was not detected."
                );
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
        }
    }
}