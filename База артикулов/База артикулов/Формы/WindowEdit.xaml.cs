using System;
using System.Collections.Generic;
using System.Windows;
using База_артикулов.Классы;
using База_артикулов.Модели;
using База_артикулов.Формы.Страницы.Редактирование;

namespace База_артикулов.Формы
{

    /// <summary>
    /// Логика взаимодействия для WindowEdit.xaml
    /// </summary>
    public partial class WindowEdit : CustomWindow
    {


        #region Поля

        #endregion

        #region Свойства
        public int ContentWidth
        {
            set; get;
        }
        public int ContentHeight
        {
            set; get;
        }
        #endregion

        #region Методы
        private void SetContentSize(int width, int height)
        {
            this.Topmost = true; // Установка окна поверх всех окон
            this.ContentWidth = width;
            this.ContentHeight = height;
            this.MinHeight = height;
            this.MinWidth = width;
        }
        private void SetContent(string title, object content)
        {
            this.Title = title;
            this.SetContent(content);
        }
        private void SetContent(object content)
        {
            if (this.fMain != null)
            {
                this.fMain.Content = content;
            }
        }
        public override void UpdateFields(List<CustomEventArgs> args = null)
        {
            this.InitializeComponent();
        }
        public override void UpdateForm(List<CustomEventArgs> args = null)
        {
            try
            {
                this.Title = this.CustomBase.GetTitle(this.CustomBase.Mode, this.CustomBase.UnpackCurrentObject(this.CurrentObject));
                switch (this.CustomBase.Mode)
                {
                    case EditModes.Create:
                        this.HandleCreateMode(args);
                        break;
                    case EditModes.Update:
                        this.HandleEditMode(args);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        public override object HandleOk()
        {
            throw new NotImplementedException();
        }
        public override object HandleCancel()
        {
            throw new NotImplementedException();
        }
        private void HandleContent(List<CustomEventArgs> args)
        {
            if (this.CustomBase == null || this.CustomBase.CurrentObjects == null)
                throw new Exception(Common.Strings.Errors.incorrectUpdateElement);
            if (this.CustomBase.CurrentObjects.Count >= 1)
            {
                object argument = this.CustomBase.UnpackCurrentObject(this.CurrentObject);
                if (argument != null)
                {
                    if (argument.ValidateTypeOrBaseType<Classes>())
                        this.SetContent(new PageEditClass(this.CustomBase, this.ContentWidth, this.ContentHeight));
                    if (argument.ValidateTypeOrBaseType<Groups>())
                        this.SetContent(new PageEditGroup(this.CustomBase, this.ContentWidth, this.ContentHeight));
                    if (argument.ValidateTypeOrBaseType<SubGroups>())
                        this.SetContent(new PageEditSubGroup(this.CustomBase, this.ContentWidth, this.ContentHeight));
                    if (argument.ValidateTypeOrBaseType<Products>())
                        this.SetContent(new PageEditProduct(this.CustomBase, this.ContentWidth, this.ContentHeight));
                    if (argument.ValidateTypeOrBaseType<ProductsVendorCodes>())
                        this.SetContent(new PageEditVendorCode(this.CustomBase, this.ContentWidth, this.ContentHeight));
                    if (argument.ValidateTypeOrBaseType<UnitsProducts>())
                        this.SetContent(new PageEditUnit(this.CustomBase, this.ContentWidth, this.ContentHeight));
                    if (argument.ValidateTypeOrBaseType<ResourcesViewProducts>())
                        this.SetContent(new PageEditResource(this.CustomBase, this.ContentWidth, this.ContentHeight));

                }
            }
        }
        private void HandleCreateMode(List<CustomEventArgs> args)
        {
            this.HandleContent(args);
        }
        private void HandleEditMode(List<CustomEventArgs> args)
        {
            this.HandleContent(args);
        }
        #endregion

        #region Конструкторы/Деструкторы

        /// <summary>
        /// Конструктор по умолчанию, использующий стандартные параметры базового класса.
        /// </summary>
        /// <param name="title">Заголовок окна. По умолчанию "WindowEdit".</param>
        public WindowEdit(string title = "WindowEdit") : base(title: title)
        {

        }

        /// <summary>
        /// Конструктор, принимающий экземпляр CustomBase.
        /// </summary>
        /// <param name="title">Заголовок окна.</param>
        /// <param name="customBase">Экземпляр CustomBase, используемый для инициализации.</param>
        public WindowEdit(
            string title,
            CustomBase customBase,
            int width = Common.WindowSizes.MediumH600W800.Width,
            int height = Common.WindowSizes.MediumH600W800.Height
            ) : base(title: title, customBase)
        {
            this.SetContentSize(width, height);
        }

        public WindowEdit(
            CustomBase customBase,
            int width = Common.WindowSizes.MediumH600W800.Width,
            int height = Common.WindowSizes.MediumH600W800.Height
            ) : base(title: string.Empty, customBase)
        {
            this.SetContentSize(width, height);
        }


        /// <summary>
        /// Конструктор, принимающий путь к файлу настроек, создает экземпляр CustomBase из настроек.
        /// </summary>
        /// <param name="title">Заголовок окна.</param>
        /// <param name="settingsPath">Путь к файлу настроек.</param>
        /// <param name="mode">Режим редактирования. По умолчанию Create.</param>
        public WindowEdit(string title, string settingsPath, EditModes mode = EditModes.Create) :
            base(title: title, new CustomBase(new Settings(settingsPath)), mode: mode)
        {

        }


        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.UpdateForm(this.CustomBase.CurrentObjects);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void PageDataChanged(object sender, CustomEventArgs e)
        {
            this.CustomBase.Result = e;
        }
        private void CustomWindow_ContentRendered(object sender, EventArgs e)
        {
            this.SetCenter(sender);
        }
        private void CustomWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
        private void CustomWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Действуем по принципу стека - в конце списка последний обрабатываемый элемент
            this.CustomBase.RemoveLastCurrentObject();
        }
        #endregion


    }
}