using CustomControlsWPF;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using База_артикулов.Классы;
using База_артикулов.Модели;
using CommonLocal = База_артикулов.Классы.Common;

namespace База_артикулов.Формы
{
    /// <summary>
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : CustomWindow
    {


        #region Поля
        private Type dbSetType;
        private int idProduct;
        #endregion

        #region Свойства
        public int? IdProduct { get => this.idProduct; set => this.idProduct = (int)value; }
        public Type DbSetType { get => this.dbSetType; set => this.dbSetType = value; }
        #endregion

        #region Методы
        /// <summary>
        /// Получает путь до текущего каталога с продуктом
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string GetPath(Products product)
        {
            if (product == null)
            {
                throw new Exception(CommonLocal.Strings.Errors.emptyObject);
            }
            String result = String.Empty;
            result = String.Format("{0}\u2192{1}\u2192{2}",
                product.SubGroups.Groups.Classes == null ? String.Empty : product.SubGroups.Groups.Classes.Descriptors.title,
                product.SubGroups.Groups == null ? String.Empty : product.SubGroups.Groups.Descriptors.title,
                product.SubGroups == null ? String.Empty : product.SubGroups.Descriptors.title
                );
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateFieldsTitles()
        {
            foreach (var childControl in this.gridFields.Children)
            {
                if (childControl.GetType() == typeof(LabeledTextBox))
                {
                    var control = childControl as LabeledTextBox;
                    var field = this.DB.Fields.FirstOrDefault(x => x.Descriptors.title.ToLower() == control.Name.Replace("txb", "").ToLower());
                    if (field != null)
                    {
                        control.Title = field.titleRus;
                    }
                }
                if (childControl.GetType() == typeof(LabeledComboBox))
                {
                    var control = childControl as LabeledComboBox;
                    var field = this.DB.Fields.FirstOrDefault(x => x.Descriptors.title.ToLower() == control.Name.Replace("cmb", "").ToLower());
                    if (field != null)
                    {
                        control.Title = field.titleRus;
                    }
                }
                if (childControl.GetType() == typeof(LabeledCheckBox))
                {
                    var control = childControl as LabeledCheckBox;
                    var field = this.DB.Fields.FirstOrDefault(x => x.Descriptors.title.ToLower() == control.Name.Replace("chb", "").ToLower());
                    if (field != null)
                    {
                        control.Title = field.titleRus;
                    }
                }

            }
        }

        private async Task<string> DownloadImageAsync(string vendorCode)
        {
            #region Создание подпапок с кэшированными изображениями
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imagesDirectory = Path.Combine(baseDirectory, Common.Strings.Path.Local.imagesFolderName);
            string cachedDirectory = Path.Combine(imagesDirectory, Common.Strings.Path.Local.imagesCachedFolderName);

            if (!Directory.Exists(imagesDirectory))
            {
                Directory.CreateDirectory(imagesDirectory);
            }
            if (!Directory.Exists(cachedDirectory))
            {
                Directory.CreateDirectory(cachedDirectory);
            }
            #endregion

            #region Пути к файлам
            string localImagePath = String.Format("{0}\\{1}.png", cachedDirectory, vendorCode);
            string cloudImagePath = String.Format("{0}/{1}.png", Common.Strings.Path.Cloud.images, vendorCode);
            #endregion



            #region Загружаем изображение
            bool isExists = await this.WDClient.IsFileExists(cloudImagePath);
            if (isExists)
            {
                this.WDClient.DownloadFile(localImagePath, cloudImagePath);
            }
            else
            {
                return null;
            }
            #endregion
            return localImagePath;
        }

        private async Task UpdateImageAsync(string vendorCode)
        {
            if (vendorCode != null)
            {
                string localImagePath = null;
                string cloudImagePath = String.Format("{0}/{1}.png", Common.Strings.Path.Cloud.images, vendorCode);
                bool isExists = await this.WDClient.IsFileExists(cloudImagePath);
                #region Загрузка файла
                if (isExists)
                #region Загружаем собственное изображение
                {
                    localImagePath = await this.DownloadImageAsync(vendorCode);
                }
                #endregion
                else
                #region Загружаем изображение подгруппы
                {
                    var product = this.DB.Products.FirstOrDefault(x => x.id == this.idProduct);
                    if (product != null)
                    {
                        cloudImagePath = String.Format("{0}/{1}.png", Common.Strings.Path.Cloud.images, product.SubGroups.Descriptors.code);
                        isExists = await this.WDClient.IsFileExists(cloudImagePath);
                        if (isExists)
                        {
                            localImagePath = this.DownloadImageAsync(vendorCode).Result;
                        }
                    }
                }
                #endregion
                #endregion

                if (localImagePath != null)
                #region Установка изображения в качестве превью
                {
                    var image = new Image();
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(localImagePath);
                    bitmap.EndInit();
                    this.imgPreview.Source = bitmap;
                }
                #endregion
            }
        }

        /// <summary>
        /// Заполняет форму свойствами указанного продукта
        /// </summary>
        /// <param name="product">продукт</param>
        /// <exception cref="Exception"></exception>
        private async void UpdateForm(Products product)
        {
            if (product == null)
            {
                throw new Exception(CommonLocal.Strings.Errors.emptyObject);
            }
            #region Каталог
            this.txbPath.Text = this.GetPath(product);
            #endregion
            #region Название
            this.txbTitle.Text = product.Descriptors.title;
            #endregion
            #region Описание
            this.txbDescription.Text = product.Descriptors.description;
            #endregion
            #region Артикул
            var currentProductVendorCodes = this.DB.ProductsVendorCodes.FirstOrDefault(x => x.idProduct == product.id);
            this.txbVendorCode.IsEnabled = false;
            if (currentProductVendorCodes != null)
            {
                if (currentProductVendorCodes.VendorCodes != null && currentProductVendorCodes.VendorCodes.Descriptors != null)
                {
                    this.txbVendorCode.Text = currentProductVendorCodes.VendorCodes.Descriptors.title;
                    await this.UpdateImageAsync(currentProductVendorCodes.VendorCodes.Descriptors.title);
                }
            }
            #endregion
            #region Бухгалтерский код
            this.txbCodeAccountant.IsEnabled = false;
            if (currentProductVendorCodes != null)
            {
                if (currentProductVendorCodes.VendorCodes != null && currentProductVendorCodes.VendorCodes.Descriptors != null)
                {
                    this.txbCodeAccountant.Text = currentProductVendorCodes.VendorCodes.codeAccountant;
                }
            }
            #endregion
            #region Короткое название
            this.txbTitleShort.Text = product.Descriptors.titleShort;
            #endregion
            #region Нормы
            this.cmbNorm.Update(this.ToList<Norms>(this.DB.Norms), product.idNorm);
            #endregion
            #region Подгруппа
            this.cmbSubGroup.Update(this.ToList<SubGroups>(this.DB.SubGroups), product.idSubGroup);
            #endregion
            #region Покрытия
            this.cmbCover.Update(this.ToList<Covers>(this.DB.Covers), product.idCover);
            #endregion
            #region Материалы
            this.cmbMaterial.Update(this.ToList<Materials>(this.DB.Materials), product.idMaterial);
            #endregion
            #region Упаковки
            this.cmbPackage.Update(this.ToList<Packages>(this.DB.Packages), product.idPackage);
            #endregion
            #region Перфорации
            this.cmbPerforation.Update(this.ToList<Perforations>(this.DB.Perforations), product.idPerforation);
            #endregion
            #region Отметка "На складе"
            this.UpdateCheckBox(this.chbInStock, "На складе", "Под заказ", product.isInStock);
            #endregion
            #region Таблица измерений
            this.dgDimensions.ItemsSource = null;
            this.dgDimensions.ItemsSource = this.ToList(this.DB.AllProductUnitsInfoView.Where(x => x.idProduct == product.id).ToList());
            #endregion
        }

        /// <summary>
        /// Заполняет форму свойствами указанного продукта
        /// </summary>
        /// <param name="idProduct">ID продукта</param>
        private void UpdateForm(int? idProduct)
        {
            this.UpdateForm(this.DB.Products.FirstOrDefault(x => x.id == this.IdProduct));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        private void FilterFields(string fieldName)
        {
            if (this.gridFields != null)
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
                        if (childControl.GetType() == typeof(LabeledCheckBox))
                        {
                            var control = childControl as LabeledCheckBox;
                            if (!control.Title.ToLower().Contains(fieldName.ToLower()))
                            {
                                control.Visibility = System.Windows.Visibility.Collapsed;
                            }
                        }

                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// Сохраняет изменения, внмесенные в продукт
        /// </summary>
        /// <param name="idProduct"></param>
        private void Save(int? idProduct)
        {
            var product = this.DB.Products.FirstOrDefault(x => x.id == idProduct);
            if (product != null)
            {
                #region Дескриптор
                product.Descriptors.title = this.txbTitle.Text;
                product.Descriptors.titleShort = this.txbTitleShort.Text;
                product.Descriptors.description = this.txbDescription.Text;
                product.Descriptors.code = this.txbVendorCode.Text;
                #endregion
                #region Артикул
                var productVendorCode = this.DB.ProductsVendorCodes.FirstOrDefault(x => x.idProduct == product.id);
                if (productVendorCode != null)
                {
                    var vendorCode = this.DB.VendorCodes.FirstOrDefault(x => x.id == productVendorCode.idCode);
                    if (vendorCode != null)
                    {
                        vendorCode.Descriptors.title = this.txbVendorCode.Text;
                    }
                }
                else
                {
                    var descriptor = new Descriptors(this.txbVendorCode.Text, this.txbVendorCode.Text, null, null);
                    this.DB.Descriptors.Add(descriptor);
                    var vendorCode = new VendorCodes(descriptor);
                    this.DB.VendorCodes.Add(vendorCode);
                    productVendorCode = new ProductsVendorCodes(product, vendorCode);
                    this.DB.ProductsVendorCodes.Add(productVendorCode);

                }
                #endregion
                #region Норма
                product.idNorm = this.cmbNorm.SelectedId;
                #endregion
                #region Подгруппа
                product.idSubGroup = this.cmbSubGroup.SelectedId;
                #endregion
                #region Покрытие
                product.idCover = this.cmbCover.SelectedId;
                #endregion
                #region Материал
                product.idMaterial = this.cmbMaterial.SelectedId;
                #endregion
                #region Перфорация
                product.idPerforation = this.cmbPerforation.SelectedId;
                #endregion
                #region Упаковка
                product.idPackage = this.cmbPackage.SelectedId;
                #endregion
                #region В наличии
                product.isInStock = (bool)this.chbInStock.IsChecked;
                #endregion
                this.DB.SaveChanges();
            }
        }
        #endregion

        #region Конструкторы/Деструкторы
        public ProductWindow(Type dbSetType, int? idProduct = null)
        {
            this.InitializeComponent();
            try
            {
                this.UpdateFieldsTitles();
                this.DbSetType = dbSetType;
                this.IdProduct = idProduct;
                this.UpdateForm(this.IdProduct);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void ProductWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //ProductUnitsWindow productUnitsWindow = new ProductUnitsWindow(1, 7);
            //productUnitsWindow.ShowDialog();
            this.InitClient();
        }
        private void txbFieldName_TextChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.FilterFields(this.txbFieldName.Text);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void cmbUnitType_SelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            //if (this.cmbUnitType.SelectedIndex != -1)
            //{
            //    Products currentProduct = this.db.Products.FirstOrDefault(x => x.id == this.idProduct);
            //    var selectedType = this.cmbUnitType.Items[this.cmbUnitType.SelectedIndex];

            //    if (currentProduct != null && selectedType != null)
            //    {
            //        int typeId = (int)this.GetObjectFieldValue(selectedType, "idType");
            //        var currentUnitProduct = this.db.UnitsProducts.FirstOrDefault(x => x.idProduct == currentProduct.id && x.idType == typeId);
            //        if (currentUnitProduct != null)
            //        {
            //            this.txbCmbCurrentUnit.Items = ToList(this.db.Units.ToList());
            //            this.txbCmbCurrentUnit.Title = currentUnitProduct.UnitsTypes.title;
            //            this.txbCmbCurrentUnit.Select(currentUnitProduct.idUnit);
            //            this.txbCmbCurrentUnit.Text = currentUnitProduct.value.ToString();
            //        }
            //    }
            //}
        }
        private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.Save(this.IdProduct);
                this.Close();
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
        private void btnAddDimension_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ProductUnitsWindow productUnitsWindow = new ProductUnitsWindow((int)this.IdProduct);
            productUnitsWindow.ShowDialog();
            this.UpdateForm(this.idProduct);
        }
        #endregion

        private void CustomWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
