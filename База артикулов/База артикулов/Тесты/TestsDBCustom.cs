using System;
using System.Configuration;
using System.Linq;
using Xunit;
using База_артикулов.Классы;
using База_артикулов.Классы.Дополнения;
using База_артикулов.Модели;

namespace База_артикулов.База_Данных.Тесты
{
    public class TestsDBCustom
    {
        #region Поля
        public Settings settings;
        public CustomDB db;
        #endregion

        #region Настройки
        // Пример метода CreateSettingsWithConnectionString с использованием ConnectionStringInfo
        public Settings CreateSettingsWithConnectionString(string connectionStringName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            var connectionStringInfo = new ConnectionStringInfo(connectionStringName, connectionString);
            return new Settings { CurrentConnectionString = connectionStringInfo };
        }

        #endregion

        #region Конструкторы
        public TestsDBCustom()
        {
            // Этот код будет выполняться перед каждым тестом в этом классе
            this.settings = this.CreateSettingsWithConnectionString("Подключение к LAPTOP-BBFM8MMD-Test");
            this.db = new CustomDB(this.settings);
        }
        #endregion

        #region Тесты БД
        [Fact]
        public void InstanceDB_ShouldReturnSameInstance()
        {
            CustomDB instance1 = CustomDB.Instance;
            CustomDB instance2 = CustomDB.Instance;

            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void UpdateDB_ShouldSetNewDBInstance()
        {
            DBSEEntities db = CustomDB.Instance.DB;

            CustomDB.Instance.Update();

            Assert.NotSame(db, CustomDB.Instance.DB);
        }

        // Обновленный тестовый случай
        [Fact]
        public void InitDB_ShouldSwitchToNewConnectionString()
        {
            string oldConnectionStringName = "Подключение к LAPTOP-BBFM8MMD";
            string newConnectionStringName = "Подключение к LAPTOP-BBFM8MMD-Test";

            // Строки подключения, как они определены в файле конфигурации
            string oldExpectedConnectionString = "metadata=res://*/Модели.ProductsModel.csdl|res://*/Модели.ProductsModel.ssdl|res://*/Модели.ProductsModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=LAPTOP-BBFM8MMD\\SQLEXPRESS;initial catalog=DBSE;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework\"";
            string newExpectedConnectionString = "metadata=res://*/Модели.ProductsModel.csdl|res://*/Модели.ProductsModel.ssdl|res://*/Модели.ProductsModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=LAPTOP-BBFM8MMD\\SQLEXPRESS;initial catalog=DBSE;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework\"";

            Settings oldSettings = this.CreateSettingsWithConnectionString(oldConnectionStringName);
            var customDb = new CustomDB(oldSettings);

            // Проверяем, что изначально используется старая строка подключения
            Assert.Equal(oldConnectionStringName, customDb.CurrentConnectionString.Name);
            Assert.Equal(oldExpectedConnectionString, customDb.CurrentConnectionString.Value);

            // Меняем на новую строку подключения
            Settings newSettings = this.CreateSettingsWithConnectionString(newConnectionStringName);
            customDb.CurrentConnectionString = newSettings.CurrentConnectionString;

            // Проверяем, что теперь используется новая строка подключения
            Assert.Equal(newConnectionStringName, customDb.CurrentConnectionString.Name);
            Assert.Equal(newExpectedConnectionString, customDb.CurrentConnectionString.Value);
        }


        #endregion

    }

    #region Объекты БД
    public class TestsDBObjects : TestsDBCustom
    {
        #region Поля
        public DBSEEntities context;
        public CustomDB service;
        #endregion

        public TestsDBObjects()
        {
            this.context = this.db.DB;
            this.service = this.db;
        }

        ~TestsDBObjects()
        {
            //this.context.SaveChanges();
        }

    }
    public class TestsProducts : TestsDBObjects
    {
        #region Товары

        #region Проверки
        [Fact]
        public void IsProductExists()
        {
            int productId = this.context.Products.FirstOrDefault().id;  // Используйте реальный id продукта из вашей базы данных для тестирования.

            bool result = this.service.IsProductExists(productId);

            Assert.True(result);  // Убедитесь, что продукт с таким id существует в вашей базе данных.
        }
        #endregion

