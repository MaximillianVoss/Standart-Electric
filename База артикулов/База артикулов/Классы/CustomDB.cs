using База_артикулов.Модели;
using System.Data.SqlClient;
using System.Configuration;

using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System;

namespace База_артикулов.Классы
{
    /// <summary>
    /// Специальный класс обертка для БД
    /// </summary>
    public class CustomDB
    {
        #region Поля

        /// <summary>
        /// База данных
        /// </summary>
        private DBSEEntities db;

        /// <summary>
        /// Экземпляр CustomDB
        /// </summary>
        private static CustomDB instance;

        /// <summary>
        /// Настройки
        /// </summary>
        private SettingsNew settings;
        #endregion

        #region Свойства

        public DBSEEntities DB { get => this.db; private set => this.db = value; }

        public string CurrentConnectionString { set => this.settings.CurrentConnectionString = value; get => this.settings.CurrentConnectionString; }

        public static CustomDB Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomDB();
                }
                return instance;
            }
        }

        #endregion

        #region Методы

        #region Строка подключения
        /// <summary>
        /// Обработчик события изменения строки подключения.
        /// Этот метод вызывается при обнаружении изменения строки подключения 
        /// в настройках. При этом происходит обновление контекста БД 
        /// с использованием новой строки подключения.
        /// </summary>
        /// <param name="newConnectionString">Новая строка подключения к БД.</param>
        private void HandleConnectionStringChange(string newConnectionString)
        {
            // Здесь ваш код обработки изменения строки подключения.
            // Например, вы можете вызвать InitDB или любой другой метод, 
            // который обновляет вашу базу данных с новой строкой подключения.
            InitDB(true);
        }
        #endregion

        #region Контекст БД
        /// <summary>
        /// Обновляет экземпляр контекста БД, создавая новый экземпляр <see cref="DBSEEntities"/>.
        /// Этот метод может быть использован для "сброса" текущего контекста 
        /// и начала работы с новым, чистым экземпляром.
        /// </summary>
        public void Update()
        {
            this.DB = new DBSEEntities();
        }


        /// <summary>
        /// Пересоздаем контекст БД в зависимости от выбранной строки подключения
        /// </summary>
        /// <param name="isForce">Обновить принудительно или нет</param>
        public void InitDB(bool isForce = false)
        {
            if (!isForce)
            {
                if (this.CurrentConnectionString != Settings.Connections.CurrentConnectionString)
                {
                    this.CurrentConnectionString = Settings.Connections.CurrentConnectionString;
                }
                if (this.DB == null)
                {
                    this.DB = new DBSEEntities(this.CurrentConnectionString);
                }
                else
                {
                    var builder1 = new SqlConnectionStringBuilder(this.DB.Database.Connection.ConnectionString);
                    EntityConnectionStringBuilder entityBuilder2 = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings[this.CurrentConnectionString].ConnectionString);
                    string sqlConnectionString2 = entityBuilder2.ProviderConnectionString;
                    SqlConnectionStringBuilder builder2 = new SqlConnectionStringBuilder(sqlConnectionString2);
                    if (builder1.DataSource != builder2.DataSource || builder1.InitialCatalog != builder2.InitialCatalog)
                    {
                        // строки подключения отличаются
                        this.DB = new DBSEEntities(this.CurrentConnectionString);
                    }

                }
            }
            else
            {
                this.DB = new DBSEEntities(this.CurrentConnectionString);
            }
        }
        #endregion

        #region Объекты БД

        #region Дескрипторы

        /// <summary>
        /// Создаёт и сохраняет новый дескриптор с предоставленными значениями или значениями по умолчанию.
        /// </summary>
        /// <param name="code">Код дескриптора. По умолчанию null.</param>
        /// <param name="title">Заголовок дескриптора. По умолчанию null.</param>
        /// <param name="titleShort">Короткое название дескриптора. По умолчанию null.</param>
        /// <param name="titleDisplay">Отображаемое название дескриптора. По умолчанию null.</param>
        /// <param name="description">Описание дескриптора. По умолчанию null.</param>
        /// <returns>Возвращает созданный объект дескриптора.</returns>
        public Descriptors CreateDescriptor(string code = null, string title = null, string titleShort = null, string titleDisplay = null, string description = null)
        {
            var descriptor = new Descriptors(code, title, titleShort, titleDisplay, description);
            this.DB.Descriptors.Add(descriptor);
            this.DB.SaveChanges();
            return descriptor;
        }

        /// <summary>
        /// Обновляет существующий дескриптор с предоставленными значениями.
        /// </summary>
        /// <param name="id">ID дескриптора, который нужно обновить.</param>
        /// <param name="code">Новый код дескриптора. Если не указан, то значение не изменяется.</param>
        /// <param name="title">Новый заголовок дескриптора. Если не указан, то значение не изменяется.</param>
        /// <param name="titleShort">Новое короткое название дескриптора. Если не указан, то значение не изменяется.</param>
        /// <param name="titleDisplay">Новое отображаемое название дескриптора. Если не указан, то значение не изменяется.</param>
        /// <param name="description">Новое описание дескриптора. Если не указан, то значение не изменяется.</param>
        /// <returns>Возвращает обновленный объект дескриптора или null, если дескриптор с указанным ID не найден.</returns>
        public Descriptors UpdateDescriptor(int id, string code = null, string title = null, string titleShort = null, string titleDisplay = null, string description = null)
        {
            var descriptor = this.DB.Descriptors.Find(id);

            if (descriptor == null)
            {
                throw new Exception($"Дескриптор с id:{id} не найден!");
            }

            if (code != null) descriptor.code = code;
            if (title != null) descriptor.title = title;
            if (titleShort != null) descriptor.titleShort = titleShort;
            if (titleDisplay != null) descriptor.titleDisplay = titleDisplay;
            if (description != null) descriptor.description = description;

            this.DB.SaveChanges();
            return descriptor;
        }

        /// <summary>
        /// Обновляет существующий дескриптор с значениями из предоставленного дескриптора.
        /// </summary>
        /// <param name="descriptorToUpdate">Дескриптор с новыми значениями для обновления.</param>
        /// <returns>Возвращает обновленный объект дескриптора или null, если дескриптор с указанным ID не найден.</returns>
        public Descriptors UpdateDescriptor(Descriptors descriptorToUpdate)
        {
            var descriptor = this.DB.Descriptors.Find(descriptorToUpdate.id);

            if (descriptor == null)
            {
                throw new Exception($"Дескриптор с id:{descriptorToUpdate.id} не найден!");
            }

            descriptor.code = descriptorToUpdate.code;
            descriptor.title = descriptorToUpdate.title;
            descriptor.titleShort = descriptorToUpdate.titleShort;
            descriptor.titleDisplay = descriptorToUpdate.titleDisplay;
            descriptor.description = descriptorToUpdate.description;

            this.DB.SaveChanges();
            return descriptor;
        }

        /// <summary>
        /// Удаляет дескриптор по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор дескриптора, который необходимо удалить.</param>
        /// <returns>Возвращает true, если удаление прошло успешно, иначе false.</returns>
        /// <exception cref="Exception">Вызывается, если дескриптор с указанным идентификатором не найден.</exception>
        public bool DeleteDescriptor(int id)
        {
            var descriptor = this.DB.Descriptors.Find(id); // Предположим, что у вашего контекста DB есть свойство Descriptors, представляющее таблицу дескрипторов.

            if (descriptor == null)
            {
                throw new Exception($"Дескриптор с id:{id} не найден!");
            }

            var resourcesToRemove = this.DB.DescriptorsResources.Where(x => x.idDescriptor == descriptor.id);
            this.DB.Descriptors.Remove(descriptor);
            this.DB.DescriptorsResources.RemoveRange(resourcesToRemove);
            this.DB.SaveChanges();

            return true;
        }

        #endregion

        #region Подгруппа
        public SubGroups CreateSubGroup(
            string code,
            string title,
            string titleShort,
            string description,
            int groupId,
        int loadDiagramId)
        {
            // Создаем новый объект Descriptors и добавляем его в базу данных
            Descriptors descriptor = CreateDescriptor(code, title, titleShort, description);

            // Создаем новый объект SubGroups и добавляем его в базу данных
            var subGroup = this.DB.SubGroups.Add(new SubGroups(
                descriptor,
                this.DB.Groups.FirstOrDefault(x => x.id == groupId),
                this.DB.LoadDiagrams.FirstOrDefault(x => x.id == loadDiagramId)
            ));

            // Сохраняем изменения в базе данных
            this.DB.SaveChanges();
            return subGroup;
        }

        public void UpdateSubGroup(
            SubGroups currentItem,
            string code,
            string title,
            string titleShort,
            string description,
            int groupId,
            int loadDiagramId,
            int applicationId)
        {
            // Проверяем, задан ли CurrentItem
            if (currentItem == null || !(currentItem is SubGroups currentSubGroup))
                throw new Exception("Редактируемый элемент не является классом");

            // Извлекаем соответствующую подгруппу из базы данных
            currentSubGroup = this.DB.SubGroups.FirstOrDefault(x => x.id == currentSubGroup.id);

            // Обновляем descriptor
            UpdateDescriptor(
                currentSubGroup.idDescriptor,
                code,
                title,
                titleShort,
                description
            );

            // Обновляем текущую подгруппу
            currentSubGroup.Groups = this.DB.Groups.FirstOrDefault(x => x.id == groupId);
            currentSubGroup.LoadDiagrams = this.DB.LoadDiagrams.FirstOrDefault(x => x.id == loadDiagramId);

            var subGroupApplication = this.DB.GroupsApplications.FirstOrDefault(x => x.idSubGroup == currentSubGroup.id);
            if (subGroupApplication != null)
            {
                subGroupApplication.Applications = this.DB.Applications.FirstOrDefault(x => x.id == applicationId);
            }
            else
            {
                this.DB.GroupsApplications.Add(new GroupsApplications(
                    currentSubGroup,
                    this.DB.Applications.FirstOrDefault(x => x.id == applicationId)
                ));
            }

            // Сохраняем изменения в базе данных
            this.DB.SaveChanges();
        }

        public void DeleteSubGroup(int subGroupId)
        {
            // Находим подгруппу, которую нужно удалить
            SubGroups subGroupToDelete = this.DB.SubGroups.FirstOrDefault(x => x.id == subGroupId);

            if (subGroupToDelete == null)
            {
                throw new Exception($"Подгруппа с ID {subGroupId} не найдена.");
            }

            // Сохраняем ID дескриптора, связанного с этой подгруппой, для дальнейшего удаления
            int descriptorId = subGroupToDelete.idDescriptor;
            var subGroupsApplicationsToDelete = this.DB.GroupsApplications.Where(x => x.idSubGroup == subGroupToDelete.id);
            this.DB.GroupsApplications.RemoveRange(subGroupsApplicationsToDelete);
            // Удаляем подгруппу
            this.DB.SubGroups.Remove(subGroupToDelete);

            // Находим и удаляем дескриптор, связанный с удаленной подгруппой
            Descriptors descriptorToDelete = this.DB.Descriptors.FirstOrDefault(x => x.id == descriptorId);
            if (descriptorToDelete != null)
            {
                this.DB.Descriptors.Remove(descriptorToDelete);
            }

            // Сохраняем изменения в базе данных
            this.DB.SaveChanges();
        }


        #endregion

        #region Группа
        /// <summary>
        /// Создает новую группу и связанный с ней дескриптор.
        /// </summary>
        /// <param name="code">Код дескриптора.</param>
        /// <param name="title">Заголовок дескриптора.</param>
        /// <param name="titleShort">Сокращенный заголовок дескриптора.</param>
        /// <param name="description">Описание дескриптора.</param>
        /// <param name="classId">ID класса, к которому принадлежит группа.</param>
        /// <returns>Созданная сущность группы.</returns>
        public Groups CreateGroup(
            string code,
            string title,
            string titleShort,
            string description,
            int classId)
        {
            Descriptors descriptor = new Descriptors(code, title, titleShort, description);
            this.DB.Descriptors.Add(descriptor);

            Groups newGroup = new Groups(descriptor, this.DB.Classes.FirstOrDefault(x => x.id == classId));
            this.DB.Groups.Add(newGroup);

            this.DB.SaveChanges();

            return newGroup;
        }
        /// <summary>
        /// Обновляет существующую группу и связанный с ней дескриптор.
        /// </summary>
        /// <param name="groupId">ID группы, которую необходимо обновить.</param>
        /// <param name="code">Новый код дескриптора.</param>
        /// <param name="title">Новый заголовок дескриптора.</param>
        /// <param name="titleShort">Новый сокращенный заголовок дескриптора.</param>
        /// <param name="description">Новое описание дескриптора.</param>
        /// <param name="classId">ID класса, к которому принадлежит группа.</param>
        public void UpdateGroup(
            int groupId,
            string code,
            string title,
            string titleShort,
            string description,
            int classId)
        {
            Groups currentGroup = this.DB.Groups.FirstOrDefault(x => x.id == groupId);
            if (currentGroup == null)
            {
                throw new Exception($"Группа с ID {groupId} не найдена.");
            }

            Descriptors descriptor = UpdateDescriptor(currentGroup.idDescriptor, code, title, titleShort, description);

            currentGroup.Descriptors = descriptor;
            currentGroup.Classes = this.DB.Classes.FirstOrDefault(x => x.id == classId);

            this.DB.SaveChanges();
        }
        /// <summary>
        /// Удаляет группу и связанный с ней дескриптор из базы данных.
        /// </summary>
        /// <param name="groupId">ID группы, которую необходимо удалить.</param>
        public void DeleteGroup(int groupId)
        {
            Groups groupToDelete = this.DB.Groups.FirstOrDefault(x => x.id == groupId);
            if (groupToDelete == null)
            {
                throw new Exception($"Группа с ID {groupId} не найдена.");
            }

            int descriptorId = groupToDelete.idDescriptor;

            this.DB.Groups.Remove(groupToDelete);

            Descriptors descriptorToDelete = this.DB.Descriptors.FirstOrDefault(x => x.id == descriptorId);
            if (descriptorToDelete != null)
            {
                this.DB.Descriptors.Remove(descriptorToDelete);
            }

            this.DB.SaveChanges();
        }

        #endregion

        #region Класс
        /// <summary>
        /// Добавляет новый объект Descriptors и связанный объект Classes в базу данных.
        /// </summary>
        /// <param name="code">Код дескриптора.</param>
        /// <param name="title">Заголовок дескриптора.</param>
        /// <param name="titleShort">Сокращенный заголовок дескриптора.</param>
        /// <param name="description">Описание дескриптора.</param>
        /// <returns>Возвращает созданный объект Classes.</returns>
        public Classes CreateClass(string code, string title, string titleShort, string description)
        {
            // Создаем новый объект Descriptors        
            Descriptors descriptor = this.CreateDescriptor(code, title, titleShort, title, description);

            // Добавляем Descriptors в базу данных
            this.DB.Descriptors.Add(descriptor);
            this.DB.SaveChanges();

            // Создаем новый объект Classes с ссылкой на созданный Descriptors
            Classes newClass = new Classes(descriptor);

            // Добавляем Classes в базу данных
            this.DB.Classes.Add(newClass);
            this.DB.SaveChanges();

            // Возвращаем созданный объект Classes
            return newClass;
        }
        /// <summary>
        /// Обновляет существующий объект Classes и связанный объект Descriptors в базе данных.
        /// </summary>
        /// <param name="classId">ID обновляемого объекта Classes.</param>
        /// <param name="code">Новый код дескриптора.</param>
        /// <param name="title">Новый заголовок дескриптора.</param>
        /// <param name="titleShort">Новый сокращенный заголовок дескриптора.</param>
        /// <param name="description">Новое описание дескриптора.</param>
        /// <returns>Возвращает обновленный объект Classes.</returns>
        public Classes UpdateClass(int classId, string code, string title, string titleShort, string description)
        {
            // Находим объект Classes по ID
            Classes existingClass = this.DB.Classes.FirstOrDefault(c => c.id == classId);

            if (existingClass == null)
            {
                throw new Exception($"Объект Classes с указанным ID '{classId}' не найден.");
            }

            // Находим связанный объект Descriptors
            Descriptors existingDescriptor = existingClass.Descriptors;

            if (existingDescriptor == null)
            {
                throw new Exception("Дескриптор для указанного класса не найден.");
            }
            this.UpdateDescriptor(existingDescriptor.id, code, title, titleShort, title, description);
            // Сохраняем изменения в базе данных
            this.DB.SaveChanges();

            return existingClass;
        }
        /// <summary>
        /// Удаляет объект Classes и связанный с ним объект Descriptors из базы данных.
        /// </summary>
        /// <param name="classId">ID объекта Classes для удаления.</param>
        /// <returns>Возвращает true, если удаление прошло успешно, и false в противном случае.</returns>
        public bool DeleteClass(int classId)
        {
            // Находим объект Classes по ID
            Classes existingClass = this.DB.Classes.FirstOrDefault(c => c.id == classId);

            if (existingClass == null)
            {
                throw new Exception($"Объект Classes с указанным ID '{classId}' не найден.");
            }

            // Находим связанный объект Descriptors
            Descriptors existingDescriptor = existingClass.Descriptors;

            // Удаляем Classes и Descriptors из базы данных
            this.DB.Classes.Remove(existingClass);
            this.DB.Descriptors.Remove(existingDescriptor);

            // Сохраняем изменения в базе данных
            int result = this.DB.SaveChanges();

            // Если было удалено два объекта (Classes и Descriptors), возвращаем true
            return result == 2;
        }
        #endregion

        #region Единицы измерения
        /// <summary>
        /// Создает новый объект UnitsProducts и добавляет его в базу данных.
        /// </summary>
        /// <param name="idUnit">ID измерения.</param>
        /// <param name="idType">ID типа измерения.</param>
        /// <param name="value">Значение измерения.</param>
        /// <returns>Возвращает созданный объект UnitsProducts.</returns>
        public UnitsProducts CreateUnitProduct(int idUnit, int idType, double value)
        {
            var newUnitProduct = new UnitsProducts
            {
                idUnit = idUnit,
                idType = idType,
                value = value
            };

            this.DB.UnitsProducts.Add(newUnitProduct);
            this.DB.SaveChanges();

            return newUnitProduct;
        }
        /// <summary>
        /// Обновляет существующий объект UnitsProducts в базе данных.
        /// </summary>
        /// <param name="unitProductId">ID объекта UnitsProducts для обновления.</param>
        /// <param name="idUnit">Новое значение ID измерения.</param>
        /// <param name="idType">Новое значение ID типа измерения.</param>
        /// <param name="value">Новое значение измерения.</param>
        public void UpdateUnitProduct(int unitProductId, int idUnit, int idType, double value)
        {
            var unitProduct = this.DB.UnitsProducts.FirstOrDefault(x => x.id == unitProductId);

            if (unitProduct == null)
                throw new Exception($"Измерение с ID '{unitProductId}' не найдено.");

            unitProduct.idUnit = idUnit;
            unitProduct.idType = idType;
            unitProduct.value = value;

            this.DB.SaveChanges();
        }
        /// <summary>
        /// Удаляет объект UnitsProducts из базы данных.
        /// </summary>
        /// <param name="unitProductId">ID объекта UnitsProducts для удаления.</param>
        public void DeleteUnitProduct(int unitProductId)
        {
            var unitProduct = this.DB.UnitsProducts.FirstOrDefault(x => x.id == unitProductId);

            if (unitProduct == null)
                throw new Exception($"Измерение с ID '{unitProductId}' не найдено.");

            this.DB.UnitsProducts.Remove(unitProduct);
            this.DB.SaveChanges();
        }

        #endregion

        #region Товары

        #endregion

        #endregion

        #endregion

        #region Конструкторы/Деструкторы

        public CustomDB() : this(new SettingsNew())
        {

        }

        public CustomDB(SettingsNew settings)
        {
            this.settings = settings;
            this.settings.CurrentConnectionStringChanged += HandleConnectionStringChange;
            this.Update();
        }

        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion
    }
}
