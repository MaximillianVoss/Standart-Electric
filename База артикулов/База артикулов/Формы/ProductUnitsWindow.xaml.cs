using System;
using System.Linq;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы
{
    /// <summary>
    /// Окно для добавления и редактирования измерений для продукта
    /// </summary>
    public partial class ProductUnitsWindow : CustomWindow
    {


        #region Поля

        #endregion

        #region Свойства
        /// <summary>
        /// ID продукта
        /// </summary>
        private int? IdProduct { get; set; }

        /// <summary>
        /// ID типа измерения
        /// </summary>
        private int? IdUnitType { get; set; }
        #endregion

        #region Методы
        private void UpdateForm(int? idProduct)
        {
            this.cmbUnitType.Update(this.ToList<UnitsTypes>(this.DB.UnitsTypes));
            this.UpdateUnits(idProduct, this.IdUnitType);
        }

        private void UpdateUnits(int? idProduct = null, int? idUnitType = null)
        {
            var productUnits = this.DB.UnitsProducts.FirstOrDefault(x => x.idProduct == idProduct && x.idType == idUnitType);
            this.btnDelete.IsEnabled = productUnits != null;
            #region Если в продукт уже добавлено это измерение
            if (productUnits != null)
            {
                this.txbCmbCurrentUnit.Update(this.ToList<Units>(this.DB.Units), productUnits.value.ToString(), "", "", productUnits.idUnit);
                this.btnOk.Text = Common.Strings.Controls.btnEdit;
            }
            #endregion
            #region Если в продуктe нет этого измерения
            else
            {
                this.txbCmbCurrentUnit.Update(this.ToList<Units>(this.DB.Units), "0", "", "", this.DB.Units.FirstOrDefault().id);
                this.btnOk.Text = Common.Strings.Controls.btnAdd;
            }
            #endregion
        }

        private void Save(int? idProduct, int? idUnitType)
        {
            if (this.IdProduct == null)
            {
                throw new Exception(Common.Strings.Errors.emptyObject);
            }
            var productUnits = this.DB.UnitsProducts.FirstOrDefault(x => x.idProduct == idProduct && x.idType == idUnitType);
            #region Если в продукт уже добавлено это измерение
            if (productUnits != null)
            {
                productUnits.idUnit = this.txbCmbCurrentUnit.SelectedId;
                productUnits.idType = idUnitType;
                productUnits.value = Convert.ToDouble(this.txbCmbCurrentUnit.Text);
            }
            #endregion

            #region Если в продуктe нет этого измерения
            else
            {
                var newProductUnit = new UnitsProducts(
                   (int)idProduct,
                   this.txbCmbCurrentUnit.SelectedId,
                   idUnitType,
                   Convert.ToDouble(this.txbCmbCurrentUnit.Text)
                   );
                this.DB.UnitsProducts.Add(newProductUnit);
                this.btnOk.Text = Common.Strings.Controls.btnEdit;
            }
            #endregion
            this.DB.SaveChanges();
        }
        #endregion

        #region Конструкторы/Деструкторы
        public ProductUnitsWindow(int idProduct)
        {
            this.InitializeComponent();
            this.IdProduct = idProduct;
            this.UpdateForm(this.IdProduct);
        }
        public ProductUnitsWindow() : this(-1)
        {

        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void cmbUnitType_SelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            this.IdUnitType = this.cmbUnitType.SelectedId;
            this.UpdateUnits(this.IdProduct, this.cmbUnitType.SelectedId);
        }
        private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.Save(this.IdProduct, this.IdUnitType);
                this.UpdateForm(this.IdProduct);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var unitProduct = this.DB.UnitsProducts.FirstOrDefault(x => x.idProduct == this.IdProduct && x.idType == this.cmbUnitType.SelectedId);
                if (unitProduct != null)
                {
                    this.DB.UnitsProducts.Remove(unitProduct);
                    this.DB.SaveChanges();
                    this.UpdateForm(this.IdProduct);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
        private void CustomWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.Title = this.DB.Products.FirstOrDefault(x => x.id == this.IdProduct).Descriptors.title;
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        #endregion


    }
}
