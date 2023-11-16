using База_артикулов.Классы;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity;
using System;

namespace База_артикулов.Модели
{
    public partial class DBSEEntities : DbContext
    {
        public DBSEEntities(string connectionName) : base($"name={connectionName}")
        {
            // Дополнительная логика, если необходимо
        }

        public DBSEEntities(ConnectionStringInfo connectionStringInfo) : base(BuildEntityFrameworkConnectionString(connectionStringInfo))
        {
            // Дополнительная логика, если необходимо
        }

        private static string BuildEntityFrameworkConnectionString(ConnectionStringInfo connectionStringInfo)
        {
            if (connectionStringInfo == null)
                throw new ArgumentNullException(nameof(connectionStringInfo), "ConnectionStringInfo не может быть null.");

            if (string.IsNullOrWhiteSpace(connectionStringInfo.Name))
                throw new ArgumentException("Имя строки подключения не может быть пустым.", nameof(connectionStringInfo.Name));

            if (string.IsNullOrWhiteSpace(connectionStringInfo.Value))
                throw new ArgumentException("Значение строки подключения не может быть пустым.", nameof(connectionStringInfo.Value));

            // Создание Entity Framework строки подключения
            var entityBuilder = new EntityConnectionStringBuilder
            {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = connectionStringInfo.Value,
                Metadata = "res://*"
            };

            return entityBuilder.ToString();
        }
    }
}
