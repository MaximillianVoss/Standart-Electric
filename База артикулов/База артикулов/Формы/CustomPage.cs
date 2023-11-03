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


    public class CustomPage : BaseWindow_WPF.BasePage
    {

        #region Поля
        public delegate void DataChangedEventHandler(object sender, CustomEventArgs e);
        public event DataChangedEventHandler DataChanged;
        #endregion

        #region Свойства
        /// <summary>
        /// Главный объект для доступа к БД облачному клиенту и общему функционалу
        /// </summary>
        public CustomBase CustomBase { set; get; }
        /// <summary>
        /// Именованная обертка для WebDAV клиента CustomBase
        /// </summary>
        public WDClient WDClient
        {
            get
            {
                return this.CustomBase.WDClient;
            }
        }
        /// <summary>
        /// Именованная обертка для базы данных DBSEEntities CustomBase
        /// </summary>
        public DBSEEntities DB
        {
            get
            {
                return this.CustomBase.CustomDb.DB;
            }
        }
        /// <summary>
        /// Объекты с которыми в данный момент взаимодействует окно, 
        /// обычно передаются ему в качестве параметров. 
        /// Обертка одноименного свойства вложенного объекта CustomBase
        /// </summary>
        public List<CustomEventArgs> СurrentObjects
        {
            set
            {
                this.CustomBase.СurrentObjects = value;
            }
            get
            {
                return this.CustomBase.СurrentObjects;
            }
        }
        #endregion

        #region Методы
        /// <summary>
        /// Вызываем при изменении данных внутри страницы
        /// </summary>
        /// <param name="data"></param>
        public void OnDataChanged(object data)
        {
            DataChanged?.Invoke(this, new CustomEventArgs(data));
        }

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

        #endregion

        #region Конструкторы/Деструкторы
        public CustomPage(SettingsNew settings, List<CustomEventArgs> currentObjects = null, EditModes mode = EditModes.Create)
        {
            this.CustomBase = new CustomBase(settings);
            this.CustomBase.Mode = mode;
            this.СurrentObjects = currentObjects;
        }
        public CustomPage() : this(null)
        {

        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion

    }
}
