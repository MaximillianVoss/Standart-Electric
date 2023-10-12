using System;
using System.Linq;
using System.Windows;
using База_артикулов.Модели;

namespace База_артикулов.Формы
{
    /// <summary>
    /// Тестовое окно для тестирования функуионала других окон
    /// </summary>
    public partial class TestWindow : CustomWindow
    {


        #region Поля

        #endregion

        #region Свойства

        #endregion

        #region Методы

        #endregion

        #region Конструкторы/Деструкторы
        public TestWindow()
        {
            this.InitializeComponent();
        }


        #endregion

        #region Операторы

        #endregion



        #region Обработчики событий
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //ProductUnitsWindow productUnitsWindow = new ProductUnitsWindow(1);
                //productUnitsWindow.Show();
                //FilesWindow filesWindow = new FilesWindow();
                //filesWindow.Show();
                //ProductWindow productWindow = new ProductWindow(typeof(Products), this.DB.Products.First().id);
                //productWindow.ShowDialog();

                //WindowEdit windowEdit = new WindowEdit(this.DB.ProductsView.FirstOrDefault(x => x.ID_продукта == 1));
                //WindowEdit windowEdit = new WindowEdit(this.DB.UnitsProducts.FirstOrDefault(x => x.id == 1));

                var product = CreateEmptyProduct("TEST PRODUCT", this.DB.SubGroups.FirstOrDefault(x => x.id > 0));
                WindowEdit windowEdit = new WindowEdit(this.DB.ProductsView.FirstOrDefault(x => x.ID_продукта == product.id));
                windowEdit.ShowDialog();
                //this.DB.Products.Remove(product);
                //this.DB.SaveChanges();

                //var vendorCode = new VendorCodes();
                //WindowEdit windowEdit = new WindowEdit("Create vendorCode", vendorCode, WindowEditModes.Create);
                //windowEdit.ShowDialog();

            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }

        }
        #endregion

    }
}
