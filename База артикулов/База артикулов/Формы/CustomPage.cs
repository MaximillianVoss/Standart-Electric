using System;
using System.Collections.Generic;
using System.Windows;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы
{


    public abstract partial class CustomPage : BaseWindow_WPF.BasePage
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
        /// Устанавливает размер окна.
        /// </summary>
        /// <param name="width">Ширина окна. Должна быть больше или равна 0.</param>
        /// <param name="height">Высота окна. Должна быть больше или равна 0.</param>
        /// <exception cref="ArgumentOutOfRangeException">Генерируется, если ширина или высота меньше 0.</exception>
        public void SetSize(int width, int height)
        {
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Ширина окна не может быть меньше 0.");
            }
            if (height < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), "Высота окна не может быть меньше 0.");
            }

            this.MinWidth = width;
            this.MinHeight = height;
        }

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
        public abstract void UpdateFields(List<CustomEventArgs> args);
        public abstract void UpdateForm(List<CustomEventArgs> args);
        public abstract object HandleOk(List<CustomEventArgs> args);
        public abstract object HandleCancel(List<CustomEventArgs> args);
        public void ProcessOk()
        {
            try
            {
                if (this.CustomBase.Result == null)
                    this.CustomBase.Result = new CustomEventArgs();
                this.CustomBase.Result = new CustomEventArgs(this.HandleOk(this.CustomBase.CurrentObjects));
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
                if (this.CustomBase.Result == null)
                    this.CustomBase.Result = new CustomEventArgs();
                this.CustomBase.Result = new CustomEventArgs(this.HandleCancel(this.CustomBase.CurrentObjects));
                this.CloseWindow(false);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        #endregion

        #region Конструкторы/Деструкторы
        public CustomPage(Settings settings, List<CustomEventArgs> currentObjects = null, EditModes mode = EditModes.Create) :
            this(new CustomBase(settings), currentObjects, mode)
        {

        }
        public CustomPage(
            CustomBase customBase = null,
            List<CustomEventArgs> currentObjects = null,
            EditModes mode = EditModes.None)
        {
            this.CustomBase = customBase ?? new CustomBase();
            if (this.CustomBase != null)
            {
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
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion

    }
}
