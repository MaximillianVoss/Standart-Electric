using CustomControlsWPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы
{
    /// <summary>
    /// Обеспечивает взаимодействие с БД и облачным хранилищем, 
    /// также позволяет вносить информацию в элементы управления должным образом
    /// </summary>
    public class CustomBase
    {


        #region Поля
        /// <summary>
        /// База данных
        /// </summary>
        private DBSEEntities db = new DBSEEntities();
        /// <summary>
        /// webDAV-клиент
        /// </summary>
        private WDClient wdClient = new WDClient(
                "devstor",
                "TE6db?lZE~8Ixc?KAtQW",
                "https://rcloud.rsk-gr.ru",
                "/remote.php/dav/files/devstor"
                );
        #endregion

        #region Свойства
        /// <summary>
        /// webDAV-клиент
        /// </summary>
        public WDClient WDClient { get => this.wdClient; set => this.wdClient = value; }
        /// <summary>
        /// База данных
        /// </summary>
        public DBSEEntities DB { get => this.db; set => this.db = value; }
        #endregion

        #region Методы

        #region Работа с облачным WebDav-клиентом
        /// <summary>
        /// Инициализирует WebDAV-клиент
        /// </summary>
        public void InitClient()
        {
            this.WDClient = new WDClient(
                "devstor",
                "TE6db?lZE~8Ixc?KAtQW",
                "https://rcloud.rsk-gr.ru",
                "/remote.php/dav/files/devstor"
                );
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
            //TODO: падает при выделении другой таблицы, проверить!
            if (obj == null)
            {
                throw new Exception("Не передан объект для получения поля!");
            }

            if (String.IsNullOrEmpty(fieldName))
            {
                throw new Exception("Не передано имя поля!");
            }

            var field = obj.GetType().GetProperty(fieldName);
            return field == null
                ? throw new Exception(База_артикулов.Классы.Common.Strings.Errors.fieldIsNotFoundInObject)
                : field.GetValue(obj, null);
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
            labeledComboBox.Items = items;
        }
        /// <summary>
        /// Заполняет LabeledTextBoxAndComboBox элементами из указанной коллекции
        /// </summary>
        /// <param name="labeledComboBox">элемент управления</param>
        /// <param name="items">коллекция объектов (поля:id,Title)</param>
        public void UpdateComboBox(LabeledTextBoxAndComboBox labeledComboBox, List<object> items)
        {
            labeledComboBox.Items = items;
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
            labeledCheckBox.IsCheckedTrue = isCheckedStr;
            labeledCheckBox.IsCheckedFalse = isUncheckedStr;
            labeledCheckBox.IsChecked = isChecked;
        }
        /// <summary>
        /// Заполняет LabeledTextBox указанным значением
        /// </summary>
        /// <param name="labeledTextBox">элемент управления</param>
        /// <param name="value">значение</param>
        /// <param name="defaultValue">значение по умолчанию(если основное знаение null)</param>
        public void UpdateTextBox(LabeledTextBox labeledTextBox, object value, string defaultValue)
        {
            labeledTextBox.Text = value == null ? defaultValue : value.ToString();
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
            var collectionWithType = typeof(List<>).MakeGenericType(type);
            return Activator.CreateInstance(collectionWithType, items);
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
            List<object> list = new List<object>();
            foreach (var item in items)
            {
                var type = item.GetType();
                var methodInfo = type.GetMethod("ToObject");
                list.Add(methodInfo.Invoke(item, null));
            }
            return list;
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
            List<object> list = new List<object>();
            foreach (T item in items)
            {
                var type = item.GetType();
                var methodInfo = type.GetMethod("ToObject");
                list.Add(methodInfo.Invoke(item, null));
            }
            return list;
        }
        #endregion

        #endregion

        #region Конструкторы/Деструкторы

        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion


    }
}
