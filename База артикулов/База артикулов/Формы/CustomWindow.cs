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

        #endregion

        #region Свойства
        /// <summary>
        /// Главный объект для доступа к БД облачному клиенту и общему функционалу
        /// </summary>
        public CustomBase CustomBase { set; get; }
        #endregion

        #region Методы

        #endregion

        #region Конструкторы/Деструкторы
        public CustomWindow(SettingsNew settings)
        {
            this.CustomBase = new CustomBase(settings);
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
