using System.IO;
using База_артикулов.Классы;
using Xunit;

namespace База_артикулов.Настройки.Тесты
{
    public class TestsSettings
    {
        private const string TEST_FILE_PATH = "test_settings.txt";
        private const string TEST_CONNECTION_STRING = "Test Connection String";
        private const string TEST_USER_NAME = "TestUser";
        private const string TEST_PASSWORD = "TestPassword";
        private const string TEST_SERVER = "TestServer";
        private const string TEST_BASE_PATH = "TestBasePath";

        [Fact]
        public void Constructor_FileNotExists_FileCreatedWithDefaultSettings()
        {
            // Подготовка
            if (File.Exists(TEST_FILE_PATH))
            {
                File.Delete(TEST_FILE_PATH);
            }

            // Действие
            var settings = new SettingsNew(TEST_FILE_PATH);

            // Проверка
            Assert.True(File.Exists(TEST_FILE_PATH));
            var lines = File.ReadAllLines(TEST_FILE_PATH);
            Assert.Equal("Подключение к LAPTOP-BBFM8MMD", lines[0]);
        }

        [Fact]
        public void Constructor_FileExists_SettingsLoadedFromFile()
        {
            // Подготовка
            var settingsData = $"{TEST_CONNECTION_STRING}\n{TEST_USER_NAME}\n{TEST_PASSWORD}\n{TEST_SERVER}\n{TEST_BASE_PATH}";
            File.WriteAllText(TEST_FILE_PATH, settingsData);

            // Действие
            var settings = new SettingsNew(TEST_FILE_PATH);

            // Проверка
            Assert.Equal(TEST_CONNECTION_STRING, settings.CurrentConnectionString);
            Assert.Equal(TEST_USER_NAME, settings.UserNameWDClient);
            Assert.Equal(TEST_PASSWORD, settings.PasswordWDClient);
            Assert.Equal(TEST_SERVER, settings.ServerWDClient);
            Assert.Equal(TEST_BASE_PATH, settings.BasePathWDClient);
        }

        [Fact]
        public void SaveToFile_ValidPath_AllSettingsSavedToFile()
        {
            // Подготовка
            var settings = new SettingsNew
            {
                CurrentConnectionString = TEST_CONNECTION_STRING,
                UserNameWDClient = TEST_USER_NAME,
                PasswordWDClient = TEST_PASSWORD,
                ServerWDClient = TEST_SERVER,
                BasePathWDClient = TEST_BASE_PATH
            };

            // Действие
            settings.SaveToFile(TEST_FILE_PATH);

            // Проверка
            var lines = File.ReadAllLines(TEST_FILE_PATH);
            Assert.Equal(TEST_CONNECTION_STRING, lines[0]);
            Assert.Equal(TEST_USER_NAME, lines[1]);
            Assert.Equal(TEST_PASSWORD, lines[2]);
            Assert.Equal(TEST_SERVER, lines[3]);
            Assert.Equal(TEST_BASE_PATH, lines[4]);
        }

        [Fact]
        public void LoadFromFile_ValidPath_AllSettingsLoadedFromFile()
        {
            // Подготовка
            var settingsData = $"{TEST_CONNECTION_STRING}\n{TEST_USER_NAME}\n{TEST_PASSWORD}\n{TEST_SERVER}\n{TEST_BASE_PATH}";
            File.WriteAllText(TEST_FILE_PATH, settingsData);
            var settings = new SettingsNew();

            // Действие
            settings.LoadFromFile(TEST_FILE_PATH);

            // Проверка
            Assert.Equal(TEST_CONNECTION_STRING, settings.CurrentConnectionString);
            Assert.Equal(TEST_USER_NAME, settings.UserNameWDClient);
            Assert.Equal(TEST_PASSWORD, settings.PasswordWDClient);
            Assert.Equal(TEST_SERVER, settings.ServerWDClient);
            Assert.Equal(TEST_BASE_PATH, settings.BasePathWDClient);
        }

        public TestsSettings()
        {
            if (File.Exists(TEST_FILE_PATH))
            {
                File.Delete(TEST_FILE_PATH);
            }
        }
    }
}
