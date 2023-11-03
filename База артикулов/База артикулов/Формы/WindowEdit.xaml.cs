using BaseWindow_WPF.Classes;
using System;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using База_артикулов.Модели;
using База_артикулов.Формы.Страницы.Редактирование;
using System.Collections.Generic;
using База_артикулов.Классы;
using System.Windows.Controls;

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

        #endregion

        #region Конструкторы/Деструкторы
        public WindowEdit() : this("WindowEdit", null)
        {

        }

        public WindowEdit(object item) : this("WindowEdit", item)
        {

        }

        public WindowEdit(string title, List<Object> currentObjects, EditModes mode = EditModes.Edit, int Width = 600, int Height = 800)
        {
            this.InitializeComponent();
            this.SetCenter();
            this.Title = title;
            this.Width = Width;
            this.Height = Height;
            this.CustomBase.СurrentObjects = currentObjects;
            this.CustomBase.Mode = mode;
        }

        public WindowEdit(string title, object item, EditModes mode = EditModes.Edit, int Width = 600, int Height = 800) :
            this(title, new List<Object> { item }, mode, Width, Height)
        {
            this.InitializeComponent();
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void PageDataChanged(object sender, CustomEventArgs e)
        {
            this.CustomBase.Result = e;
        }

        private void SetContent(string title, object content)
        {
            if (this.fMain != null)
            {
                this.Title = title;
                this.fMain.Content = content;
            }
        }
        private void HandleCreateMode()
        {
            if (this.CustomBase.СurrentObjects.Count == 1)
            {
                object currentItem = this.CustomBase.СurrentObjects[0];
                if (currentItem == null)
                {
                    throw new Exception("Не указан тип создаваемого объекта! В форму надо передать объект с конструктором по умолчанию!");
                }
                if (currentItem.IsTypeOrBaseEqual(typeof(Classes)))
                {
                    this.SetContent("Создание класса", new PageEditClass(this.CustomBase.СurrentObjects, this.CustomBase.Mode));
                }
                else if (currentItem.IsTypeOrBaseEqual(typeof(UnitsProducts)))
                {
                    this.SetContent("Создание единицы продукции", new PageEditUnit(this.CustomBase.СurrentObjects, this.CustomBase.Mode));
                }
                else if (currentItem.IsTypeOrBaseEqual(typeof(ResourcesViewProducts)))
                {
                    this.SetContent("Создание ресурса продукции", new PageEditResource(this.CustomBase.СurrentObjects, this.CustomBase.Mode));
                }
                else if (currentItem.IsTypeOrBaseEqual(typeof(ProductsView)))
                {
                    this.SetContent("Создание продукта", new PageEditProduct(this.CustomBase.СurrentObjects, this.CustomBase.Mode));
                }
                else if (currentItem.IsTypeOrBaseEqual(typeof(VendorCodes)))
                {
                    throw new Exception("Перепроверить этот код!");
                    VendorCodes newVendorCode = new VendorCodes();
                    newVendorCode.Manufacturers = this.DB.Manufacturers.FirstOrDefault(x => x.id > 0);
                    var page = new PageEditVendorCode(newVendorCode);
                    page.DataChanged += PageDataChanged;
                    this.SetContent("Создание кода поставщика", page);
                }

            }

        }
        private void HandleEditMode()
        {
            if (this.Mode == EditModes.Edit)
            {
                if (this.CurrentItem == null)
                    throw new Exception("Не выбран элемент для редактирования!");

                if (this.ItemType == typeof(TreeViewItemCustom))
                {
                    object objValue = ((TreeViewItemCustom)this.CurrentItem).Value;
                    if (objValue != null)
                    {
                        Type objBaseType = objValue.GetType().BaseType;
                        if (objBaseType == typeof(Classes))
                        {
                            this.Title = "Редактирование класса";
                            this.fMain.Content = new PageEditClass(objValue);
                        }
                        if (objBaseType == typeof(Groups))
                        {
                            this.Title = "Редактирование группы";
                            this.fMain.Content = new PageEditGroup(objValue);
                        }
                        if (objBaseType == typeof(SubGroups))
                        {
                            this.Title = "Редактирование подгруппы";
                            this.fMain.Content = new PageEditSubGroup(objValue);
                        }
                    }
                }
                else
                {
                    object objValue = this.CurrentItem;
                    Type objBaseType = objValue.GetType().BaseType;
                    if (objBaseType.BaseType == typeof(ProductsView) || objValue.GetType() == typeof(ProductsView))
                    {
                        this.Title = "Редактирование продукта";
                        this.fMain.Content = new PageEditProduct(objValue);
                    }
                    if (objBaseType == typeof(UnitsProducts))
                    {
                        this.Title = "Редактирование единицы продукции";
                        this.fMain.Content = new PageEditUnit(objValue);
                    }
                    if (this.CustomBase.IsTypeEqual(typeof(ResourcesViewProducts), this.CurrentItem))
                    {
                        this.Title = "Редактирование ресурса продукции";
                        this.fMain.Content = new PageEditResource(objValue, this.Mode);
                    }
                    else if (this.CustomBase.IsTypeEqual(typeof(VendorCodes), this.CurrentItem))
                    {
                        this.Title = "Редактирование кода поставщика";
                        var page = new PageEditVendorCode(this.CurrentItem);
                        page.DataChanged += PageDataChanged;
                        this.fMain.Content = page;
                    }
                }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (this.CustomBase.Mode)
                {
                    case EditModes.Create:
                        HandleCreateMode();
                        break;

                    case EditModes.Edit:
                        HandleEditMode();
                        break;
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        #endregion

    }
}