        #region Фильтрация товаров по классу,группе,подгруппе
        [Fact]
        public void FilterByClass_ReturnsCorrectResults()
        {
            System.Collections.Generic.List<ProductsViewLite> result = this.db.GetFilteredProducts(classValue: "Стальные конструкции КМ");
            Assert.All(result, product => Assert.Equal("Стальные конструкции КМ", product.Наименование_класса));
        }

        [Fact]
        public void FilterByGroup_ReturnsCorrectResults()
        {
            System.Collections.Generic.List<ProductsViewLite> result = this.db.GetFilteredProducts(group: "Эстакады модульные");
            Assert.All(result, product => Assert.Equal("Эстакады модульные", product.Наименование_группы));
        }

        [Fact]
        public void FilterBySubGroup_ReturnsCorrectResults()
        {
            System.Collections.Generic.List<ProductsViewLite> result = this.db.GetFilteredProducts(subGroup: "Фермы СМЭ.Ф");
            Assert.All(result, product => Assert.Equal("Фермы СМЭ.Ф", product.Наименование_подгруппы));
        }

        [Fact]
        public void FilterByAll_ReturnsCorrectResults()
        {
            System.Collections.Generic.List<ProductsViewLite> result = this.db.GetFilteredProducts("Эстакады модульные", "Стальные конструкции КМ", "Фермы СМЭ.Ф");

            Assert.All(result, product =>
            {
                Assert.Equal("Эстакады модульные", product.Наименование_группы);
                Assert.Equal("Стальные конструкции КМ", product.Наименование_класса);
                Assert.Equal("Фермы СМЭ.Ф", product.Наименование_подгруппы);
            });
        }
        #endregion

        [Fact]
        public void CreateProduct_ValidInputs_ProductCreated()
        {
            // Arrange
            string title = "TestTitle";
            string titleShort = "TestShort";
            string description = "TestDescription";
            string vendorCode = "12345";
            int idNorm = this.context.Norms.FirstOrDefault()?.id ?? 0; // Предполагая, что у вас есть таблица Norms
            int idSubGroup = this.context.SubGroups.FirstOrDefault()?.id ?? 0;
            int idCover = this.context.Covers.FirstOrDefault()?.id ?? 0;
            int idMaterial = this.context.Materials.FirstOrDefault()?.id ?? 0;
            int idPerforation = this.context.Perforations.FirstOrDefault()?.id ?? 0;
            int idPackage = this.context.Packages.FirstOrDefault()?.id ?? 0;
            bool isInStock = true;

            // Act
            Products createdProduct = this.service.CreateProduct(title, titleShort, description, vendorCode, idNorm, idSubGroup, idCover, idMaterial, idPerforation, idPackage, isInStock);

            // Assert
            Assert.NotNull(createdProduct);
            Assert.Equal(title, createdProduct.Descriptors.title);
            Assert.Equal(titleShort, createdProduct.Descriptors.titleShort);
            Assert.Equal(description, createdProduct.Descriptors.description);
            // Добавьте другие проверки при необходимости

            // Cleanup
            this.context.Products.Remove(createdProduct); // Удаляем созданный продукт после теста
            this.context.SaveChanges();
        }

        [Fact]
        public void RetrieveProductsViewLiteWrappedCustom_ExecutesSuccessfully()
        {
            // Arrange
            string sqlQuery = "SELECT * FROM ProductsViewLiteWrapped";

            // Act
            System.Collections.Generic.List<ProductsViewLiteWrappedCustom> products = this.service.ExecuteSqlQuery<ProductsViewLiteWrappedCustom>(sqlQuery);

            // Assert
            Assert.NotNull(products); // Убедитесь, что список не пустой
            Assert.All(products, product =>
            {
                Assert.NotNull(product.Наименование_продукта); // Проверка, что поле Наименование продукта не пустое
                // Добавьте дополнительные проверки для других полей при необходимости
                Assert.True(product.ID_продукта > 0);
            });

            // Дополнительные проверки можно добавить в зависимости от ваших требований
        }
        #endregion
    }
    public class TestsDescriptors : TestsDBObjects
    {
        #region Дескрипторы


