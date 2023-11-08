using System;
using System.IO;
using System.Text.Json;
using Xunit;
using База_артикулов.Классы;
using База_артикулов.Формы;

namespace База_артикулов.ГрафическийИнтерфейс.Тесты
{
    public class TestsCustomBase : IDisposable
    {
        private const string TestSettingsFilePath = "testSettings.json";

        public TestsCustomBase()
        {
            CreateTestSettingsFile();
        }

        private static void CreateTestSettingsFile()
        {
            var settings = new
            {
                CurrentConnectionString = "Test Connection String",
                UserNameWDClient = "TestUser",
                PasswordWDClient = "TestPassword",
                ServerWDClient = "TestServer",
                BasePathWDClient = "TestBasePath"
            };
            string jsonString = JsonSerializer.Serialize(settings);
            File.WriteAllText(TestSettingsFilePath, jsonString);
        }

        [Fact]
        public void Constructor_WhenCalled_LoadsSettingsFromFile()
        {
            // Act
            var customBase = new CustomBase(TestSettingsFilePath);

            // Assert
            Assert.NotNull(customBase.CurrentObjects);
            Assert.NotNull(customBase.CustomDb); // Assuming CustomDb is initialized within CustomBase
            // Assert.NotNull(customBase.WDClient); This needs to be checked where WDClient is actually a property

            // Assert the settings are loaded correctly
            Assert.Equal("Test Connection String", customBase.CustomDb.CurrentConnectionString);
            // WDClient properties should be checked within the WDClient object
            Assert.NotNull(customBase.WDClient.Client);
            Assert.NotNull(customBase.WDClient.UserName);
            Assert.NotNull(customBase.WDClient.Password);
            Assert.NotNull(customBase.WDClient.Server);
            Assert.NotNull(customBase.WDClient.BasePath);

            Assert.Equal("TestUser", customBase.WDClient.UserName);
            Assert.Equal("TestPassword", customBase.WDClient.Password);
            Assert.Equal("TestServer", customBase.WDClient.Server);
            Assert.Equal("TestBasePath", customBase.WDClient.BasePath);
        }

        // ... Other test methods ...

        public void Dispose()
        {
            // Clean up after each test
            if (File.Exists(TestSettingsFilePath))
            {
                File.Delete(TestSettingsFilePath);
            }
        }
    }
}
