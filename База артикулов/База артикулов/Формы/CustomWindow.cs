using BaseWindow_WPF;
using System;
using System.Collections.Generic;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы
{
    public abstract class CustomWindow : BaseWindow
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
        public WDClient WDClient => this.CustomBase.WDClient;
        /// <summary>
        /// Именованная обертка для базы данных DBSEEntities CustomBase
        /// </summary>
        public DBSEEntities DB => this.CustomBase.CustomDb.DB;
        /// <summary>
        /// Объекты с которыми в данный момент взаимодействует окно, 
        /// обычно передаются ему в качестве параметров. 
        /// Обертка одноименного свойства вложенного объекта CustomBase
        /// </summary>
        public List<CustomEventArgs> СurrentObjects
        {
            set => this.CustomBase.CurrentObjects = value;
            get => this.CustomBase.CurrentObjects;
        }
        /// <summary>
        /// Первый переданный объект в списке аргументов (если они не <see langword="null"/>)
        /// </summary>
        public CustomEventArgs CurrentObject { get => this.CustomBase.CurrentObject; set => this.CustomBase.CurrentObject = value; }

        #endregion

        #region Методы
        /// <summary>
        /// Закрывает окно
        /// </summary>
        /// <param name="dialogResult">Были внесены изменения в окне или нет</param>
        public void CloseWindow(bool? dialogResult = false)
        {
            this.DialogResult = dialogResult;
            this.Close();
        }
        /// <summary>
        /// Вызываем при изменении данных внутри страницы
        /// </summary>
        /// <param name="data"></param>
        public void OnDataChanged(object data)
        {
            DataChanged?.Invoke(this, new CustomEventArgs(data));
        }
        public abstract void UpdateFields(List<CustomEventArgs> args = null);
        public abstract void UpdateForm(List<CustomEventArgs> args = null);
        public abstract object HandleOk();
        public abstract object HandleCancel();
        public void ProcessOk()
        {
            try
            {
                this.CustomBase.Result = new CustomEventArgs(this.HandleOk());
                this.CloseWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        public void ProcessCancel()
        {
            try
            {
                this.CustomBase.Result = new CustomEventArgs(this.HandleCancel());
                this.CloseWindow(false);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        #endregion

        #region Конструкторы/Деструкторы

        /// <summary>
        /// Конструктор, принимающий экземпляр CustomBase и дополнительные параметры.
        /// </summary>
        /// <param name="title">Заголовок окна.</param>
        /// <param name="customBase">Экземпляр CustomBase, используемый для инициализации.</param>
        /// <param name="expectedArgsCount">Ожидаемое количество аргументов.</param>
        /// <param name="currentObjects">Текущие объекты для обработки.</param>
        /// <param name="mode">Режим редактирования. По умолчанию Create.</param>
        public CustomWindow(
            string title = "No title",
            CustomBase customBase = null,
            int expectedArgsCount = 0,
            List<CustomEventArgs> currentObjects = null,
            EditModes mode = EditModes.None)
        {
            this.Title = title;
            this.CustomBase = customBase ?? new CustomBase();
            if (this.CustomBase != null)
            {
                this.CustomBase.CustomDb.Update();
                //this.CustomBase.IsArgsCorrectException(expectedArgsCount);
                if (mode != EditModes.None)
                    this.CustomBase.Mode = mode;
                if (currentObjects != null)
                    this.CustomBase.CurrentObjects = currentObjects;
                if (this.CustomBase.CurrentObjects != null)
                {
                    this.UpdateForm(this.CustomBase.CurrentObjects);
                    this.UpdateFields(this.CustomBase.CurrentObjects);
                }
            }
        }

        /// <summary>
        /// Конструктор, принимающий настройки и создающий экземпляр CustomBase из этих настроек.
        /// </summary>
        /// <param name="title">Заголовок окна.</param>
        /// <param name="settings">Настройки для создания CustomBase.</param>
        /// <param name="currentObjects">Текущие объекты для обработки.</param>
        /// <param name="mode">Режим редактирования. По умолчанию Create.</param>
        public CustomWindow(
            string title,
            Settings settings,
            List<CustomEventArgs> currentObjects = null,
            EditModes mode = EditModes.Create) :
            this(title, new CustomBase(settings), 0, currentObjects, mode)
        {

        }

        #endregion


        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion


    }
}
