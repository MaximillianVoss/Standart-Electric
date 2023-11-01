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
        #endregion

        #region Свойства
        /// <summary>
        /// Главный объект для доступа к БД облачному клиенту и общему функционалу
        /// </summary>
        public CustomBase CustomBase { set; get; }
        /// <summary>
        /// webDAV-клиент, ссылается на одноименное поле CustomBase
        /// </summary>
        public WDClient WDClient
        {
            get => this.CustomBase.WDClient;
            set => this.CustomBase.WDClient = value;
        }
        /// <summary>
        /// База данных, ссылается на одноименное поле CustomBase
        /// </summary>
        public CustomDB DB
        {
            get => this.CustomBase.DB;
            set => this.CustomBase.DB = value;
        }
        #endregion

        #region Методы
        /// <summary>
        /// Вызываем при изменении данных внутри страницы
        /// </summary>
        /// <param name="data"></param>
        public void OnDataChanged(object data)
        {
            DataChanged?.Invoke(this, new MyEventArgs { Data = data });
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
        public CustomPage(SettingsNew settings)
        {
            this.CustomBase = new CustomBase(settings);
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
