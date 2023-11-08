using BaseWindow_WPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #endregion

        #region Методы
        private void SetContent(string title, object content)
        {
            if (this.fMain != null)
            {
                this.Title = title;
                this.fMain.Content = content;
            }
        }
        public override void UpdateFields(List<CustomEventArgs> args = null)
        {
            //throw new NotImplementedException();
        }
        public override void UpdateForm(List<CustomEventArgs> args = null)
        {
            switch (this.CustomBase.Mode)
            {
                case EditModes.Create:
                    HandleCreateMode(args);
                    break;

                case EditModes.Edit:
                    HandleEditMode(args);
                    break;
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
        private void HandleCreateMode(List<CustomEventArgs> args)
        {
            if (this.CustomBase == null || this.CustomBase.CurrentObjects == null)
            {
                throw new Exception("Не указан тип создаваемого объекта! В форму надо передать объект с конструктором по умолчанию!");
            }
            if (this.CustomBase.CurrentObjects.Count == 1)
            {
                if (this.CurrentObject.DataType == typeof(TreeViewItemCustom))
                {
                    var treeViewSelectedObject = ((TreeViewItemCustom)this.CurrentObject.Data).Value;
                    if (treeViewSelectedObject.IsTypeOrBaseEqual(typeof(Classes)))
                    {
                        this.SetContent("Создание класса", new PageEditClass(this.CustomBase));
                    }
                    if (treeViewSelectedObject.IsTypeOrBaseEqual(typeof(Groups)))
                    {
                        this.SetContent("Создание группы", new PageEditGroup(this.CustomBase));
                    }

                }


                //if (this.CurrentObject.IsTypeOrBaseEqual(typeof(SubGroups)))
                //{
                //    this.SetContent("Создание подгруппы", new PageEditSubGroup(this.CustomBase, 1));
                //}

                //else if (currentItem.IsTypeOrBaseEqual(typeof(UnitsProducts)))
                //{
                //    this.SetContent("Создание единицы продукции", new PageEditUnit(this.CustomBase.CurrentObjects, this.CustomBase.Mode));
                //}
                //else if (currentItem.IsTypeOrBaseEqual(typeof(ResourcesViewProducts)))
                //{
                //    this.SetContent("Создание ресурса продукции", new PageEditResource(this.CustomBase.CurrentObjects, this.CustomBase.Mode));
                //}
                //else if (currentItem.IsTypeOrBaseEqual(typeof(ProductsView)))
                //{
                //    this.SetContent("Создание продукта", new PageEditProduct(this.CustomBase.CurrentObjects, this.CustomBase.Mode));
                //}
                //else if (currentItem.IsTypeOrBaseEqual(typeof(VendorCodes)))
                //{
                //    throw new Exception("Перепроверить этот код!");
                //    VendorCodes newVendorCode = new VendorCodes();
                //    newVendorCode.Manufacturers = this.DB.Manufacturers.FirstOrDefault(x => x.id > 0);
                //    var page = new PageEditVendorCode(newVendorCode);
                //    page.DataChanged += PageDataChanged;
                //    this.SetContent("Создание кода поставщика", page);
                //}

            }

        }
        private void HandleEditMode(List<CustomEventArgs> args)
        {
            if (this.CustomBase.CurrentObject == null)
                throw new Exception("Не выбран элемент для редактирования!");

            //if (this.ItemType == typeof(TreeViewItemCustom))
            //{
            //    object objValue = ((TreeViewItemCustom)this.CurrentObject).Value;
            //    if (objValue != null)
            //    {
            //        Type objBaseType = objValue.GetType().BaseType;
            //        if (objBaseType == typeof(Classes))
            //        {
            //            this.Title = "Редактирование класса";
            //            this.fMain.Content = new PageEditClass(objValue);
            //        }
            //        if (objBaseType == typeof(Groups))
            //        {
            //            this.Title = "Редактирование группы";
            //            this.fMain.Content = new PageEditGroup(objValue);
            //        }
            //        if (objBaseType == typeof(SubGroups))
            //        {
            //            this.Title = "Редактирование подгруппы";
            //            this.fMain.Content = new PageEditSubGroup(objValue);
            //        }
            //    }
            //}
            //else
            //{
            //    object objValue = this.CurrentObject;
            //    Type objBaseType = objValue.GetType().BaseType;
            //    if (objBaseType.BaseType == typeof(ProductsView) || objValue.GetType() == typeof(ProductsView))
            //    {
            //        this.Title = "Редактирование продукта";
            //        this.fMain.Content = new PageEditProduct(objValue);
            //    }
            //    if (objBaseType == typeof(UnitsProducts))
            //    {
            //        this.Title = "Редактирование единицы продукции";
            //        this.fMain.Content = new PageEditUnit(objValue);
            //    }
            //    if (this.CustomBase.IsTypeEqual(typeof(ResourcesViewProducts), this.CurrentObject))
            //    {
            //        this.Title = "Редактирование ресурса продукции";
            //        this.fMain.Content = new PageEditResource(objValue, this.Mode);
            //    }
            //    else if (this.CustomBase.IsTypeEqual(typeof(VendorCodes), this.CurrentObject))
            //    {
            //        this.Title = "Редактирование кода поставщика";
            //        var page = new PageEditVendorCode(this.CurrentObject);
            //        page.DataChanged += PageDataChanged;
            //        this.fMain.Content = page;
            //    }
            //}

        }
        #endregion

        #region Конструкторы/Деструкторы

        /// <summary>
        /// Конструктор по умолчанию, использующий стандартные параметры базового класса.
        /// </summary>
        /// <param name="title">Заголовок окна. По умолчанию "WindowEdit".</param>
        public WindowEdit(string title = "WindowEdit") : base(title: title)
        {
            InitializeWindow(title);
        }

        /// <summary>
        /// Конструктор, принимающий экземпляр CustomBase.
        /// </summary>
        /// <param name="title">Заголовок окна.</param>
        /// <param name="customBase">Экземпляр CustomBase, используемый для инициализации.</param>
        public WindowEdit(string title, CustomBase customBase) : base(title: title, customBase)
        {
            InitializeWindow(title);
        }

        /// <summary>
        /// Конструктор, принимающий путь к файлу настроек, создает экземпляр CustomBase из настроек.
        /// </summary>
        /// <param name="title">Заголовок окна.</param>
        /// <param name="settingsPath">Путь к файлу настроек.</param>
        /// <param name="mode">Режим редактирования. По умолчанию Create.</param>
        public WindowEdit(string title, string settingsPath, EditModes mode = EditModes.Create) :
            base(title: title, new CustomBase(new SettingsNew(settingsPath)), mode: mode)
        {
            InitializeWindow(title);
        }

        /// <summary>
        /// Вспомогательный метод для инициализации окна WindowEdit, включая заголовок.
        /// </summary>
        /// <param name="title">Заголовок окна, который будет установлен.</param>
        private void InitializeWindow(string title)
        {
            this.InitializeComponent();
            this.SetCenter(); // Предполагается, что SetCenter - это метод, который центрирует окно.
            this.Title = title; // Устанавливаем заголовок окна.
                                // Здесь может быть выполнена остальная инициализация, например, установка размеров и т.д.
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


        #endregion

    }
}