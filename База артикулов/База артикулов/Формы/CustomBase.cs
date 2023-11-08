using CustomControlsWPF;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы
{
    /// <summary>
    /// Возможные режимы окна или формы
    /// </summary>
    public enum EditModes
    {
        Create,
        Edit,
        Delete,
        None
    }

    public static class ObjectExtensions
    {
        /// <summary>
        /// Получает указанное поле из объекта
        /// </summary>
        /// <param name="obj">Целевой объект.</param>
        /// <param name="fieldName">Имя поля.</param>
        /// <returns>Значение поля.</returns>
        public static object GetFieldValue(this object obj, string fieldName)
        {
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
        /// <summary>
        /// Проверяет, является ли тип объекта или его базовый тип указанным типом.
        /// </summary>
        /// <param name="obj">Объект для проверки.</param>
        /// <param name="type">Тип для сравнения.</param>
        /// <returns>True, если тип объекта или его базовый тип совпадает с указанным типом, иначе false.</returns>
        public static bool IsTypeOrBaseEqual(this object obj, Type type)
        {
            return obj.GetType() == type || obj.GetType().BaseType == type;
        }
    }

    /// <summary>
    /// Обеспечивает взаимодействие с БД и облачным хранилищем, 
    /// также позволяет вносить информацию в элементы управления должным образом
    /// </summary>
    public class CustomBase
    {

        #region Поля
        private List<CustomEventArgs> currentObjects = new List<CustomEventArgs>();
        private CustomEventArgs currentObject = null;
        #endregion

        #region Свойства
        /// <summary>
        /// Текущий режим: создание/редактирование/удаление
        /// </summary>
        public EditModes Mode { set; get; }
        /// <summary>
        /// Объекты с которыми в данный момент взаимодействует окно/страница, обычно передаются ему в качестве параметров
        /// </summary>
        public List<CustomEventArgs> CurrentObjects
        {
            get => currentObjects;
            set
            {
                if (value == null)
                    this.currentObjects = new List<CustomEventArgs>();
                else
                    this.currentObjects = value;
            }
        }
        /// <summary>
        /// Объект возвращаемый окном/страницей после закрытия,
        /// NULL - если ничего не требуется возвращать
        /// </summary>
        public CustomEventArgs Result { set; get; }
        /// <summary>
        /// webDAV-клиент
        /// </summary>
        public WDClient WDClient { set; get; }
        /// <summary>
        /// База данных
        /// </summary>
        public CustomDB CustomDb { set; get; }
        /// <summary>
        /// Текущий объект, выбранный в списке параметров
        /// </summary>
        public CustomEventArgs CurrentObject
        {
            set
            {
                this.currentObject = value;
            }
            get
            {
                if (this.CurrentObjects != null && this.CurrentObjects.Count > 0)
                    this.currentObject = this.CurrentObjects[0];
                return this.currentObject;
            }
        }

        #endregion

        #region Конструкторы/Деструкторы

        /// <summary>
        /// Конструктор по умолчанию, создающий CustomBase с настройками по умолчанию.
        /// </summary>
        public CustomBase()
        {
            // Initialize with default settings or state.
            // Assuming that CustomDB and WDClient can be initialized with default constructors as well.
            this.CustomDb = new CustomDB();
            this.WDClient = new WDClient();
            this.CurrentObjects = new List<CustomEventArgs>();
        }

        /// <summary>
        /// Конструктор, принимающий путь к файлу настроек и создающий CustomBase на основе этих настроек.
        /// </summary>
        /// <param name="settingsFilePath">Путь к файлу настроек.</param>
        public CustomBase(string settingsFilePath) : this(new SettingsNew(settingsFilePath))
        {

        }

        /// <summary>
        /// Конструктор, принимающий настройки и инициализирующий CustomBase на основе этих настроек.
        /// </summary>
        /// <param name="settings">Настройки для инициализации CustomBase.</param>
        public CustomBase(SettingsNew settings)
        {
            this.CustomDb = new CustomDB(settings);
            this.WDClient = new WDClient(settings);
            this.CurrentObjects = new List<CustomEventArgs>();
        }

        #endregion


        #region Методы
        public bool IsArgsCorrect(int expectedArgsCount)
        {
            return this.CurrentObjects == null ? expectedArgsCount == 0 : this.CurrentObjects.Count == expectedArgsCount;
        }

        public bool IsArgsCorrectException(int expectedArgsCount)
        {
            if (!IsArgsCorrect(expectedArgsCount))
                throw new Exception(String.Format("Ожидалось: {0} параметров, получено: {1} параметров", expectedArgsCount, this.CurrentObjects.Count));
            return true;
        }

        #region Работа с объектами
        /// <summary>
        /// Добавляет элемент в список текущих объектов.
        /// </summary>
        /// <param name="item">Элемент для добавления.</param>
        public void AddCurrentObject(CustomEventArgs item)
        {
            if (item != null)
            {
                this.CurrentObjects.Add(item);
            }
        }

        /// <summary>
        /// Очищает список текущих объектов.
        /// </summary>
        public void ClearCurrentObjects()
        {
            this.CurrentObjects.Clear();
        }

        /// <summary>
        /// Очищает список текущих объектов и добавляет новый элемент.
        /// </summary>
        /// <param name="item">Элемент для добавления после очистки списка.</param>
        public void AddWithClearCurrentObjects(CustomEventArgs item)
        {
            this.CurrentObjects.Clear();
            if (item != null)
            {
                this.CurrentObjects.Add(item);
            }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<object> ToList(List<IToObject> items)
        {
            return items.Select(item => item.ToObject()).ToList();
        }
        #endregion

        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion


    }
}
