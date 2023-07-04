using CustomControlsWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using База_артикулов.Модели;

namespace База_артикулов.Формы
{
    /// <summary>
    /// Логика взаимодействия для ProductWindow_OLD.xaml
    /// </summary>
    public partial class ProductWindow_OLD : CustomWindow
    {


        #region Поля
        private Type dbSetType;
        private int itemId;
        #endregion

        #region Свойства
        public Type DbSetType { get => this.dbSetType; set => this.dbSetType = value; }
        public int ItemId { get => this.itemId; set => this.itemId = value; }
        #endregion

        #region Методы
        //List<object> ToList(DbSet<Units> items)
        //{
        //    List<object> list = new List<object>();
        //    foreach (var item in items)
        //    {
        //        list.Add(new
        //        {
        //            id = item.id,
        //            title = item.title
        //        }); 
        //    }
        //    return list;
        //}
        //List<object> ToList(DbSet<TovarSubGroups> items)
        //{
        //    List<object> list = new List<object>();
        //    foreach (var item in items)
        //    {
        //        list.Add(new
        //        {
        //            id = item.id,
        //            title = item.TovarSubGroupTitle
        //        });
        //    }
        //    return list;
        //}
        //List<object> ToList(DbSet<Materials> items)
        //{
        //    List<object> list = new List<object>();
        //    foreach (var item in items)
        //    {
        //        list.Add(new
        //        {
        //            id = item.id,
        //            title = item.MaterialTitle
        //        });
        //    }
        //    return list;
        //}
        //List<object> ToList(DbSet<Norms> items)
        //{
        //    List<object> list = new List<object>();
        //    foreach (var item in items)
        //    {
        //        list.Add(new
        //        {
        //            id = item.id,
        //            title = item.NormTitle
        //        });
        //    }
        //    return list;
        //}
        //List<object> ToList(DbSet<PerforationSizes> items)
        //{
        //    List<object> list = new List<object>();
        //    foreach (var item in items)
        //    {
        //        list.Add(new
        //        {
        //            id = item.id,
        //            title = item.Perforation_Size_String
        //        }); ;
        //    }
        //    return list;
        //}
        private void UpdateComboTextBox(LabeledTextBoxAndComboBox control, int currentItemId, List<object> items)
        {
            control.Items.Clear();
            control.Items = items;

        }

