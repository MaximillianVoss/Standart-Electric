using System;
using Xunit;
using База_артикулов.Классы;

namespace База_артикулов.Строка_Подключения.Тесты
{

    public class ConnectionStringInfoTests
    {
        private const string ValidConnectionString = "metadata=res://*/Model.csdl|res://*/Model.ssdl|res://*/Model.msl;provider=System.Data.SqlClient;provider connection string=\"data source=TestServer;initial catalog=TestDB;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework\"";
        private const string InvalidConnectionString = "invalid-connection-string";
        private const string ConnectionName = "TestConnection";

        [Fact]
        public void Constructor_ValidParameters_SetsProperties()
        {
            var connectionStringInfo = new ConnectionStringInfo(ConnectionName, ValidConnectionString);

            Assert.Equal(ConnectionName, connectionStringInfo.Name);
            Assert.Equal(ValidConnectionString, connectionStringInfo.Value);
        }

        [Fact]
        public void Constructor_InvalidConnectionString_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ConnectionStringInfo(ConnectionName, InvalidConnectionString));
            Assert.Equal("Недопустимая строка подключения Entity Framework", exception.Message);
        }

        [Fact]
        public void IsValidConnectionString_ValidConnectionString_ReturnsTrue()
        {
            var connectionStringInfo = new ConnectionStringInfo(ConnectionName, ValidConnectionString);
            var result = connectionStringInfo.IsValidConnectionString(ValidConnectionString);

            Assert.True(result);
        }

        [Fact]
        public void IsValidConnectionString_InvalidConnectionString_ReturnsFalse()
        {
            var connectionStringInfo = new ConnectionStringInfo(ConnectionName, ValidConnectionString);
            var result = connectionStringInfo.IsValidConnectionString(InvalidConnectionString);

            Assert.False(result);
        }

        [Fact]
        public void SetValue_ValidConnectionString_PropertySet()
        {
            var connectionStringInfo = new ConnectionStringInfo(ConnectionName, ValidConnectionString);
            connectionStringInfo.Value = ValidConnectionString;

            Assert.Equal(ValidConnectionString, connectionStringInfo.Value);
        }

        [Fact]
        public void SetValue_InvalidConnectionString_ThrowsArgumentException()
        {
            var connectionStringInfo = new ConnectionStringInfo(ConnectionName, ValidConnectionString);

            var exception = Assert.Throws<ArgumentException>(() => connectionStringInfo.Value = InvalidConnectionString);
            Assert.Equal("Недопустимая строка подключения Entity Framework", exception.Message);
        }

        [Fact]
        public void SetName_PropertySet()
        {
            var connectionStringInfo = new ConnectionStringInfo(ConnectionName, ValidConnectionString);
            connectionStringInfo.Name = "NewName";

            Assert.Equal("NewName", connectionStringInfo.Name);
        }

        // Дополнительные тесты, если требуются, например, для проверки граничных условий, null-значений и т.д.
    }

}
