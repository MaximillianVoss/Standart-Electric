using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
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

        #endregion

        #region Методы

        public override void UpdateFields(List<CustomEventArgs> args)
        {
            UnitsProducts unitsProducts = this.CustomBase.UnpackCurrentObject<UnitsProducts>(this.CurrentObject);
            if (unitsProducts != null)
            {
                if (this.CustomBase.Mode == EditModes.Update)
                {
                    this.txbCmbCurrentUnit.Text = unitsProducts.value.ToString();
                    this.txbCmbCurrentUnit.SelectedId = unitsProducts.idUnit;
                    this.cmbUnitType.SelectedId = unitsProducts.idType;
                }
                if (this.CustomBase.Mode == EditModes.Create)
                {
                    this.txbCmbCurrentUnit.Text = "0";
                    this.cmbUnitType.SelectFirst();
                    this.txbCmbCurrentUnit.SelectFirst();
                }
            }
        }

        public override void UpdateForm(List<CustomEventArgs> args)
        {
            this.InitializeComponent();
            this.CustomBase.UpdateOkButton(this.btnOk);
            this.CustomBase.UpdateComboBox(this.cmbUnitType, this.CustomBase.ToList(this.DB.UnitsTypes.ToList()));
            this.CustomBase.UpdateComboBox(this.txbCmbCurrentUnit, this.CustomBase.ToList(this.DB.Units.ToList()));
            // Получение символа десятичного разделителя из текущей культуры
            string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            // Создание регулярного выражения с учётом локального десятичного разделителя
            string regexPattern = @"^-?\d+(" + Regex.Escape(decimalSeparator) + @"\d+)?$";
            // Установка текста валидации и регулярного выражения для контрола
            this.txbCmbCurrentUnit.ValidationText = "Пожалуйста, введите действительное число.";
            this.txbCmbCurrentUnit.RegEx = regexPattern;
        }

        public override object HandleOk(List<CustomEventArgs> args)
        {
            UnitsProducts unitsProducts = this.CustomBase.UnpackCurrentObject<UnitsProducts>(this.CurrentObject);
            if (unitsProducts == null)
            {
                throw new Exception("Объект типа UnitsProducts должен быть передан в форму, как при создании, так и при редактированни!");
            }
            if (!this.txbCmbCurrentUnit.IsValid)
            {
                throw new Exception("Исправьте ошибки в полях перед сохранением!");
            }
            if (this.CustomBase.Mode == EditModes.Create)
            {
                this.CustomBase.Result.Data = this.CustomBase.CustomDb.CreateUnitProduct(
                    unitsProducts.idProduct,
                    (int)this.txbCmbCurrentUnit.SelectedId,
                    (int)this.cmbUnitType.SelectedId,
                    Double.Parse(this.txbCmbCurrentUnit.Text)
                    );
            }
            if (this.CustomBase.Mode == EditModes.Update)
            {
                this.CustomBase.CustomDb.UpdateUnitProduct(
                    unitsProducts.id,
                     (int)this.txbCmbCurrentUnit.SelectedId,
                    (int)this.cmbUnitType.SelectedId,
                    Double.Parse(this.txbCmbCurrentUnit.Text)
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
        public PageEditUnit(CustomBase customBase, int width = 600, int height = 800) : base(customBase)
        {
            this.SetSize(width, height);
            this.InitializeComponent();
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
