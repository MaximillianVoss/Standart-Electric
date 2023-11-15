using System.IO;
using System.Text.Json;
using Xunit;
using База_артикулов.Классы;

namespace База_артикулов.Настройки.Тесты
{
    public class TestsSettings
    {
        private const string TEST_FILE_PATH = "test_settings.json";
        private const string TEST_CONNECTION_STRING = "Test Connection String";
        private const string TEST_USER_NAME = "TestUser";
        private const string TEST_PASSWORD = "TestPassword";
        private const string TEST_SERVER = "TestServer";
        private const string TEST_BASE_PATH = "TestBasePath";

        private void WriteSettingsToJsonFile(string filePath, Settings settings)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(filePath, jsonString);
        }

        [Fact]
        public void Constructor_FileNotExists_FileCreatedWithDefaultSettings()
        {
            // Подготовка
            if (File.Exists(TEST_FILE_PATH))
            {
                File.Delete(TEST_FILE_PATH);
            }

            // Действие
            var settings = new Settings(TEST_FILE_PATH);

            // Проверка
            Assert.True(File.Exists(TEST_FILE_PATH));
            string jsonString = File.ReadAllText(TEST_FILE_PATH);
            var loadedSettings = JsonSerializer.Deserialize<Settings>(jsonString);
            Assert.Equal("Подключение к LAPTOP-BBFM8MMD", loadedSettings.CurrentConnectionStringName);
        }

        [Fact]
        public void Constructor_FileExists_SettingsLoadedFromFile()
        {
            // Подготовка
            var initialSettings = new Settings
            {
                CurrentConnectionStringName = TEST_CONNECTION_STRING,
                UserNameWDClient = TEST_USER_NAME,
                PasswordWDClient = TEST_PASSWORD,
                ServerWDClient = TEST_SERVER,
                BasePathWDClient = TEST_BASE_PATH
            };
            this.WriteSettingsToJsonFile(TEST_FILE_PATH, initialSettings);

            // Действие
            var settings = new Settings(TEST_FILE_PATH);

            // Проверка
            Assert.Equal(TEST_CONNECTION_STRING, settings.CurrentConnectionStringName);
            Assert.Equal(TEST_USER_NAME, settings.UserNameWDClient);
            Assert.Equal(TEST_PASSWORD, settings.PasswordWDClient);
            Assert.Equal(TEST_SERVER, settings.ServerWDClient);
            Assert.Equal(TEST_BASE_PATH, settings.BasePathWDClient);
        }

        [Fact]
        public void SaveToFile_ValidPath_AllSettingsSavedToFile()
        {
            // Подготовка
            var settings = new Settings
            {
                CurrentConnectionStringName = TEST_CONNECTION_STRING,
                UserNameWDClient = TEST_USER_NAME,
                PasswordWDClient = TEST_PASSWORD,
                ServerWDClient = TEST_SERVER,
                BasePathWDClient = TEST_BASE_PATH
            };

            // Действие
            settings.SaveToFile(TEST_FILE_PATH);

            // Проверка
            string jsonString = File.ReadAllText(TEST_FILE_PATH);
            var savedSettings = JsonSerializer.Deserialize<Settings>(jsonString);
            Assert.Equal(TEST_CONNECTION_STRING, savedSettings.CurrentConnectionStringName);
            Assert.Equal(TEST_USER_NAME, savedSettings.UserNameWDClient);
            Assert.Equal(TEST_PASSWORD, savedSettings.PasswordWDClient);
            Assert.Equal(TEST_SERVER, savedSettings.ServerWDClient);
            Assert.Equal(TEST_BASE_PATH, savedSettings.BasePathWDClient);
        }

        [Fact]
        public void LoadFromFile_ValidPath_AllSettingsLoadedFromFile()
        {
            // Подготовка
            var initialSettings = new Settings
            {
                CurrentConnectionStringName = TEST_CONNECTION_STRING,
                UserNameWDClient = TEST_USER_NAME,
                PasswordWDClient = TEST_PASSWORD,
                ServerWDClient = TEST_SERVER,
                BasePathWDClient = TEST_BASE_PATH
            };
            this.WriteSettingsToJsonFile(TEST_FILE_PATH, initialSettings);
            var settings = new Settings();

            // Действие
            settings.LoadFromFile(TEST_FILE_PATH);

            // Проверка
            Assert.Equal(TEST_CONNECTION_STRING, settings.CurrentConnectionStringName);
            Assert.Equal(TEST_USER_NAME, settings.UserNameWDClient);
            Assert.Equal(TEST_PASSWORD, settings.PasswordWDClient);
            Assert.Equal(TEST_SERVER, settings.ServerWDClient);
            Assert.Equal(TEST_BASE_PATH, settings.BasePathWDClient);
        }

        public TestsSettings()
        {
            // This is the constructor of the test class.
            if (File.Exists(TEST_FILE_PATH))
            {
                File.Delete(TEST_FILE_PATH);
            }
        }
    }
}
