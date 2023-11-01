using CustomControlsWPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы
{
    public class MyEventArgs : EventArgs
    {
        public object Data { get; set; }
    }

    public class CustomPage : BaseWindow_WPF.BasePage
    {

        #region Поля
        public delegate void DataChangedEventHandler(object sender, MyEventArgs e);
        public event DataChangedEventHandler DataChanged;

        /// <summary>
        /// Здесь хранится имя, не сама строка подключения!
        /// </summary>
        private string currentConnectionString;
        private CustomBase customBase = new CustomBase();
        #endregion

        #region Свойства
        public CustomBase CustomBase { set; get; }
        /// <summary>
        /// Была ли коллекция обновлена.
        /// Например, бул удален или добавлен, изменен элемент любой из таблиц.
        /// </summary>
        public bool IsCollectionUpdated { set; get; }
        /// <summary>
        /// webDAV-клиент
        /// </summary>
        public WDClient WDClient
        {
            get => this.customBase.WDClient;
            set => this.customBase.WDClient = value;
        }
        /// <summary>
        /// База данных
        /// </summary>
        public DBSEEntities DB
        {
            get => this.customBase.DB;
            set => this.customBase.DB = value;
        }
        #endregion

        #region Методы

        public bool IsTypeEqual(Type type, object @object)
        {
            return @object.GetType() == type || @object.GetType().BaseType == type;
        }
        /// <summary>
        /// Вызываем при изменении данных внутри страницы
        /// </summary>
        /// <param name="data"></param>
        public void OnDataChanged(object data)
        {
            DataChanged?.Invoke(this, new MyEventArgs { Data = data });
        }

        public Products CreateEmptyProduct(string title = "Новый продукт", SubGroups subGroups = null)
        {
            // Создание нового дескриптора
            var descriptor = new Descriptors
            {
                title = title
            };

            // Сохранение дескриптора в БД
            this.DB.Descriptors.Add(descriptor);
            this.DB.SaveChanges();

            // Создание нового продукта со значениями по умолчанию
            var product = new Products
            {
                Descriptors = descriptor,
                Covers = this.DB.Covers.FirstOrDefault(x => x.id > 0),
                SubGroups = subGroups
                // Добавьте здесь другие значения по умолчанию, если они есть
            };

            // Сохранение продукта в БД
            product = this.DB.Products.Add(product);
            this.DB.SaveChanges();
            return product;
        }
        #region Работа с товарами
        /// <summary>
        /// Получает товар с указанным id
        /// </summary>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        public Products GetProduct(int idProduct)
        {
            return this.DB.Products.FirstOrDefault(x => x.id == idProduct);
        }
        /// <summary>
        /// Получает представление продукта с указаным id
        /// </summary>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        public ProductsView GetProductView(int idProduct)
        {
            return this.DB.ProductsView.FirstOrDefault(x => x.ID_продукта == idProduct);
        }
        /// <summary>
        /// Поверяет наличие товара в базе
        /// </summary>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        public bool IsProductExists(int idProduct)
        {
            return this.GetProduct(idProduct) != null;
        }
        /// <summary>
        /// Получает дескриптор товара с указанным id
        /// </summary>
        /// <param name="idProduct">id товара</param>
        /// <returns></returns>
        public Descriptors GetDescriptorProduct(int idProduct)
        {
            var product = this.GetProduct(idProduct);
            if (product == null)
                return null;
            else
                return this.DB.Descriptors.FirstOrDefault(x => x.id == product.idDescriptor);
        }
        /// <summary>
        /// Проверяет наличие дескриптора (любого) в базе
        /// </summary>
        /// <param name="idDescriptor">id дексриптора</param>
        /// <returns></returns>
        public bool IsDescriptorExists(int idDescriptor)
        {
            return this.DB.Descriptors.FirstOrDefault(x => x.id == idDescriptor) != null;
        }
        /// <summary>
        /// Проверяет наличие дескриптора товара в базе
        /// </summary>
        /// <param name="idProduct">id товара</param>
        /// <returns></returns>
        public bool IsDescriptorProductExists(int idProduct)
        {
            if (this.IsProductExists(idProduct))
            {
                return this.GetDescriptorProduct(idProduct) != null;
            }
            return false;
        }
        public DescriptorsResources GetDescriptorsResources(int idDescriptor)
        {
            return this.DB.DescriptorsResources.FirstOrDefault(x => x.idDescriptor == idDescriptor);
        }

        public bool IsDescriptorResourcesExists(int idDescriptor)
        {
            return this.GetDescriptorsResources(idDescriptor) != null;
        }
        #endregion

        #region Работа с дескриптором
        /// <summary>
        /// Загружает дескриптор с указанным ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Descriptors Load(int id)
        {
            return this.DB.Descriptors.FirstOrDefault(x => x.id == id);
        }
        /// <summary>
        /// Сохраняет дескриптор с указанным ID
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public Descriptors Save(Descriptors descriptor)
        {
            return this.Save(descriptor.id, descriptor.code, descriptor.title, descriptor.titleShort, descriptor.titleDisplay, descriptor.description);
        }
        /// <summary>
        /// Сохраняет дескриптор с указанным ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="title"></param>
        /// <param name="titleShort"></param>
        /// <param name="titleDisplay"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Descriptors Save(int id, string code, string title, string titleShort, string titleDisplay, string description)
        {
            var descriptor = this.Load(id) ?? throw new Exception($"Дескриптор с id:{id} не найден!");
            descriptor.code = code;
            descriptor.title = title;
            descriptor.titleShort = titleShort;
            descriptor.titleDisplay = titleDisplay;
            descriptor.description = description;
            this.DB.SaveChanges();
            return descriptor;
        }
        #endregion

        #region Работа с облачным WebDav-клиентом
        /// <summary>
        /// Инициализирует WebDAV-клиент
        /// </summary>
        public void InitClient()
        {
            this.customBase.InitClient();
        }
        #endregion

        #region Работа с объектами
        /// <summary>
        /// Получает указанное поле из объекта
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public object GetObjectFieldValue(object obj, string fieldName)
        {
            return this.customBase.GetObjectFieldValue(obj, fieldName);
        }
        #endregion

        #region Обновление элементов управления
        /// <summary>
        /// Заполняет LabeledComboBox элементами из указанной коллекции
        /// </summary>
        /// <param name="labeledComboBox">элемент управления</param>
        /// <param name="items">коллекция объектов (поля:id,Title)</param>
        public void UpdateComboBox(LabeledComboBox labeledComboBox, List<object> items)
        {
            this.customBase.UpdateComboBox(labeledComboBox, items);
        }
        /// <summary>
        /// Заполняет LabeledTextBoxAndComboBox элементами из указанной коллекции
        /// </summary>
        /// <param name="labeledComboBox">элемент управления</param>
        /// <param name="items">коллекция объектов (поля:id,Title)</param>
        public void UpdateComboBox(LabeledTextBoxAndComboBox labeledComboBox, List<object> items)
        {
            this.customBase.UpdateComboBox(labeledComboBox, items);
        }
        /// <summary>
        /// Обновляет значение LabeledCheckBox указанными значениями
        /// </summary>
        /// <param name="labeledCheckBox">элемент управления</param>
        /// <param name="isCheckedStr">сообщение, когда галочка установлена</param>
        /// <param name="isUncheckedStr">сообщение, когда галочка НЕ установлена</param>
        /// <param name="isChecked">начальное значение LabeledCheckBox</param>
        public void UpdateCheckBox(LabeledCheckBox labeledCheckBox, string isCheckedStr, string isUncheckedStr, bool isChecked)
        {
            this.customBase.UpdateCheckBox(labeledCheckBox, isCheckedStr, isUncheckedStr, isChecked);
        }
        /// <summary>
        /// Заполняет LabeledTextBox указанным значением
        /// </summary>
        /// <param name="labeledTextBox">элемент управления</param>
        /// <param name="value">значение</param>
        /// <param name="defaultValue">значение по умолчанию(если основное знаение null)</param>
        public void UpdateTextBox(LabeledTextBox labeledTextBox, object value, string defaultValue)
        {
            this.customBase.UpdateTextBox(labeledTextBox, value, defaultValue);
        }
        #endregion

        #region Преобразование в списки
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object ToList(IEnumerable items, Type type)
        {
            return this.customBase.ToList(items, type);
        }
        /// <summary>
        /// Получает список объектов из указанного DbSet.
        /// Для заполнения у объекта должен быть метод ToObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<object> ToList<T>(DbSet items)
        {
            return this.customBase.ToList<T>(items);
        }
        /// <summary>
        /// Получает список объектов из указанного List.
        /// Для заполнения у объекта должен быть метод ToObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<object> ToList<T>(List<T> items)
        {
            return this.customBase.ToList<T>(items);
        }

        public List<object> ToList(List<IToObject> items)
        {
            return items.Select(item => item.ToObject()).ToList();
        }
        #endregion

        /// <summary>
        /// Закрывает родительское окно
        /// </summary>
        /// <param name="dialogResult">Были внесены изменения в окне или нет</param>
        public void CloseWindow(bool? dialogResult = false)
        {
            var window = Window.GetWindow(this);
            window.DialogResult = dialogResult;
            window?.Close();
        }

        /// <summary>
        /// Пересоздаем контекст БД в зависимости от выбранной строки подключения
        /// </summary>
        /// <param name="isForce">Обновить принудительно или нет</param>
        public void InitDB(bool isForce = false)
        {
            if (!isForce)
            {
                if (this.currentConnectionString != Settings.Connections.CurrentConnectionString)
                {
                    this.currentConnectionString = Settings.Connections.CurrentConnectionString;
                }
                if (this.DB == null)
                {
                    this.DB = new DBSEEntities(this.currentConnectionString);
                }
                else
                {
                    var builder1 = new SqlConnectionStringBuilder(this.DB.Database.Connection.ConnectionString);
                    EntityConnectionStringBuilder entityBuilder2 = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings[this.currentConnectionString].ConnectionString);
                    string sqlConnectionString2 = entityBuilder2.ProviderConnectionString;
                    SqlConnectionStringBuilder builder2 = new SqlConnectionStringBuilder(sqlConnectionString2);
                    if (builder1.DataSource != builder2.DataSource || builder1.InitialCatalog != builder2.InitialCatalog)
                    {
                        // строки подключения отличаются
                        this.DB = new DBSEEntities(this.currentConnectionString);
                    }

                }
            }
            else
            {
                this.DB = new DBSEEntities(this.currentConnectionString);
            }
        }

        #endregion

        #region Конструкторы/Деструкторы
        public CustomPage()
        {
            #region Подпись на события при выборе другой строки подключения
            Settings.Connections.CurrentConnectionStringChanged += (newConnectionString) =>
            {
                this.InitDB();
            };
            #endregion
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion

    }
}
