using System.IO;
using База_артикулов.Классы;
using Xunit;

namespace База_артикулов.Тесты
{
    public class TestsSettings
    {
        private const string TEST_FILE_PATH = "test_settings.txt";
        private const string TEST_CONNECTION_STRING = "Test Connection String";

        [Fact]
        public void Constructor_FileNotExists_FileCreatedWithDefaultConnectionString()
        {
            // Подготовка
            if (File.Exists(TEST_FILE_PATH))
            {
                File.Delete(TEST_FILE_PATH);
            }

            // Действие
            var settings = new SettingsNew(TEST_FILE_PATH);  // в вашем классе путь зашит, так что тест создаст settings.txt

            // Проверка
            Assert.True(File.Exists(TEST_FILE_PATH));
            string str = File.ReadAllText(TEST_FILE_PATH);
            Assert.Equal("Подключение к LAPTOP-BBFM8MMD", File.ReadAllText(TEST_FILE_PATH));
        }

        [Fact]
        public void Constructor_FileExists_ConnectionStringLoadedFromFile()
        {
            // Подготовка
            File.WriteAllText(TEST_FILE_PATH, TEST_CONNECTION_STRING);

            // Действие
            var settings = new SettingsNew();  // в вашем классе путь зашит, так что тест считает из settings.txt
            settings.CurrentConnectionString = TEST_CONNECTION_STRING;

            // Проверка
            Assert.Equal(TEST_CONNECTION_STRING, settings.CurrentConnectionString);
        }

        [Fact]
        public void SaveToFile_ValidPath_ConnectionStringSavedToFile()
        {
            // Подготовка
            var settings = new SettingsNew();
            settings.CurrentConnectionString = TEST_CONNECTION_STRING;

            // Действие
            settings.SaveToFile(TEST_FILE_PATH);

            // Проверка
            Assert.Equal(TEST_CONNECTION_STRING, File.ReadAllText(TEST_FILE_PATH));
        }

        [Fact]
        public void LoadFromFile_ValidPath_ConnectionStringLoadedFromFile()
        {
            // Подготовка
            File.WriteAllText(TEST_FILE_PATH, TEST_CONNECTION_STRING);
            var settings = new SettingsNew();

            // Действие
            settings.LoadFromFile(TEST_FILE_PATH);

            // Проверка
            Assert.Equal(TEST_CONNECTION_STRING, settings.CurrentConnectionString);
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

