using System;
using System.IO;
using System.Text.Json;

namespace База_артикулов.Классы
{
    public class SettingsNew
    {
        private const string DEFAULT_FILE_PATH = "settings.json";

        #region Поля
        private string _currentConnectionString = "Подключение к LAPTOP-BBFM8MMD";
        private string _userNameWDClient;
        private string _passwordWDClient;
        private string _serverWDClient;
        private string _basePathWDClient;
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
                    OnCurrentConnectionStringChanged(_currentConnectionString);
                }
            }
        }

        public string UserNameWDClient { get => _userNameWDClient; set => _userNameWDClient = value; }
        public string PasswordWDClient { get => _passwordWDClient; set => _passwordWDClient = value; }
        public string ServerWDClient { get => _serverWDClient; set => _serverWDClient = value; }
        public string BasePathWDClient { get => _basePathWDClient; set => _basePathWDClient = value; }

        public event Action<string> CurrentConnectionStringChanged;
        #endregion

        #region Методы

        public void SaveToFile(string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(this, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

        public void LoadFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    var settings = JsonSerializer.Deserialize<SettingsNew>(jsonString);
                    if (settings != null)
                    {
                        _currentConnectionString = settings.CurrentConnectionString;
                        _userNameWDClient = settings.UserNameWDClient;
                        _passwordWDClient = settings.PasswordWDClient;
                        _serverWDClient = settings.ServerWDClient;
                        _basePathWDClient = settings.BasePathWDClient;

                        OnCurrentConnectionStringChanged(_currentConnectionString);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
        }

        #endregion

        #region Конструкторы/Деструкторы
        public SettingsNew() //: this(DEFAULT_FILE_PATH)
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
