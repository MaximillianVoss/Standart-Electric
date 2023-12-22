using BaseWindow_WPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
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
        CustomBase CustomBase
        {
            set; get;
        }
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

        #region Тесты
        private void TestCreateGroup()
        {
            this.CustomBase = new CustomBase(new Settings("settings.json"));
            this.CustomBase.AddCurrentObject(new CustomEventArgs(new TreeViewItemCustom(1, "Test", new Groups())));
            this.CustomBase.Mode = EditModes.Create;
            var windowEdit = new WindowEdit(
                Common.Strings.Titles.Windows.add,
                this.CustomBase,
                Common.WindowSizes.SmallH320W400.Width,
                Common.WindowSizes.SmallH320W400.Height
                );
            windowEdit.ShowDialog();
        }
        private void TestUpdateProduct()
        {
            this.CustomBase = new CustomBase(new Settings("settings.json"));
            this.CustomBase.AddCurrentObject(this.DB.Products.FirstOrDefault());
            this.CustomBase.Mode = EditModes.Update;
            var windowEdit = new WindowEdit(
                Common.Strings.Titles.Windows.add,
                this.CustomBase,
                Common.WindowSizes.MediumH600W800.Width,
                Common.WindowSizes.MediumH600W800.Height
                );
            windowEdit.ShowDialog();
        }
        #endregion

        #region Обработчики событий
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            try
            {
                this.TestUpdateProduct();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }

        }
        #endregion

    }
}
