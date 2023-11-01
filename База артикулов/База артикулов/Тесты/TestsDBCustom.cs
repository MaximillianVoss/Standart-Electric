using System;
using System.Linq;
using Xunit;
using База_артикулов.Классы;
using System.Configuration;
using База_артикулов.Модели;

namespace База_артикулов.База_Данных.Тесты
{
    public class TestsDBCustom
    {
        #region Поля
        public SettingsNew settings;
        public CustomDB db;
        #endregion

        #region Настройки
        public SettingsNew CreateSettingsWithConnectionString(string connectionStringName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            return new SettingsNew { CurrentConnectionString = connectionString };
        }

        #endregion

        #region Конструкторы
        public TestsDBCustom()
        {
            // Этот код будет выполняться перед каждым тестом в этом классе
            settings = CreateSettingsWithConnectionString("Подключение к LAPTOP-BBFM8MMD-Test");
            db = new CustomDB(settings);
        }
        #endregion

        #region Тесты БД
        [Fact]
        public void InstanceDB_ShouldReturnSameInstance()
        {
            var instance1 = CustomDB.Instance;
            var instance2 = CustomDB.Instance;

            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void UpdateDB_ShouldSetNewDBInstance()
        {
            var db = CustomDB.Instance.DB;

            CustomDB.Instance.Update();

            Assert.NotSame(db, CustomDB.Instance.DB);
        }

        [Fact]
        public void InitDB_ShouldSwitchToNewConnectionString()
        {
            var oldConnectionString = "old_connection_string";
            var newConnectionString = "new_connection_string";

            var oldSettings = new SettingsNew { CurrentConnectionString = oldConnectionString };
            var customDb = new CustomDB(oldSettings);

            // Проверяем, что изначально используется старая строка подключения
            Assert.Equal(oldConnectionString, customDb.CurrentConnectionString); // Предполагая, что у вас есть такое свойство.

            // Меняем на новую строку подключения
            oldSettings.CurrentConnectionString = newConnectionString;
            customDb.InitDB(true);

            // Проверяем, что теперь используется новая строка подключения
            Assert.Equal(newConnectionString, customDb.CurrentConnectionString);
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
            context = this.db.DB;
            service = this.db;
        }

        ~TestsDBObjects()
        {
            this.context.SaveChanges();
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

            var result = service.IsProductExists(productId);

            Assert.True(result);  // Убедитесь, что продукт с таким id существует в вашей базе данных.
        }
        #endregion

        #region Фильтрация товаров по классу,группе,подгруппе
        [Fact]
        public void FilterByClass_ReturnsCorrectResults()
        {
            var result = this.db.GetFilteredProducts(classValue: "Стальные конструкции КМ");
            Assert.All(result, product => Assert.Equal("Стальные конструкции КМ", product.Наименование_класса));
        }

        [Fact]
        public void FilterByGroup_ReturnsCorrectResults()
        {
            var result = this.db.GetFilteredProducts(group: "Эстакады модульные");
            Assert.All(result, product => Assert.Equal("Эстакады модульные", product.Наименование_группы));
        }

        [Fact]
        public void FilterBySubGroup_ReturnsCorrectResults()
        {
            var result = this.db.GetFilteredProducts(subGroup: "Фермы СМЭ.Ф");
            Assert.All(result, product => Assert.Equal("Фермы СМЭ.Ф", product.Наименование_подгруппы));
        }

        [Fact]
        public void FilterByAll_ReturnsCorrectResults()
        {
            var result = this.db.GetFilteredProducts("Эстакады модульные", "Стальные конструкции КМ", "Фермы СМЭ.Ф");

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
            var title = "TestTitle";
            var titleShort = "TestShort";
            var description = "TestDescription";
            var vendorCode = "12345";
            var idNorm = context.Norms.FirstOrDefault()?.id ?? 0; // Предполагая, что у вас есть таблица Norms
            var idSubGroup = context.SubGroups.FirstOrDefault()?.id ?? 0;
            var idCover = context.Covers.FirstOrDefault()?.id ?? 0;
            var idMaterial = context.Materials.FirstOrDefault()?.id ?? 0;
            var idPerforation = context.Perforations.FirstOrDefault()?.id ?? 0;
            var idPackage = context.Packages.FirstOrDefault()?.id ?? 0;
            var isInStock = true;

            // Act
            var createdProduct = service.CreateProduct(title, titleShort, description, vendorCode, idNorm, idSubGroup, idCover, idMaterial, idPerforation, idPackage, isInStock);

            // Assert
            Assert.NotNull(createdProduct);
            Assert.Equal(title, createdProduct.Descriptors.title);
            Assert.Equal(titleShort, createdProduct.Descriptors.titleShort);
            Assert.Equal(description, createdProduct.Descriptors.description);
            // Добавьте другие проверки при необходимости

            // Cleanup
            context.Products.Remove(createdProduct); // Удаляем созданный продукт после теста
            context.SaveChanges();
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
            var descriptor = db.CreateDescriptor("test_code", "test_title", "test_titleShort", "test_titleDisplay", "test_description");

            // Проверяем, что дескриптор был добавлен
            Assert.NotNull(db.DB.Descriptors.Find(descriptor.id));

            // Act
            var result = db.DeleteDescriptor(descriptor.id);

            // Assert
            Assert.True(result);
            Assert.Null(db.DB.Descriptors.Find(descriptor.id)); // Убеждаемся, что дескриптор был удален из БД
        }

        [Fact]
        public void UpdateDescriptor_ShouldUpdateAndReturnUpdatedDescriptor()
        {
            // Arrange
            settings = CreateSettingsWithConnectionString("Подключение к LAPTOP-BBFM8MMD-Test");
            db = new CustomDB(settings);
            var initialDescriptor = db.CreateDescriptor("initial_code", "initial_title", "initial_titleShort", "initial_titleDisplay", "initial_description");
            var initialId = initialDescriptor.id;

            try
            {
                var newCode = "updated_code";
                var newTitle = "updated_title";
                var newTitleShort = "updated_titleShort";
                var newTitleDisplay = "updated_titleDisplay";
                var newDescription = "updated_description";

                // Act
                var updatedDescriptor = db.UpdateDescriptor(initialId, newCode, newTitle, newTitleShort, newTitleDisplay, newDescription);

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
                db.DeleteDescriptor(initialId);
            }
        }

        [Fact]
        public void UpdateDescriptor_ShouldThrowExceptionWhenDescriptorNotFound()
        {
            // Arrange
            settings = CreateSettingsWithConnectionString("Подключение к LAPTOP-BBFM8MMD-Test");
            db = new CustomDB(settings);
            var nonExistentId = 999999; // предполагая, что такого ID нет в базе данных

            // Act & Assert
            Assert.Throws<Exception>(() => db.UpdateDescriptor(nonExistentId));
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
            db.DB.Descriptors.Add(originalDescriptor);
            db.DB.SaveChanges();

            try
            {
                // Assert initial creation
                var fetchedDescriptor = db.DB.Descriptors.Find(originalDescriptor.id);
                Assert.Equal("initial_code", fetchedDescriptor.code);
                Assert.Equal("initial_title", fetchedDescriptor.title);
                // Добавьте другие проверки здесь, если это необходимо

                // Act
                fetchedDescriptor.code = "updated_code";
                fetchedDescriptor.title = "updated_title";
                fetchedDescriptor.titleShort = "updated_titleShort";
                fetchedDescriptor.titleDisplay = "updated_titleDisplay";
                fetchedDescriptor.description = "updated_description";
                db.UpdateDescriptor(fetchedDescriptor);

                // Assert
                var updatedDescriptor = db.DB.Descriptors.Find(fetchedDescriptor.id);
                Assert.Equal("updated_code", updatedDescriptor.code);
                Assert.Equal("updated_title", updatedDescriptor.title);
                // Добавьте другие проверки здесь, чтобы удостовериться, что все поля были обновлены
            }
            finally
            {
                // Cleanup
                db.DeleteDescriptor(originalDescriptor.id);
            }
        }

        #region Проверки дексрипторов
        [Fact]
        public void IsDescriptorExists()
        {
            int descriptorId = 1;

            var result = service.IsDescriptorExists(descriptorId);

            Assert.True(result);
        }

        [Fact]
        public void IsDescriptorProductExists()
        {
            int productId = 1;

            var result = service.IsDescriptorProductExists(productId);

            Assert.True(result);
        }

        [Fact]
        public void IsDescriptorResourcesExists()
        {
            int descriptorId = this.context.DescriptorsResources.FirstOrDefault().idDescriptor;
            var result = service.IsDescriptorResourcesExists(descriptorId);
            Assert.True(result);
        }

        [Fact]
        public void GetDescriptorProduct()
        {
            int productId = 1;

            var descriptor = service.GetDescriptorProduct(productId);

            Assert.NotNull(descriptor);
        }

        [Fact]
        public void GetDescriptorsResources()
        {
            int descriptorId = this.context.DescriptorsResources.FirstOrDefault().idDescriptor;

            var descriptorResource = service.GetDescriptorsResources(descriptorId);

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

            // Act
            db.CreateSubGroup(code, title, titleShort, description, groupId, loadDiagramId);

            // Assert
            var createdSubGroup = this.db.DB.SubGroups.FirstOrDefault(x => x.Descriptors.code == code);
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

            // Act
            var subGroup = db.CreateSubGroup(code, title, titleShort, description, groupId, loadDiagramId);
            // Arrange
            int subGroupId = subGroup.id; // Убедитесь, что такой ID существует в вашей тестовой БД и что этот ID подходит для удаления
            int subGroupDescriptorId = subGroup.Descriptors.id;
            // Act
            db.DeleteSubGroup(subGroupId);

            // Assert
            var deletedSubGroup = this.db.DB.SubGroups.FirstOrDefault(x => x.id == subGroupId);
            Assert.Null(deletedSubGroup);

            var relatedDescriptor = this.db.DB.Descriptors.FirstOrDefault(x => x.id == subGroupDescriptorId);
            Assert.Null(relatedDescriptor);
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
            var createdSubGroup = db.CreateSubGroup(code, title, titleShort, description, groupId, loadDiagramId);
            int subGroupId = createdSubGroup.id;
            var originalSubGroup = this.db.DB.SubGroups.FirstOrDefault(x => x.id == subGroupId);
            Assert.NotNull(originalSubGroup);

            string updatedCode = "UpdatedCode";
            string updatedTitle = "UpdatedTitle";
            string updatedTitleShort = "UpdatedTitleShort";
            string updatedDescription = "UpdatedDescription";
            int updatedGroupId = 2; // Убедитесь, что такой ID существует в вашей тестовой БД
            int updatedLoadDiagramId = 2; // Убедитесь, что такой ID существует в вашей тестовой БД
            int applicationId = 1; // Убедитесь, что такой ID существует в вашей тестовой БД

            // Act
            db.UpdateSubGroup(originalSubGroup, updatedCode, updatedTitle, updatedTitleShort, updatedDescription, updatedGroupId, updatedLoadDiagramId, applicationId);

            // Assert
            var updatedSubGroup = this.db.DB.SubGroups.FirstOrDefault(x => x.id == subGroupId);
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
            var newGroup = db.CreateGroup("testCode", "testTitle", "testTitleShort", "testDescription", 1);

            Assert.NotNull(newGroup);
            Assert.Equal("testCode", newGroup.Descriptors.code);
            Assert.Equal("testTitle", newGroup.Descriptors.title);

            // Очистка созданного элемента после теста.
            db.DeleteGroup(newGroup.id);
        }

        [Fact]
        public void UpdateGroup_ShouldUpdateExistingGroup()
        {
            // Создание тестовой группы для обновления.
            var testGroup = db.CreateGroup("initialCode", "initialTitle", "initialTitleShort", "initialDescription", 1);

            db.UpdateGroup(testGroup.id, "updatedCode", "updatedTitle", "updatedTitleShort", "updatedDescription", 2);

            var updatedGroup = db.DB.Groups.FirstOrDefault(x => x.id == testGroup.id);

            Assert.NotNull(updatedGroup);
            Assert.Equal("updatedCode", updatedGroup.Descriptors.code);
            Assert.Equal("updatedTitle", updatedGroup.Descriptors.title);

            // Очистка созданного элемента после теста.
            db.DeleteGroup(testGroup.id);
        }

        [Fact]
        public void DeleteGroup_ShouldRemoveGroupFromDatabase()
        {
            // Создание тестовой группы для удаления.
            var testGroup = db.CreateGroup("testCode", "testTitle", "testTitleShort", "testDescription", 1);

            db.DeleteGroup(testGroup.id);

            var deletedGroup = db.DB.Groups.FirstOrDefault(x => x.id == testGroup.id);

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
            Classes addedClass = db.CreateClass(code, title, titleShort, description);

            // Assert
            // Проверка, что Descriptors добавлен в базу данных
            Descriptors addedDescriptor = db.DB.Descriptors.FirstOrDefault(d => d.id == addedClass.idDescriptor);
            Assert.NotNull(addedDescriptor);

            // Проверка, что Classes добавлен в базу данных
            Classes retrievedClass = db.DB.Classes.FirstOrDefault(c => c.Descriptors.code == code);
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
            Classes testClass = db.CreateClass("initial_code", "initial_title", "initial_titleShort", "initial_description");
            int classId = testClass.id;

            // Act
            Classes updatedClass = db.UpdateClass(classId, newCode, newTitle, newTitleShort, newDescription);

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
            Classes testClass = db.CreateClass("test_code", "test_title", "test_titleShort", "test_description");
            int classId = testClass.id;
            int classDescriptorId = testClass.Descriptors.id;
            // Act
            bool isDeleted = db.DeleteClass(classId);

            // Assert
            Assert.True(isDeleted);
            Assert.Null(db.DB.Classes.FirstOrDefault(c => c.id == classId));
            Assert.Null(db.DB.Descriptors.FirstOrDefault(d => d.id == classDescriptorId));
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
            var product = this.db.DB.Products.FirstOrDefault();
            var unit = this.db.DB.Units.FirstOrDefault();
            var unitType = this.db.DB.UnitsTypes.FirstOrDefault();
            double value = 10.0;

            // Act
            UnitsProducts addedUnitProduct = db.CreateUnitProduct(product.id, unit.id, unitType.id, value);

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
            var product = this.db.DB.Products.FirstOrDefault();
            var unit = this.db.DB.Units.FirstOrDefault();
            var unitType = this.db.DB.UnitsTypes.FirstOrDefault();
            UnitsProducts newUnitProduct = db.CreateUnitProduct(product.id, unit.id, unitType.id, 10.0);
            double newValue = 20.0;

            // Act
            db.UpdateUnitProduct(newUnitProduct.id, unit.id, unitType.id, newValue);

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
            var product = this.db.DB.Products.FirstOrDefault();
            var unit = this.db.DB.Units.FirstOrDefault();
            var unitType = this.db.DB.UnitsTypes.FirstOrDefault();
            UnitsProducts newUnitProduct = db.CreateUnitProduct(product.id, unit.id, unitType.id, 10.0);

            // Act
            db.DeleteUnitProduct(newUnitProduct.id);

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
            var context = this.db.DB;
            var service = this.db;

            // Act
            service.CreateVendorCode("test_code", "test_accountantCode", 1, true, true, true);
            var addedItem = context.VendorCodes.FirstOrDefault(vc => vc.Descriptors.title == "test_code");

            // Assert
            Assert.NotNull(addedItem);
            context.VendorCodes.Remove(addedItem); // Удаляем для очистки
            context.SaveChanges();
        }

        [Fact]
        public void UpdateVendorCodeTest()
        {
            // Arrange
            var context = this.db.DB;
            var service = this.db;
            var item = new VendorCodes("initial_code", "initial_accountantCode", 1, true, true, true);
            context.VendorCodes.Add(item);
            context.SaveChanges();

            // Act
            service.UpdateVendorCode(item.id, "updated_code", "updated_accountantCode", 2, false, false, false);
            var updatedItem = context.VendorCodes.Find(item.id);

            // Assert
            Assert.Equal("updated_code", updatedItem.Descriptors.title);
            context.VendorCodes.Remove(updatedItem); // Удаляем для очистки
            context.SaveChanges();
        }

        [Fact]
        public void DeleteVendorCodeTest()
        {
            // Arrange
            var context = this.db.DB;
            var service = this.db;
            var item = new VendorCodes("test_code_for_delete", "test_accountantCode_for_delete", 1, true, true, true);
            context.VendorCodes.Add(item);
            context.SaveChanges();

            // Act
            service.DeleteVendorCode(item.id);
            var deletedItem = context.VendorCodes.Find(item.id);

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
            var product = context.Products.FirstOrDefault();
            int productId = product.id;
            string url = "https://example.com/resource";
            string filePath = "/path/to/your/file.txt";
            string resourceTitle = "TestResource";

            service.CreateResource(productId, url, filePath, resourceTitle);

            var createdResource = context.DescriptorsResources.FirstOrDefault(dr => dr.title == resourceTitle);

            Assert.NotNull(createdResource); // Ресурс должен быть создан
        }

        [Fact]
        public void UpdateResource()
        {
            // Arrange
            var product = context.Products.FirstOrDefault();
            int productId = product.id;
            //Act
            string updatedTitle = "UpdatedTitle";
            service.UpdateResource(productId, updatedTitle);
            var descriptorResource = this.db.GetDescriptorsResources(product.Descriptors.id);
            var updatedResource = context.DescriptorsResources.FirstOrDefault(
                dr => dr.title == updatedTitle && dr.idDescriptor == descriptorResource.idDescriptor);
            Assert.NotNull(updatedResource); // Название ресурса должно быть обновлено
        }

        [Fact]
        public void DeleteResource()
        {
            // Arrange
            var product = context.Products.FirstOrDefault();
            int productId = product.id;
            service.DeleteResource(productId);

            var deletedResource = context.DescriptorsResources.Find(productId);

            Assert.Null(deletedResource); // Ресурс должен быть удален
        }
        #endregion
    }

    #endregion
}