        /// <summary>
        /// Заполняем поля формы свойствами указаннго объекта
        /// </summary>
        /// <param name="product"></param>
        private void FillForm(Products product)
        {
            //C:\_Стандарт-Электрик\web - Материалы для сайта\webfiles
            if (product != null)
            {
                //this.txbFullName.Text = product.FullName;
                //this.cmbtxbEdIzmWeightMain.Update(
                //    product.WeightMain.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr,
                //    product.ID_EdIzmWeightMain,
                //    ToList(db.EdIzm)
                //    );
                //this.cmbtxbEdIzmWeightAlternative.Update(
                //    product.WeightAlternative.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr,
                //    product.ID_EdIzmWeightAlternative, ToList(db.EdIzm)
                //    );
                //this.txbWeightFactor.Update(
                //    product.WeightFactor.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr
                //    );
                //this.cmbtxbLength.Update(
                //    product.Length.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr,
                //    product.ID_EdIzmLenght,
                //    ToList(db.EdIzm)
                //    );
                //this.cmbtxbHeight.Update(
                //    product.Height.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr,
                //    product.ID_EdIzmHeight
                //    , ToList(db.EdIzm)
                //    );
                //this.cmbtxbWidth.Update(
                //    product.Width.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr,
                //    product.ID_EdIzmWidth,
                //    ToList(db.EdIzm)
                //    );
                //this.txbThickness.Update(
                //    product.Thickness.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr
                //    );
                //this.cmbtxbVolume.Update(
                //    product.Volume.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr,
                //    product.ID_EdIzmVolume,
                //    ToList(db.EdIzm)
                //    );
                //this.cmbTovarSubGroups.Update(
                //    product.ID_TovarSubGroups,
                //    ToList(db.TovarSubGroups)
                //    );
                //this.cmbMaterial.Update(
                //    product.ID_Materials,
                //    ToList(db.Materials)
                //    );
                //this.cmbNorm.Update(
                //    product.ID_Norm,
                //    ToList(db.Norms)
                //    );
                //this.checkboxAvailabilityInStock.Update(
                //    product.AvailabilityInStock,
                //    "Это складская позиция",
                //    "Это заказная позиция"
                //    );
                //this.cmbPerforationSize.Update(
                //    product.ID_PerforationSize,
                //    ToList(db.PerforationSizes)
                //    );
                //this.txbAmountInPackage.Update(
                //    product.AmountInPackage.Value.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regInt,
                //    LocalCommon.Strings.Errors.incorrectIntStr
                //    );
                //this.txbNumberOfPackagesPerPallet.Update(
                //    product.NumberOfPackagesPerPallet.Value.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regInt,
                //    LocalCommon.Strings.Errors.incorrectIntStr
                //    );
                //this.txbMinimumOrder.Update(
                //    product.MinimumOrder.Value.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regInt,
                //    LocalCommon.Strings.Errors.incorrectIntStr
                //    );
                //this.txbMultiplicity.Update(
                //    product.Multiplicity.Value.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regInt,
                //    LocalCommon.Strings.Errors.incorrectIntStr
                //    );
                //this.txbWeightOfPackage.Update(
                //    product.WeightOfPackage.Value.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr
                //    );
                //this.txbWeightofPallet.Update(
                //    product.WeightOfPallet.Value.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr
                //    );
                //this.txbPackageWidth.Update(
                //    product.PackageWidth.Value.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr
                //    );
                //this.txbPackageLength.Update(
                //    product.PackageLenght.Value.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr
                //    );
                //this.txbPackageHeight.Update(
                //    product.PackageHeight.Value.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr
                //    );
                //this.checkboxStackability.Update(
                //    product.Stackability,
                //    "Этот продукт можно штабелировать",
                //    "Этот продукт нельзя штабелировать"
                //    );
                //this.txbPointLoadCapacity.Update(
                //    product.PointLoadCapacity.Value.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr
                //    );
                //this.txbDistributedLoadCapacity.Update(
                //    product.DistributedLoadCapacity.Value.ToString(),
                //    LocalCommon.Strings.RegularExpressions.regFloat,
                //    LocalCommon.Strings.Errors.incorrectFloatStr
                //    );



                //this.cmbID_EdIzmWeightAlternative.Items = ToList(this.db.EdIzm);
                //this.txbFullName.RegEx = @"[0-9]{1,}$";
                //this.txbFullName.ValidationText = "Вы должны ввести число!";
                //this.cmbID_EdIzmWeightAlternative.Select(product.ID_EdIzmWeightAlternative);
                //this.cmbID_EdIzmWeightAlternative.Text = product.ID_EdIzmWeightAlternative.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        private void FilterFields(string fieldName)
        {
            #region Показать элементы, если поле не пустое
            if (String.IsNullOrEmpty(fieldName))
            {
                foreach (var childControl in this.gridFields.Children)
                {
                    var control = childControl as UserControl;
                    control.Visibility = System.Windows.Visibility.Visible;
                }
            }
            #endregion
            #region Скрыть элементы, если поле пустое
            else
            {
                foreach (var childControl in this.gridFields.Children)
                {
                    if (childControl.GetType() == typeof(LabeledTextBox))
                    {
                        var control = childControl as LabeledTextBox;
                        if (!control.Title.ToLower().Contains(fieldName.ToLower()))
                        {
                            control.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                    if (childControl.GetType() == typeof(LabeledComboBox))
                    {
                        var control = childControl as LabeledComboBox;
                        if (!control.Title.ToLower().Contains(fieldName.ToLower()))
                        {
                            control.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                    if (childControl.GetType() == typeof(LabeledTextBoxAndComboBox))
                    {
                        var control = childControl as LabeledTextBoxAndComboBox;
                        if (!control.Title.ToLower().Contains(fieldName.ToLower()))
                        {
                            control.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }

                }
            }
            #endregion

        }
        #endregion

        #region Конструкторы/Деструкторы
        public ProductWindow_OLD(Type dbSetType, int itemId = -1)
        {
            this.InitializeComponent();
            this.dbSetType = dbSetType;
            this.itemId = itemId;
            if (this.dbSetType != null && this.itemId != -1)
            {
                this.FillForm(this.DB.Products.FirstOrDefault(x => x.id == this.ItemId));
            }
        }




        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void txbFieldName_TextChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            this.FilterFields(this.txbFieldName.Text);
        }
        #endregion
    }
}
