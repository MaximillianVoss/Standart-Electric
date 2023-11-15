using BaseWindow_WPF.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using База_артикулов.Классы;
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
        CustomBase CustomBase { set; get; }
        #endregion

        #region Методы

        #endregion

        #region Конструкторы/Деструкторы
        public TestWindow() : base("TestWindow")
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.InitializeComponent();
        }

        public override object HandleCancel()
        {
            throw new NotImplementedException();
        }

        public override object HandleOk()
        {
            throw new NotImplementedException();
        }

        public override void UpdateFields(List<CustomEventArgs> args = null)
        {
            //throw new NotImplementedException();
        }

        public override void UpdateForm(List<CustomEventArgs> args = null)
        {
            //throw new NotImplementedException();
        }


        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            try
            {
                this.CustomBase = new CustomBase(new Settings("settings.json"));
                this.CustomBase.AddWithClearCurrentObjects(new CustomEventArgs(new TreeViewItemCustom(1, "Test", new Groups())));
                this.CustomBase.Mode = EditModes.Create;
                var windowEdit = new WindowEdit(
                    Common.Strings.Titles.Windows.add,
                    this.CustomBase,
                    Common.WindowSizes.SmallH320W400.Width,
                    Common.WindowSizes.SmallH320W400.Height
                    );
                // windowEdit.Owner = this;
                //windowEdit.SetCenter();
                windowEdit.ShowDialog();


                //ProductUnitsWindow productUnitsWindow = new ProductUnitsWindow(1);
                //productUnitsWindow.Show();
                //FilesWindow filesWindow = new FilesWindow();
                //filesWindow.Show();
                //ProductWindow productWindow = new ProductWindow(typeof(Products), this.CustomDb.Products.First().id);
                //productWindow.ShowDialog();

                //WindowEdit windowEdit = new WindowEdit(this.CustomDb.ProductsView.FirstOrDefault(x => x.ID_продукта == 1));
                //WindowEdit windowEdit = new WindowEdit(this.CustomDb.UnitsProducts.FirstOrDefault(x => x.id == 1));

                //var product = CreateEmptyProduct("TEST PRODUCT", this.DB.SubGroups.FirstOrDefault(x => x.id > 0));
                //WindowEdit windowEdit = new WindowEdit(this.DB.ProductsView.FirstOrDefault(x => x.ID_продукта == product.id));
                //windowEdit.ShowDialog();

                //this.CustomDb.Products.Remove(product);
                //this.CustomDb.SaveChanges();

                //var vendorCode = new VendorCodes();
                //WindowEdit windowEdit = new WindowEdit("Create vendorCode", vendorCode, EditModes.Create);
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