        [Fact]
        public void CreateAndDeleteDescriptor_ShouldAddAndRemoveDescriptorFromDB()
        {
            // Arrange
            Descriptors descriptor = this.db.CreateDescriptor("test_code", "test_title", "test_titleShort", "test_titleDisplay", "test_description");

            // Проверяем, что дескриптор был добавлен
            Assert.NotNull(this.db.DB.Descriptors.Find(descriptor.id));

            // Act
            bool result = this.db.DeleteDescriptor(descriptor.id);

            // Assert
            Assert.True(result);
            Assert.Null(this.db.DB.Descriptors.Find(descriptor.id)); // Убеждаемся, что дескриптор был удален из БД
        }

        [Fact]
        public void UpdateDescriptor_ShouldUpdateAndReturnUpdatedDescriptor()
        {
            // Arrange
            this.settings = this.CreateSettingsWithConnectionString("Подключение к LAPTOP-BBFM8MMD-Test");
            this.db = new CustomDB(this.settings);
            Descriptors initialDescriptor = this.db.CreateDescriptor("initial_code", "initial_title", "initial_titleShort", "initial_titleDisplay", "initial_description");
            int initialId = initialDescriptor.id;

            try
            {
                string newCode = "updated_code";
                string newTitle = "updated_title";
                string newTitleShort = "updated_titleShort";
                string newTitleDisplay = "updated_titleDisplay";
                string newDescription = "updated_description";

                // Act
                Descriptors updatedDescriptor = this.db.UpdateDescriptor(initialId, newCode, newTitle, newTitleShort, newTitleDisplay, newDescription);

                // Assert
                Assert.NotNull(updatedDescriptor);
                Assert.Equal(newCode, updatedDescriptor.code);
                Assert.Equal(newTitle, updatedDescriptor.title);
                Assert.Equal(newTitleShort, updatedDescriptor.titleShort);
                Assert.Equal(newTitleDisplay, updatedDescriptor.titleDisplay);
                Assert.Equal(newDescription, updatedDescriptor.description);
            }
            finally
            {
                // Cleanup
                this.db.DeleteDescriptor(initialId);
            }
        }

        [Fact]
        public void UpdateDescriptor_ShouldThrowExceptionWhenDescriptorNotFound()
        {
            // Arrange
            this.settings = this.CreateSettingsWithConnectionString("Подключение к LAPTOP-BBFM8MMD-Test");
            this.db = new CustomDB(this.settings);
            int nonExistentId = 999999; // предполагая, что такого ID нет в базе данных

            // Act & Assert
            Assert.Throws<Exception>(() => this.db.UpdateDescriptor(nonExistentId));
        }

        [Fact]
        public void UpdateDescriptor_ShouldUpdateExistingDescriptorInDB()
        {
            // Arrange
            var originalDescriptor = new Descriptors
            {
                code = "initial_code",
                title = "initial_title",
                titleShort = "initial_titleShort",
                titleDisplay = "initial_titleDisplay",
                description = "initial_description"
            };
            this.db.DB.Descriptors.Add(originalDescriptor);
            this.db.DB.SaveChanges();

            try
            {
                // Assert initial creation
                Descriptors fetchedDescriptor = this.db.DB.Descriptors.Find(originalDescriptor.id);
                Assert.Equal("initial_code", fetchedDescriptor.code);
                Assert.Equal("initial_title", fetchedDescriptor.title);
                // Добавьте другие проверки здесь, если это необходимо

                // Act
                fetchedDescriptor.code = "updated_code";
                fetchedDescriptor.title = "updated_title";
                fetchedDescriptor.titleShort = "updated_titleShort";
                fetchedDescriptor.titleDisplay = "updated_titleDisplay";
                fetchedDescriptor.description = "updated_description";
                this.db.UpdateDescriptor(fetchedDescriptor);

                // Assert
                Descriptors updatedDescriptor = this.db.DB.Descriptors.Find(fetchedDescriptor.id);
                Assert.Equal("updated_code", updatedDescriptor.code);
                Assert.Equal("updated_title", updatedDescriptor.title);
                // Добавьте другие проверки здесь, чтобы удостовериться, что все поля были обновлены
            }
            finally
            {
                // Cleanup
                this.db.DeleteDescriptor(originalDescriptor.id);
            }
        }

