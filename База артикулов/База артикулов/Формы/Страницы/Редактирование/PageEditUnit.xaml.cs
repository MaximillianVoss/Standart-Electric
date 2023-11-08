using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для PageEditUnit.xaml
    /// </summary>
    public partial class PageEditUnit : CustomPage
    {


        #region Поля

        #endregion

        #region Свойства
        UnitsProducts currentUnit { set; get; }
        #endregion

        #region Методы

        void Update(UnitsProducts unit)
        {
            if (unit != null)
            {
                this.cmbUnitType.Update(this.CustomBase.ToList<UnitsTypes>(this.DB.UnitsTypes), unit.idType);
                this.txbCmbCurrentUnit.Update(this.CustomBase.ToList<Units>(this.DB.Units),
                    text: unit.value.ToString(),
                    currentItemId: this.currentUnit.idUnit);
            }
            else
            {
                this.cmbUnitType.Update(this.CustomBase.ToList<UnitsTypes>(this.DB.UnitsTypes));
                this.txbCmbCurrentUnit.Update(this.CustomBase.ToList<Units>(this.DB.Units), "Новое измерение");
            }
            this.DB.SaveChanges();
        }

        void Save()
        {
            if (this.currentUnit != null)
            {
                var unitProductDb = this.DB.UnitsProducts.FirstOrDefault(x => x.id == this.currentUnit.id);
                if (unitProductDb != null)
                {
                    unitProductDb.idUnit = this.txbCmbCurrentUnit.SelectedId;
                    unitProductDb.idType = this.cmbUnitType.SelectedId;
                    unitProductDb.value = Convert.ToDouble(this.txbCmbCurrentUnit.Text);
                }
                else
                {
                    this.currentUnit.idUnit = this.txbCmbCurrentUnit.SelectedId;
                    this.currentUnit.idType = this.cmbUnitType.SelectedId;
                    this.currentUnit.value = Convert.ToDouble(this.txbCmbCurrentUnit.Text);
                    this.DB.UnitsProducts.Add(this.currentUnit);
                }
            }
            else
            {
                throw new Exception("Предполагается, что мы передаем измерение как при создании, так и при редактированни!");
            }
            this.DB.SaveChanges();
        }
        #endregion

        #region Конструкторы/Деструкторы
        public PageEditUnit(object unit = null)
        {
            InitializeComponent();
            this.currentUnit = (UnitsProducts)unit;
            this.btnOk.Text =
                this.currentUnit != null || this.DB.UnitsProducts.FirstOrDefault(x => x.id == this.currentUnit.id) == null ?
            Common.Strings.Titles.Controls.Buttons.saveChanges :
            Common.Strings.Titles.Controls.Buttons.createItem;
            this.Update(this.currentUnit);
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
                this.CloseWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.CloseWindow();
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
