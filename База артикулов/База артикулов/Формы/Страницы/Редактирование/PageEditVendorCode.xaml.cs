using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        VendorCodes currentItem { set; get; }
        #endregion

        #region Методы
        void UpdateForm(VendorCodes vendorCodes)
        {
            if (vendorCodes != null)
            {
                this.chbIsActual.IsChecked = vendorCodes.isActual;
                this.chbIsPublic.IsChecked = vendorCodes.isPublic;
                this.chbIsSale.IsChecked = vendorCodes.isSale;
                this.txbCode.Text = vendorCodes.Descriptors == null ? String.Empty : vendorCodes.Descriptors.title;
                this.txbAccountantCode.Text = vendorCodes.codeAccountant;
                this.cmbManufacturer.Update(this.CustomBase.ToList<Manufacturers>(this.DB.Manufacturers), vendorCodes.idManufacturer);
            }
        }
        void Save()
        {
            //this.InitDB();
            this.currentItem = new VendorCodes();
            if (this.currentItem != null)
            {
                this.currentItem.isActual = this.chbIsActual.IsChecked.GetValueOrDefault();
                this.currentItem.isPublic = this.chbIsPublic.IsChecked.GetValueOrDefault();
                this.currentItem.isSale = this.chbIsSale.IsChecked.GetValueOrDefault();
                if (this.currentItem.Descriptors != null)
                {
                    this.currentItem.Descriptors.title = this.txbCode.Text;
                }
                else
                {
                    Descriptors descriptors = new Descriptors();
                    descriptors.title = this.txbCode.Text;
                    descriptors = this.DB.Descriptors.Add(descriptors);
                    this.currentItem.Descriptors = descriptors;
                }
                this.currentItem.codeAccountant = this.txbAccountantCode.Text;
                this.currentItem.idManufacturer = (int)this.cmbManufacturer.SelectedId;
                if (this.DB.Entry(this.currentItem).State == EntityState.Detached)
                {
                    this.DB.VendorCodes.Add(this.currentItem);
                }
                this.DB.SaveChanges();
                this.OnDataChanged(this.currentItem);
            }
        }
        #endregion

        #region Конструкторы/Деструкторы
        public PageEditVendorCode(object vendorCode)
        {
            InitializeComponent();
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
            try
            {
                this.Save();
                this.CloseWindow();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.CloseWindow(false);
        }

        public override void UpdateFields(List<CustomEventArgs> args)
        {
            throw new NotImplementedException();
        }

        public override void UpdateForm(List<CustomEventArgs> args)
        {
            throw new NotImplementedException();
        }

        public override object HandleOk(List<CustomEventArgs> args)
        {
            throw new NotImplementedException();
        }

        public override object HandleCancel(List<CustomEventArgs> args)
        {
            throw new NotImplementedException();
        }
        #endregion





    }
}