        #region Проверки дексрипторов
        [Fact]
        public void IsDescriptorExists()
        {
            int descriptorId = 1;

            bool result = this.service.IsDescriptorExists(descriptorId);

            Assert.True(result);
        }

        [Fact]
        public void IsDescriptorProductExists()
        {
            int productId = 1;

            bool result = this.service.IsDescriptorProductExists(productId);

            Assert.True(result);
        }

        [Fact]
        public void IsDescriptorResourcesExists()
        {
            int descriptorId = this.context.DescriptorsResources.FirstOrDefault().idDescriptor;
            bool result = this.service.IsDescriptorResourcesExists(descriptorId);
            Assert.True(result);
        }

        [Fact]
        public void GetDescriptorProduct()
        {
            int productId = 1;

            Descriptors descriptor = this.service.GetDescriptorProduct(productId);

            Assert.NotNull(descriptor);
        }

        [Fact]
        public void GetDescriptorsResources()
        {
            int descriptorId = this.context.DescriptorsResources.FirstOrDefault().idDescriptor;
            DescriptorsResources descriptorResource = this.service.GetDescriptorsResources(descriptorId);
            Assert.NotNull(descriptorResource);
        }

        #endregion

        #endregion
    }
    public class TestsSubGroups : TestsDBObjects
    {
        #region Подгруппы

        [Fact]
        public void CreateSubGroup_ValidInput_SubGroupAndDescriptorCreated()
        {
            // Arrange
            string code = "TestCode";
            string title = "TestTitle";
            string titleShort = "TestShortTitle";
            string description = "TestDescription";
            int groupId = 1; // Убедитесь, что такой ID существует в вашей тестовой БД
            int loadDiagramId = 1; // Убедитесь, что такой ID существует в вашей тестовой БД
            int applicationId = 1; // Добавлен новый параметр

            // Act
            this.db.CreateSubGroup(code, title, titleShort, description, groupId, loadDiagramId, applicationId);

            // Assert
            SubGroups createdSubGroup = this.db.DB.SubGroups.FirstOrDefault(x => x.Descriptors.code == code);
            Assert.NotNull(createdSubGroup);
            Assert.Equal(title, createdSubGroup.Descriptors.title);
            this.context.SubGroups.Remove(createdSubGroup);
        }

        [Fact]
        public void DeleteSubGroup_ValidInput_SubGroupAndDescriptorDeleted()
        {
            // Arrange
            string code = "TestCode";
            string title = "TestTitle";
            string titleShort = "TestShortTitle";
            string description = "TestDescription";
            int groupId = 1; // Убедитесь, что такой ID существует в вашей тестовой БД
            int loadDiagramId = 1; // Убедитесь, что такой ID существует в вашей тестовой БД
            int applicationId = 1; // Убедитесь, что такой ID существует в вашей тестовой БД

            // Act
            SubGroups subGroup = this.db.CreateSubGroup(code, title, titleShort, description, groupId, loadDiagramId, applicationId);
            int subGroupId = subGroup.id; // ID созданной подгруппы
            int subGroupDescriptorId = subGroup.Descriptors.id; // ID связанного с подгруппой дескриптора

            // Act
            this.db.DeleteSubGroup(subGroupId);

            // Assert
            SubGroups deletedSubGroup = this.db.DB.SubGroups.FirstOrDefault(x => x.id == subGroupId);
            Assert.Null(deletedSubGroup); // Подгруппа должна быть удалена

            Descriptors relatedDescriptor = this.db.DB.Descriptors.FirstOrDefault(x => x.id == subGroupDescriptorId);
            Assert.Null(relatedDescriptor); // Дескриптор подгруппы также должен быть удалён
        }

