using System;
using System.IO;

namespace База_артикулов.Классы
{
    public class SettingsNew
    {
        private const string DEFAULT_FILE_PATH = "settings.txt";

        #region Поля
        private string _currentConnectionString = "Подключение к LAPTOP-BBFM8MMD";
        #endregion

        #region Свойства
        public string CurrentConnectionString
        {
            get => _currentConnectionString;
            set
            {
                if (_currentConnectionString != value)
                {
                    _currentConnectionString = value;

                    // Invoke the event
                    OnCurrentConnectionStringChanged(_currentConnectionString);
                }
            }
        }

        public event Action<string> CurrentConnectionStringChanged;
        #endregion

        #region Методы

        public void SaveToFile(string filePath)
        {
            try
            {
                File.WriteAllText(filePath, _currentConnectionString);
            }
            catch (Exception ex)
            {
                // обработайте ошибки при записи файла, если это необходимо
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

        public void LoadFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    _currentConnectionString = File.ReadAllText(filePath);

                    // Если вы хотите оповестить об изменении после загрузки из файла:
                    OnCurrentConnectionStringChanged(_currentConnectionString);
                }
            }
            catch (Exception ex)
            {
                // обработайте ошибки при чтении файла, если это необходимо
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
        }

        #endregion

        #region Конструкторы/Деструкторы
        public SettingsNew() : this(DEFAULT_FILE_PATH)
        {

        }
        public SettingsNew(string filePath)
        {
            if (!File.Exists(filePath))
            {
                SaveToFile(filePath);
            }
            else
            {
                LoadFromFile(filePath);
            }
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void OnCurrentConnectionStringChanged(string newConnectionString)
        {
            CurrentConnectionStringChanged?.Invoke(newConnectionString);
        }
        #endregion
    }
}
