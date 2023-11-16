using System;
using System.IO;
using System.Text.Json;
using Xunit;
using База_артикулов.Классы;

namespace База_артикулов.Настройки.Тесты
{
    public class TestsSettings
    {
        private const string TEST_FILE_PATH = "test_settings.json";
        private readonly ConnectionStringInfo TEST_CONNECTION_STRING = new ConnectionStringInfo("TestName", "metadata=res://*/Model.csdl|res://*/Model.ssdl|res://*/Model.msl;provider=System.Data.SqlClient;provider connection string=\"data source=TestServer;initial catalog=TestDB;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework\"");
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
            // Удаление тестового файла, если он существует
            if (File.Exists(TEST_FILE_PATH))
            {
                File.Delete(TEST_FILE_PATH);
            }

            // Создание новых настроек
            var settings = new Settings(TEST_FILE_PATH);

            // Проверка существования файла
            Assert.True(File.Exists(TEST_FILE_PATH));

            // Загрузка и проверка созданных настроек
            string jsonString = File.ReadAllText(TEST_FILE_PATH);
            Settings loadedSettings = JsonSerializer.Deserialize<Settings>(jsonString);

            Assert.Null(loadedSettings.CurrentConnectionString);
        }

        [Fact]
        public void Constructor_FileExists_SettingsLoadedFromFile()
        {
            // Создание начальных настроек и запись в файл
            var initialSettings = new Settings
            {
                CurrentConnectionString = this.TEST_CONNECTION_STRING,
                UserNameWDClient = TEST_USER_NAME,
                PasswordWDClient = TEST_PASSWORD,
                ServerWDClient = TEST_SERVER,
                BasePathWDClient = TEST_BASE_PATH
            };
            this.WriteSettingsToJsonFile(TEST_FILE_PATH, initialSettings);

            // Создание нового объекта настроек и загрузка из файла
            var settings = new Settings(TEST_FILE_PATH);

            // Проверка загруженных значений
            Assert.Equal(this.TEST_CONNECTION_STRING.Name, settings.CurrentConnectionString.Name);
            Assert.Equal(this.TEST_CONNECTION_STRING.Value, settings.CurrentConnectionString.Value);
            Assert.Equal(TEST_USER_NAME, settings.UserNameWDClient);
            Assert.Equal(TEST_PASSWORD, settings.PasswordWDClient);
            Assert.Equal(TEST_SERVER, settings.ServerWDClient);
            Assert.Equal(TEST_BASE_PATH, settings.BasePathWDClient);
        }

        [Fact]
        public void SaveToFile_ValidPath_AllSettingsSavedToFile()
        {
            // Создание настроек
            var settings = new Settings
            {
                CurrentConnectionString = this.TEST_CONNECTION_STRING,
                UserNameWDClient = TEST_USER_NAME,
                PasswordWDClient = TEST_PASSWORD,
                ServerWDClient = TEST_SERVER,
                BasePathWDClient = TEST_BASE_PATH
            };

            // Сохранение настроек в файл
            settings.SaveToFile(TEST_FILE_PATH);

            // Загрузка и проверка сохраненных настроек
            string jsonString = File.ReadAllText(TEST_FILE_PATH);
            Settings savedSettings = JsonSerializer.Deserialize<Settings>(jsonString);

            Assert.Equal(this.TEST_CONNECTION_STRING.Name, savedSettings.CurrentConnectionString.Name);
            Assert.Equal(this.TEST_CONNECTION_STRING.Value, savedSettings.CurrentConnectionString.Value);
            Assert.Equal(TEST_USER_NAME, savedSettings.UserNameWDClient);
            Assert.Equal(TEST_PASSWORD, savedSettings.PasswordWDClient);
            Assert.Equal(TEST_SERVER, savedSettings.ServerWDClient);
            Assert.Equal(TEST_BASE_PATH, savedSettings.BasePathWDClient);
        }

        [Fact]
        public void LoadFromFile_ValidPath_AllSettingsLoadedFromFile()
        {
            // Создание и запись начальных настроек
            var initialSettings = new Settings
            {
                CurrentConnectionString = this.TEST_CONNECTION_STRING,
                UserNameWDClient = TEST_USER_NAME,
                PasswordWDClient = TEST_PASSWORD,
                ServerWDClient = TEST_SERVER,
                BasePathWDClient = TEST_BASE_PATH
            };
            this.WriteSettingsToJsonFile(TEST_FILE_PATH, initialSettings);

            // Создание нового объекта настроек и загрузка из файла
            var settings = new Settings();
            settings.LoadFromFile(TEST_FILE_PATH);

            // Проверка загруженных значений
            Assert.Equal(this.TEST_CONNECTION_STRING.Name, settings.CurrentConnectionString.Name);
            Assert.Equal(this.TEST_CONNECTION_STRING.Value, settings.CurrentConnectionString.Value);
            Assert.Equal(TEST_USER_NAME, settings.UserNameWDClient);
            Assert.Equal(TEST_PASSWORD, settings.PasswordWDClient);
            Assert.Equal(TEST_SERVER, settings.ServerWDClient);
            Assert.Equal(TEST_BASE_PATH, settings.BasePathWDClient);
        }

        [Fact]
        public void CreateDefaultSettingsFile()
        {
            // Путь к выходному каталогу проекта
            string outputDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string settingsFilePath = Path.Combine(outputDirectory, "settings.json");

            // Создаем объект ConnectionStringInfo для строки подключения к БД
            var connectionStringInfo = new ConnectionStringInfo(
                "Подключение к LAPTOP-BBFM8MMD",
                "metadata=res://*/Модели.ProductsModel.csdl|res://*/Модели.ProductsModel.ssdl|res://*/Модели.ProductsModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=LAPTOP-BBFM8MMD\\SQLEXPRESS;initial catalog=DBSE;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework\""
            );

            // Создаем настройки по умолчанию
            var settings = new Settings
            {
                CurrentConnectionString = connectionStringInfo,
                UserNameWDClient = "devstor",
                PasswordWDClient = "TE6db?lZE~8Ixc?KAtQW",
                ServerWDClient = "https://rcloud.rsk-gr.ru",
                BasePathWDClient = "/remote.php/dav/files/devstor"
            };

            // Сериализуем настройки в JSON и сохраняем в файл
            string jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(settingsFilePath, jsonString);

            // Проверяем, что файл был создан
            Assert.True(File.Exists(settingsFilePath));
        }

        public TestsSettings()
        {
            // Удаление тестового файла перед запуском тестов
            if (File.Exists(TEST_FILE_PATH))
            {
                File.Delete(TEST_FILE_PATH);
            }
        }
    }
}
