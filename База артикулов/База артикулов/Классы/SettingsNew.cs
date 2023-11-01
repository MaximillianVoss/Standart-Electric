using System;
using System.IO;
using System.Text;

namespace База_артикулов.Классы
{
    public class SettingsNew
    {
        private const string DEFAULT_FILE_PATH = "settings.txt";

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
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(_currentConnectionString);
                sb.AppendLine(_userNameWDClient);
                sb.AppendLine(_passwordWDClient);
                sb.AppendLine(_serverWDClient);
                sb.AppendLine(_basePathWDClient);

                File.WriteAllText(filePath, sb.ToString());
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
                    var lines = File.ReadAllLines(filePath);
                    if (lines.Length >= 5)
                    {
                        _currentConnectionString = lines[0];
                        _userNameWDClient = lines[1];
                        _passwordWDClient = lines[2];
                        _serverWDClient = lines[3];
                        _basePathWDClient = lines[4];

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