        [Fact]
        public void UpdateSubGroup_ValidInput_SubGroupAndDescriptorUpdated()
        {
            // Arrange
            string code = "TestCode";
            string title = "TestTitle";
            string titleShort = "TestShortTitle";
            string description = "TestDescription";
            int groupId = 1; // Убедитесь, что такой ID существует в вашей тестовой БД
            int loadDiagramId = 1; // Убедитесь, что такой ID существует в вашей тестовой БД
            int applicationId = 1; // Добавленный параметр applicationId

            SubGroups createdSubGroup = this.db.CreateSubGroup(code, title, titleShort, description, groupId, loadDiagramId, applicationId);
            int subGroupId = createdSubGroup.id;
            SubGroups originalSubGroup = this.db.DB.SubGroups.FirstOrDefault(x => x.id == subGroupId);
            Assert.NotNull(originalSubGroup);

            string updatedCode = "UpdatedCode";
            string updatedTitle = "UpdatedTitle";
            string updatedTitleShort = "UpdatedTitleShort";
            string updatedDescription = "UpdatedDescription";
            int updatedGroupId = 2; // Убедитесь, что такой ID существует в вашей тестовой БД
            int updatedLoadDiagramId = 2; // Убедитесь, что такой ID существует в вашей тестовой БД
            int updatedApplicationId = 2; // Обновленный параметр applicationId

            // Act
            this.db.UpdateSubGroup(originalSubGroup, updatedCode, updatedTitle, updatedTitleShort, updatedDescription, updatedGroupId, updatedLoadDiagramId, updatedApplicationId);

            // Assert
            SubGroups updatedSubGroup = this.db.DB.SubGroups.FirstOrDefault(x => x.id == subGroupId);
            Assert.NotNull(updatedSubGroup);
            Assert.Equal(updatedTitle, updatedSubGroup.Descriptors.title);
            this.context.SubGroups.Remove(createdSubGroup);
        }


        #endregion
    }
    public class TestsGroups : TestsDBObjects
    {
        #region Группы
        [Fact]
        public void CreateGroup_ShouldReturnNewGroup()
        {
            Groups newGroup = this.db.CreateGroup("testCode", "testTitle", "testTitleShort", "testDescription", 1);

            Assert.NotNull(newGroup);
            Assert.Equal("testCode", newGroup.Descriptors.code);
            Assert.Equal("testTitle", newGroup.Descriptors.title);

            // Очистка созданного элемента после теста.
            this.db.DeleteGroup(newGroup.id);
        }

        [Fact]
        public void UpdateGroup_ShouldUpdateExistingGroup()
        {
            // Создание тестовой группы для обновления.
            Groups testGroup = this.db.CreateGroup("initialCode", "initialTitle", "initialTitleShort", "initialDescription", 1);

            this.db.UpdateGroup(testGroup.id, "updatedCode", "updatedTitle", "updatedTitleShort", "updatedDescription", 2);

            Groups updatedGroup = this.db.DB.Groups.FirstOrDefault(x => x.id == testGroup.id);

            Assert.NotNull(updatedGroup);
            Assert.Equal("updatedCode", updatedGroup.Descriptors.code);
            Assert.Equal("updatedTitle", updatedGroup.Descriptors.title);

            // Очистка созданного элемента после теста.
            this.db.DeleteGroup(testGroup.id);
        }

