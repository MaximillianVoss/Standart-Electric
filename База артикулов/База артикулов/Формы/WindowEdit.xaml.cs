using BaseWindow_WPF.Classes;
using System;
using System.Windows;
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
        public Type ItemType { get; set; }
        public object CurrentItem { get; set; }
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

        public WindowEdit(string title, object item)
        {
            this.InitializeComponent();
            this.SetCenter();
            this.Title = title;
            this.CurrentItem = item;
            if (item != null)
                this.ItemType = item.GetType();
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
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
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        #endregion

    }
}