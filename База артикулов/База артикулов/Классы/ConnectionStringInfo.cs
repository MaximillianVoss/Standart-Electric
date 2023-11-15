using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace База_артикулов.Классы
{
    /// <summary>
    /// Представляет информацию о строке подключения к базе данных.
    /// </summary>
    public class ConnectionStringInfo
    {
        #region Поля
        private string _value;
        #endregion

        #region Свойства

        /// <summary>
        /// Получает или задает название строки подключения.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает значение строки подключения.
        /// </summary>
        public string Value
        {
            get => _value;
            set
            {
                if (!IsValidConnectionString(value))
                {
                    throw new ArgumentException("Недопустимая строка подключения Entity Framework");
                }
                _value = value;
            }
        }

        #endregion

        #region Конструкторы/Деструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса ConnectionStringInfo с указанными названием и значением строки подключения.
        /// </summary>
        /// <param name="name">Название строки подключения.</param>
        /// <param name="value">Значение строки подключения.</param>
        public ConnectionStringInfo(string name, string value)
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Проверяет, соответствует ли заданная строка подключения формату строки подключения Entity Framework.
        /// </summary>
        /// <param name="connectionString">Строка подключения для проверки.</param>
        /// <returns>Возвращает <c>true</c>, если строка подключения соответствует ожидаемому формату; в противном случае возвращает <c>false</c>.</returns>
        /// <remarks>
        /// Данный метод проверяет строку подключения на соответствие специфическому формату, используемому в Entity Framework,
        /// включая наличие метаданных, указания провайдера и строки подключения провайдера.
        /// </remarks>
        public bool IsValidConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                return false;

            var regexPattern = @"^metadata=res:\/\/\*\/[a-zA-Z0-9_.]+\.csdl\|res:\/\/\*\/[a-zA-Z0-9_.]+\.ssdl\|res:\/\/\*\/[a-zA-Z0-9_.]+\.msl;provider=System\.Data\.SqlClient;provider connection string=""([^""]+)""";
            return Regex.IsMatch(connectionString, regexPattern);
        }

        #endregion

        #region Операторы
        #endregion

        #region Обработчики событий
        #endregion
    }
}
