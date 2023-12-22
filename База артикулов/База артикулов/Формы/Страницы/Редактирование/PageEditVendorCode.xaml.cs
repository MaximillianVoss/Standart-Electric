using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы.Страницы.Редактирование
{
    /// <summary>
    /// Логика взаимодействия для PageEditVendorCode.xaml
    /// </summary>
    public partial class PageEditVendorCode : CustomPage
    {


        #region Поля

        #endregion

        #region Свойства

        #endregion

        #region Методы

        public override void UpdateFields(List<CustomEventArgs> args)
        {
            ProductsVendorCodes productVendorCode = this.CustomBase.UnpackCurrentObject<ProductsVendorCodes>(this.CurrentObject);
            if (productVendorCode != null)
            {
                if (this.CustomBase.Mode == EditModes.Update)
                {
                    VendorCodes vendorCode = this.CustomBase.CustomDb.DB.VendorCodes.FirstOrDefault(x => x.id == productVendorCode.idCode);
                    if (vendorCode == null)
                    {
                        throw new Exception(Common.Strings.Errors.emptyObject);
                    }
                    this.chbIsActual.IsChecked = vendorCode.isActual;
                    this.chbIsPublic.IsChecked = vendorCode.isPublic;
                    this.chbIsSale.IsChecked = vendorCode.isSale;
                    this.txbCode.Text = vendorCode.Descriptors == null ? String.Empty : vendorCode.Descriptors.title;
                    this.txbAccountantCode.Text = vendorCode.codeAccountant;
                    if (vendorCode.Manufacturers != null)
                    {
                        this.cmbManufacturer.Select(vendorCode.Manufacturers.id);
                    }
                    this.cmbManufacturer.Update(this.CustomBase.ToList<Manufacturers>(this.DB.Manufacturers), vendorCode.idManufacturer);
                }
                if (this.CustomBase.Mode == EditModes.Create)
                {
                    var manufacturer = this.DB.Manufacturers.FirstOrDefault();
                    if (manufacturer != null)
                    {
                        this.cmbManufacturer.Select(manufacturer.id);
                    }
                }
            }
        }
        public override void UpdateForm(List<CustomEventArgs> args)
        {
            this.InitializeComponent();
            this.CustomBase.UpdateComboBox(this.cmbManufacturer, this.CustomBase.ToList(this.CustomBase.CustomDb.DB.Manufacturers.ToList()));
            this.CustomBase.UpdateOkButton(this.btnOk);
        }
        public override object HandleOk(List<CustomEventArgs> args)
        {
            if (this.CustomBase.Mode == EditModes.Create)
            {
                this.CustomBase.Result.Data = this.CustomBase.CustomDb.CreateVendorCode(
                    this.txbCode.Text,
                    this.txbAccountantCode.Text,
                    (int)this.cmbManufacturer.SelectedId,
                    (bool)this.chbIsActual.IsChecked,
                    (bool)this.chbIsPublic.IsChecked,
                    (bool)this.chbIsSale.IsChecked
                    );
            }
            if (this.CustomBase.Mode == EditModes.Update)
            {
                ProductsVendorCodes productVendorCode = this.CustomBase.UnpackCurrentObject<ProductsVendorCodes>(this.CurrentObject);
                if (productVendorCode == null)
                {
                    throw new Exception(Common.Strings.Errors.failedToGetParam);
                }
                this.CustomBase.CustomDb.UpdateVendorCode(
                    productVendorCode.idCode,
                    this.txbCode.Text,
                    this.txbAccountantCode.Text,
                    (int)this.cmbManufacturer.SelectedId,
                    this.chbIsActual.IsChecked ?? false,  // Если IsChecked == null, используйте 'false'
                    this.chbIsPublic.IsChecked ?? false,  // Если IsChecked == null, используйте 'false'
                    this.chbIsSale.IsChecked ?? false     // Если IsChecked == null, используйте 'false'
                );

                this.CustomBase.Result.Data = true;

            }
            return true;
        }
        public override object HandleCancel(List<CustomEventArgs> args)
        {
            return false;
        }
        #endregion

        #region Конструкторы/Деструкторы
        public PageEditVendorCode(CustomBase customBase, int width = 600, int height = 800) : base(customBase)
        {
            this.SetSize(width, height);
            this.InitializeComponent();
            //if (!this.CustomBase.IsTypeOrBaseEqual(typeof(VendorCodes), vendorCode))
            //    throw new Exception("Передан объект неподходящего типа! Ожидался объект типа VendorCodes");
            //this.currentItem = (VendorCodes)vendorCode;
            //this.UpdateForm((VendorCodes)vendorCode);
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.ProcessOk();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.ProcessCancel();
        }
        #endregion





    }
}
