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
using База_артикулов.Properties;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы
{
    public class CustomWindow : BaseWindow
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
        #endregion

        #region Конструкторы/Деструкторы
        public CustomWindow(SettingsNew settings, List<Object> currentObjects = null)
        {
            this.CustomBase = new CustomBase(settings);
            this.СurrentObjects = currentObjects;
        }
        public CustomWindow() : this(null)
        {

        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion


    }
}
