using System;

namespace База_артикулов.Классы
{
    public static class Settings
    {
        public static class Connections
        {
            // private WDClient wdClient = new WDClient("devstor", "TE6db?lZE~8Ixc?KAtQW", "https://rcloud.rsk-gr.ru", "/remote.php/dav/files/devstor");
            //private static string _currentConnectionString = "Подключение к SPB-NB004";
            //private static string _currentConnectionString = "Подключение к корпоративному SQL серверу";
            private static string _currentConnectionString = "Подключение к LAPTOP-BBFM8MMD";

            public static string CurrentConnectionString
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

            public static event Action<string> CurrentConnectionStringChanged;

            private static void OnCurrentConnectionStringChanged(string newConnectionString)
            {
                CurrentConnectionStringChanged?.Invoke(newConnectionString);
            }

        }
    }
}
