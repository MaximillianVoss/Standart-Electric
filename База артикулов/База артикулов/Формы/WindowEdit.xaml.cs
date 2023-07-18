using BaseWindow_WPF.Classes;
using System;
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

        #endregion

        #region Свойства
        public Type ItemType { get; set; }
        public object CurrentItem { get; set; }
        /// <summary>
        /// Режим окна: создание/редактирование/удаление
        /// </summary>
        public WindowEditModes Mode { set; get; }
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

        public WindowEdit(string title, object item, WindowEditModes mode = WindowEditModes.Edit)
        {
            this.InitializeComponent();
            this.SetCenter();
            this.Title = title;
            this.CurrentItem = item;
            if (item != null)
                this.ItemType = item.GetType();
            this.Mode = mode;
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Mode == WindowEditModes.Create)
                {
                    if (this.CurrentItem == null)
                        throw new Exception("Не указат тип создаваемого объекта! В форму надо передать объект с конструктором по умолчанию!");
                    if (this.CurrentItem.GetType() == typeof(Classes) || this.CurrentItem.GetType().BaseType == typeof(Classes))
                    {
                        this.fMain.Content = new PageEditClass();
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
                                this.fMain.Content = new PageEditClass(objValue);
                            }
                            if (objBaseType == typeof(Groups))
                            {
                                this.fMain.Content = new PageEditGroup(objValue);
                            }
                            if (objBaseType == typeof(SubGroups))
                            {
                                this.fMain.Content = new PageEditSubGroup(objValue);
                            }

                        }
                    }
                    else
                    {
                        object objValue = this.CurrentItem;
                        Type objBaseType = objValue.GetType().BaseType;
                        if (objBaseType == typeof(Products) ||
                            objBaseType == typeof(ProductsView) ||
                            objValue.GetType() == typeof(ProductsView))
                        {
                            var windowsEdit = new ProductWindow(objValue);
                            //this.fMain.Content = new PageEditProduct(objValue);
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