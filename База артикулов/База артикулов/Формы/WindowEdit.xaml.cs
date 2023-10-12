using BaseWindow_WPF.Classes;
using System;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using База_артикулов.Модели;
using База_артикулов.Формы.Страницы.Редактирование;

namespace База_артикулов.Формы
{
    /// <summary>
    /// Возможные режимы окна
    /// </summary>
    public enum WindowEditModes
    {
        Create,
        Edit,
        Delete
    }
    /// <summary>
    /// Логика взаимодействия для WindowEdit.xaml
    /// </summary>
    public partial class WindowEdit : CustomWindow
    {


        #region Поля
        private object _currentItem;


        #endregion

        #region Свойства
        public Type ItemType { get; set; }
        public object CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (_currentItem != null && _currentItem is EntityObject)
                {
                    var entity = _currentItem as EntityObject;
                    if (entity.EntityState != EntityState.Detached)
                    {
                        this.DB.Entry(this._currentItem).State = EntityState.Detached;
                    }
                }

                _currentItem = value;
            }
        }
        /// <summary>
        /// Режим окна: создание/редактирование/удаление
        /// </summary>
        public WindowEditModes Mode { set; get; }
        #endregion

        #region Методы
        private Products CreateEmptyProduct(string title = "Новый продукт", SubGroups subGroups = null)
        {
            // Создание нового дескриптора
            var descriptor = new Descriptors
            {
                title = title
            };

            // Сохранение дескриптора в БД
            this.DB.Descriptors.Add(descriptor);
            this.DB.SaveChanges();

            // Создание нового продукта со значениями по умолчанию
            var product = new Products
            {
                Descriptors = descriptor,
                Norms = this.DB.Norms.FirstOrDefault(x => x.id > 0),
                SubGroups = subGroups == null ? this.DB.SubGroups.FirstOrDefault(x => x.id > 0) : subGroups,
                Covers = this.DB.Covers.FirstOrDefault(x => x.id > 0),
                Materials = this.DB.Materials.FirstOrDefault(x => x.id > 0),
                Perforations = this.DB.Perforations.FirstOrDefault(x => x.id > 0),
                Packages = this.DB.Packages.FirstOrDefault(x => x.id > 0),
                isInStock = false
            };

            // Сохранение продукта в БД
            this.DB.Products.Add(product);
            this.DB.SaveChanges();
            return product;
        }
        #endregion

        #region Конструкторы/Деструкторы
        public WindowEdit() : this("WindowEdit", null)
        {

        }

        public WindowEdit(object item) : this("WindowEdit", item)
        {

        }

        public WindowEdit(string title, object item, WindowEditModes mode = WindowEditModes.Edit, int Width = 600, int Height = 800)
        {
            this.InitializeComponent();
            this.Width = Width;
            this.Height = Height;
            this.Title = title;
            this.SetCenter();
            this.CurrentItem = item;
            if (item != null)
                this.ItemType = item.GetType();
            this.Mode = mode;
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void PageDataChanged(object sender, MyEventArgs e)
        {
            this.CurrentItem = e.Data;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.InitDB();
                if (this.Mode == WindowEditModes.Create)
                {
                    if (this.CurrentItem == null)
                        throw new Exception("Не указан тип создаваемого объекта! В форму надо передать объект с конструктором по умолчанию!");

                    if (IsTypeEqual(typeof(Classes), this.CurrentItem))
                    {
                        this.Title = "Создание класса";
                        this.fMain.Content = new PageEditClass();
                    }
                    else if (IsTypeEqual(typeof(UnitsProducts), this.CurrentItem))
                    {
                        this.Title = "Создание единицы продукции";
                        this.fMain.Content = new PageEditUnit(this.CurrentItem);
                    }
                    else if (IsTypeEqual(typeof(ResourcesViewProducts), this.CurrentItem))
                    {
                        this.Title = "Создание ресурса продукции";
                        this.fMain.Content = new PageEditResource(this.CurrentItem, this.Mode);
                    }
                    else if (IsTypeEqual(typeof(ProductsView), this.CurrentItem))
                    {
                        this.Title = "Создание продукта";
                        this.fMain.Content = new PageEditProduct(this.CreateEmptyProduct());
                    }
                    else if (IsTypeEqual(typeof(VendorCodes), this.CurrentItem))
                    {
                        this.Title = "Создание кода поставщика";
                        VendorCodes newVendorCode = new VendorCodes();
                        newVendorCode.Manufacturers = this.DB.Manufacturers.FirstOrDefault(x => x.id > 0);
                        var page = new PageEditVendorCode(newVendorCode);
                        page.DataChanged += PageDataChanged;
                        this.fMain.Content = page;
                    }
                }
                if (this.Mode == WindowEditModes.Edit)
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
                        if (IsTypeEqual(typeof(ResourcesViewProducts), this.CurrentItem))
                        {
                            this.Title = "Редактирование ресурса продукции";
                            this.fMain.Content = new PageEditResource(objValue, this.Mode);
                        }
                        else if (IsTypeEqual(typeof(VendorCodes), this.CurrentItem))
                        {
                            this.Title = "Редактирование кода поставщика";
                            var page = new PageEditVendorCode(this.CurrentItem);
                            page.DataChanged += PageDataChanged;
                            this.fMain.Content = page;
                        }
                    }
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