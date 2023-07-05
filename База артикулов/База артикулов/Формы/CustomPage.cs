using CustomControlsWPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы
{
    public class CustomPage : BaseWindow_WPF.BasePage
    {

        #region Поля
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
        /// <param name="items">коллекция объектов (поля:id,title)</param>
        public void UpdateComboBox(LabeledComboBox labeledComboBox, List<object> items)
        {
            this.customBase.UpdateComboBox(labeledComboBox, items);
        }
        /// <summary>
        /// Заполняет LabeledTextBoxAndComboBox элементами из указанной коллекции
        /// </summary>
        /// <param name="labeledComboBox">элемент управления</param>
        /// <param name="items">коллекция объектов (поля:id,title)</param>
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

        #endregion

        #region Конструкторы/Деструкторы

        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion

    }
}