        [Fact]
        public void DeleteGroup_ShouldRemoveGroupFromDatabase()
        {
            // Создание тестовой группы для удаления.
            Groups testGroup = this.db.CreateGroup("testCode", "testTitle", "testTitleShort", "testDescription", 1);

            this.db.DeleteGroup(testGroup.id);

            Groups deletedGroup = this.db.DB.Groups.FirstOrDefault(x => x.id == testGroup.id);

            Assert.Null(deletedGroup);
        }
        #endregion
    }
    public class TestsClasses : TestsDBObjects
    {
        #region Классы
        [Fact]
        public void CreateClass_ShouldAddNewClassesAndDescriptorsToDB()
        {
            // Arrange
            string code = "test_code";
            string title = "test_title";
            string titleShort = "test_titleShort";
            string description = "test_description";

            // Act
            Classes addedClass = this.db.CreateClass(code, title, titleShort, description);

            // Assert
            // Проверка, что Descriptors добавлен в базу данных
            Descriptors addedDescriptor = this.db.DB.Descriptors.FirstOrDefault(d => d.id == addedClass.idDescriptor);
            Assert.NotNull(addedDescriptor);

            // Проверка, что Classes добавлен в базу данных
            Classes retrievedClass = this.db.DB.Classes.FirstOrDefault(c => c.Descriptors.code == code);
            Assert.NotNull(retrievedClass);

            // Проверка, что добавленный Classes ссылается на правильный Descriptors
            Assert.Equal(addedDescriptor.id, addedClass.Descriptors.id);
            this.context.Classes.Remove(addedClass);
        }
        [Fact]
        public void UpdateClass_ShouldUpdateClassesAndDescriptorInDB()
        {
            // Arrange
            string newCode = "updated_test_code";
            string newTitle = "updated_test_title";
            string newTitleShort = "updated_test_titleShort";
            string newDescription = "updated_test_description";

            // Создаем новый Classes для теста
            Classes testClass = this.db.CreateClass("initial_code", "initial_title", "initial_titleShort", "initial_description");
            int classId = testClass.id;

            // Act
            Classes updatedClass = this.db.UpdateClass(classId, newCode, newTitle, newTitleShort, newDescription);

            // Assert
            Assert.NotNull(updatedClass);

            // Проверка, что Descriptors был обновлен
            Descriptors updatedDescriptor = updatedClass.Descriptors;
            Assert.Equal(newCode, updatedDescriptor.code);
            Assert.Equal(newTitle, updatedDescriptor.title);
            Assert.Equal(newTitleShort, updatedDescriptor.titleShort);
            Assert.Equal(newDescription, updatedDescriptor.description);
            this.context.Classes.Remove(testClass);
        }
        [Fact]
        public void DeleteClass_ShouldRemoveClassesAndDescriptorFromDB()
        {
            // Arrange
            // Создаем новый Classes для теста
            Classes testClass = this.db.CreateClass("test_code", "test_title", "test_titleShort", "test_description");
            int classId = testClass.id;
            int classDescriptorId = testClass.Descriptors.id;
            // Act
            bool isDeleted = this.db.DeleteClass(classId);

            // Assert
            Assert.True(isDeleted);
            Assert.Null(this.db.DB.Classes.FirstOrDefault(c => c.id == classId));
            Assert.Null(this.db.DB.Descriptors.FirstOrDefault(d => d.id == classDescriptorId));
        }
        #endregion
    }
    public class TestsUnitsOfMeasurement : TestsDBObjects
    {
        #region Единицы измерения
        [Fact]
        public void CreateUnitProduct_ShouldAddUnitProductToDB()
        {
            // Arrange
            Products product = this.db.DB.Products.FirstOrDefault();
            Units unit = this.db.DB.Units.FirstOrDefault();
            UnitsTypes unitType = this.db.DB.UnitsTypes.FirstOrDefault();
            double value = 10.0;

            // Act
            UnitsProducts addedUnitProduct = this.db.CreateUnitProduct(product.id, unit.id, unitType.id, value);

            // Assert
            UnitsProducts retrievedUnitProduct = this.db.DB.UnitsProducts.FirstOrDefault(up => up.id == addedUnitProduct.id);
            Assert.NotNull(retrievedUnitProduct);
            Assert.Equal(value, retrievedUnitProduct.value);
            this.context.UnitsProducts.Remove(retrievedUnitProduct);
        }

        [Fact]
        public void UpdateUnitProduct_ShouldUpdateExistingUnitProductInDB()
        {
            // Arrange
            Products product = this.db.DB.Products.FirstOrDefault();
            Units unit = this.db.DB.Units.FirstOrDefault();
            UnitsTypes unitType = this.db.DB.UnitsTypes.FirstOrDefault();
            UnitsProducts newUnitProduct = this.db.CreateUnitProduct(product.id, unit.id, unitType.id, 10.0);
            double newValue = 20.0;

            // Act
            this.db.UpdateUnitProduct(newUnitProduct.id, unit.id, unitType.id, newValue);

            // Assert
            UnitsProducts updatedUnitProduct = this.db.DB.UnitsProducts.FirstOrDefault(up => up.id == newUnitProduct.id);
            Assert.NotNull(updatedUnitProduct);
            Assert.Equal(newValue, updatedUnitProduct.value);
            this.context.UnitsProducts.Remove(newUnitProduct);
        }

