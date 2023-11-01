using BaseWindow_WPF;
using CustomControlsWPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы
{
    public class CustomWindow : BaseWindow
    {


        #region Поля
        /// <summary>
        /// Здесь хранится имя, не сама строка подключения!
        /// </summary>
        private string currentConnectionString;
        private CustomBase customBase = new CustomBase();
        #endregion

        #region Свойства
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
        #endregion

        /// <summary>
        /// Пересоздаем контекст БД в зависимости от выбранной строки подключения
        /// </summary>
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
        public CustomWindow()
        {
            this.InitDB();
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