        [Fact]
        public void DeleteUnitProduct_ShouldRemoveUnitProductFromDB()
        {
            // Arrange
            Products product = this.db.DB.Products.FirstOrDefault();
            Units unit = this.db.DB.Units.FirstOrDefault();
            UnitsTypes unitType = this.db.DB.UnitsTypes.FirstOrDefault();
            UnitsProducts newUnitProduct = this.db.CreateUnitProduct(product.id, unit.id, unitType.id, 10.0);

            // Act
            this.db.DeleteUnitProduct(newUnitProduct.id);

            // Assert
            UnitsProducts deletedUnitProduct = this.db.DB.UnitsProducts.FirstOrDefault(up => up.id == newUnitProduct.id);
            Assert.Null(deletedUnitProduct);
        }
        #endregion
    }
    public class TestsArticles : TestsDBObjects
    {
        #region Артикулы
        [Fact]
        public void CreateVendorCodeTest()
        {
            // Arrange
            DBSEEntities context = this.db.DB;
            CustomDB service = this.db;

            // Act
            service.CreateVendorCode("test_code", "test_accountantCode", 1, true, true, true);
            VendorCodes addedItem = context.VendorCodes.FirstOrDefault(vc => vc.Descriptors.title == "test_code");

            // Assert
            Assert.NotNull(addedItem);
            context.VendorCodes.Remove(addedItem); // Удаляем для очистки
            context.SaveChanges();
        }

        [Fact]
        public void UpdateVendorCodeTest()
        {
            // Arrange
            DBSEEntities context = this.db.DB;
            CustomDB service = this.db;
            var item = new VendorCodes("initial_code", "initial_accountantCode", 1, true, true, true);
            context.VendorCodes.Add(item);
            context.SaveChanges();

            // Act
            service.UpdateVendorCode(item.id, "updated_code", "updated_accountantCode", 2, false, false, false);
            VendorCodes updatedItem = context.VendorCodes.Find(item.id);

            // Assert
            Assert.Equal("updated_code", updatedItem.Descriptors.title);
            context.VendorCodes.Remove(updatedItem); // Удаляем для очистки
            context.SaveChanges();
        }

        [Fact]
        public void DeleteVendorCodeTest()
        {
            // Arrange
            DBSEEntities context = this.db.DB;
            CustomDB service = this.db;
            var item = new VendorCodes("test_code_for_delete", "test_accountantCode_for_delete", 1, true, true, true);
            context.VendorCodes.Add(item);
            context.SaveChanges();

            // Act
            service.DeleteVendorCode(item.id);
            VendorCodes deletedItem = context.VendorCodes.Find(item.id);

            // Assert
            Assert.Null(deletedItem);
        }
        #endregion
    }
    public class TestsResources : TestsDBObjects
    {
        #region Ресурсы
        [Fact]
        public void CreateResource()
        {
            // Arrange
            Products product = this.context.Products.FirstOrDefault();
            int productId = product.id;
            string url = "https://example.com/resource";
            string filePath = "/path/to/your/file.txt";
            string resourceTitle = "TestResource";

            this.service.CreateResource(productId, url, filePath, resourceTitle);

            DescriptorsResources createdResource = this.context.DescriptorsResources.FirstOrDefault(dr => dr.title == resourceTitle);

            Assert.NotNull(createdResource); // Ресурс должен быть создан
        }

        [Fact]
        public void UpdateResource()
        {
            // Arrange
            Products product = this.context.Products.FirstOrDefault();
            int productId = product.id;
            //Act
            string updatedTitle = "UpdatedTitle";
            this.service.UpdateResource(productId, updatedTitle);
            DescriptorsResources descriptorResource = this.db.GetDescriptorsResources(product.Descriptors.id);
            DescriptorsResources updatedResource = this.context.DescriptorsResources.FirstOrDefault(
                dr => dr.title == updatedTitle && dr.idDescriptor == descriptorResource.idDescriptor);
            Assert.NotNull(updatedResource); // Название ресурса должно быть обновлено
        }

        [Fact]
        public void DeleteResource()
        {
            // Arrange
            Products product = this.context.Products.FirstOrDefault();
            int productId = product.id;
            this.service.DeleteResource(productId);

            DescriptorsResources deletedResource = this.context.DescriptorsResources.Find(productId);

            Assert.Null(deletedResource); // Ресурс должен быть удален
        }
        #endregion
    }

    #endregion
}
