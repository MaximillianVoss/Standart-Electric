﻿using CustomControlsWPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы.Страницы.Редактирование
{
    /// <summary>
    /// Логика взаимодействия для PageEditProduct.xaml
    /// </summary>
    public partial class PageEditProduct : CustomPage
    {
        #region Поля
        private Type dbSetType;
        private int idProduct;
        /// <summary>
        /// Продукт, который создается или редактируется
        /// </summary>
        private ProductsView currentProduct;
        /// <summary>
        /// Измерение, выбранное в таблице на форме
        /// </summary>
        private UnitsProducts currentUnit;
        /// <summary>
        /// Файл, выбранные в таблице файлов
        /// </summary>
        private ResourcesViewProducts currentResource;
        #endregion

        #region Свойства
        public int? IdProduct { get => this.idProduct; set => this.idProduct = (int)value; }
        public Type DbSetType { get => this.dbSetType; set => this.dbSetType = value; }
        public ProductsView CurrentProduct { get => this.currentProduct; set => this.currentProduct = value; }
        #endregion

        #region Методы
        /// <summary>
        /// Получает путь до текущего каталога с продуктом
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string GetPath(ProductsView product)
        {
            if (product == null)
            {
                throw new Exception(Common.Strings.Errors.emptyObject);
            }
            String result = String.Empty;
            result = String.Format("{0}\u2192{1}\u2192{2}",
                product.Наименование_подгруппы == null ? String.Empty : product.Наименование_подгруппы,
                product.Наименование_группы == null ? String.Empty : product.Наименование_группы,
                product.Наименование_класса == null ? String.Empty : product.Наименование_класса
                );
            return result;
        }

        /// <summary>
        /// Обновляет названия полей
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

        private async Task<string> DownloadImageAsync(string code)
        {
            try
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
                string localImagePath = String.Format("{0}\\{1}.png", cachedDirectory, code);
                string cloudImagePath = String.Format("{0}/{1}.png", Common.Strings.Path.Cloud.images, code);
                #endregion

                // Проверяем наличие файла на локальном диске
                if (File.Exists(localImagePath))
                {
                    return localImagePath;
                }

                #region Загружаем изображение
                bool isExists = await this.WDClient.IsFileExists(cloudImagePath);
                if (isExists)
                {
                    _ = this.WDClient.DownloadFile(localImagePath, cloudImagePath);
                    return localImagePath;
                }
                else
                {
                    return null;
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productsView"></param>
        /// <returns></returns>
        private async Task UpdateImageAsync(ProductsView productsView)
        {
            try
            {
                if (productsView == null)
                    return;

                List<String> cloudPaths = new List<string>()
            {
                productsView.Артикул,
                productsView.Код_подгруппы,
                productsView.Код_группы,
                productsView.Код_класса,
                //Изобржаение заглушка 0000.png
                "0000"
            };
                string localImagePath = String.Empty;
                for (int i = 0; i < cloudPaths.Count; i++)
                {
                    localImagePath = await this.DownloadImageAsync(cloudPaths[i]);
                    if (localImagePath != null)
                    {
                        break;
                    }
                }

                this.Dispatcher.Invoke(() =>
                {
                    if (!String.IsNullOrEmpty(localImagePath) && File.Exists(localImagePath))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(localImagePath);
                        bitmap.EndInit();
                        this.imgPreview.Source = bitmap;
                    }
                });
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }


        /// <summary>
        /// Заполняет форму свойствами указанного продукта
        /// </summary>
        /// <param name="productView">продукт</param>
        /// <exception cref="Exception"></exception>
        private void UpdateForm(ProductsView productView)
        {
            try
            {
                if (productView == null)
                {
                    throw new Exception(Common.Strings.Errors.emptyObject);
                }
                var product = this.DB.Products.FirstOrDefault(x => x.id == productView.ID_продукта);
                if (product == null)
                {
                    throw new Exception("Не удалось найти продукт!");
                }
                #region Изображение
                _ = this.UpdateImageAsync(productView);
                #endregion
                #region Каталог
                this.txbPath.Text = this.GetPath(productView);
                #endregion
                #region Название
                this.txbTitle.Text = productView.Наименование_продукта;
                #endregion
                #region Описание
                this.txbDescription.Text = productView.Описание_продукта;
                #endregion
                #region Артикул
                this.txbVendorCode.IsEnabled = false;
                this.txbVendorCode.Text = productView.Артикул;
                #endregion
                #region Бухгалтерский код
                this.txbCodeAccountant.IsEnabled = false;
                this.txbCodeAccountant.Text = productView.Бухгалтерский_код;
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
                //TODO:обновить представление, неправильно показывает
                //this.dgDimensions.ItemsSource = this.DB.ProductUnitsView.Where(x => x.ID_товара == this.CurrentProduct.ID_продукта).Distinct().ToList();
                var entityConnStr = ConfigurationManager.ConnectionStrings[Settings.Connections.CurrentConnectionString].ConnectionString;
                var entityBuilder = new EntityConnectionStringBuilder(entityConnStr);
                string connectionString = entityBuilder.ProviderConnectionString;
                string query = String.Format("SELECT * FROM ProductUnitsView where [ID товара] = {0}", this.CurrentProduct.ID_продукта);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        this.dgDimensions.ItemsSource = dataTable.DefaultView;
                    }
                }

                #endregion
                #region Таблица файлов
                //TODO:обновить представление, неправильно показывает
                this.dgFiles.ItemsSource = this.DB.ResourcesViewProducts.ToList();
                #endregion
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Заполняет форму свойствами указанного продукта
        /// </summary>
        /// <param name="idProduct">ID продукта</param>
        private void UpdateForm(int? idProduct)
        {
            this.UpdateForm(this.DB.ProductsView.FirstOrDefault(x => x.ID_продукта == this.IdProduct));
        }

        void UpdateUnitSelection()
        {
            DataRowView rowView = dgDimensions.SelectedItem as DataRowView;
            if (rowView != null)
            {
                // Для получения значения определенного столбца используйте:
                var idProduct = Convert.ToInt32(rowView["ID товара"]);
                var idUnitProduct = Convert.ToInt32(rowView["ID связи продукт-измерение"]);
                this.currentUnit = this.DB.UnitsProducts.FirstOrDefault(x => x.idProduct == idProduct && x.id == idUnitProduct);
                this.DB.Entry(this.currentUnit).Reload();
            }
        }

        void UpdateFileSelection()
        {
            this.currentResource = this.dgFiles.SelectedItem as ResourcesViewProducts;
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
        /// Сохраняет изменения, внесенные в продукт
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

        public PageEditProduct(object product)
        {
            this.InitializeComponent();
            try
            {
                if (product.GetType() != typeof(ProductsView))
                    throw new Exception("Ожидался объект типа ProductsView");
                this.CurrentProduct = (ProductsView)product;
                this.IdProduct = this.CurrentProduct.ID_продукта;
                this.UpdateFieldsTitles();
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

        private void CustomPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
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
                this.CloseWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CloseWindow(false);
        }
        private void btnAddDimension_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ProductUnitsWindow productUnitsWindow = new ProductUnitsWindow((int)this.IdProduct);
            productUnitsWindow.ShowDialog();
            this.UpdateForm(this.idProduct);
        }
        private void imgPreview_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _ = UpdateImageAsync(this.CurrentProduct);
        }


        #region Контекстное меню таблиц

        #region Измерения
        private void dgDimensions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateUnitSelection();
            //this.ShowError(dgDimensions.SelectedItem.ToString());
        }
        private void dgDimensions_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
        private void btnDimensionsAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var unitProduct = new UnitsProducts();
                unitProduct.idProduct = this.currentProduct.ID_продукта;
                WindowEdit windowEdit = new WindowEdit(Common.Strings.Titles.Windows.add, unitProduct, WindowEditModes.Create);
                windowEdit.ShowDialog();
                if ((bool)windowEdit.DialogResult)
                {
                    this.InitDB();
                    this.UpdateForm(this.currentProduct);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnDimensionsEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.InitDB();
                this.UpdateUnitSelection();
                WindowEdit windowEdit = new WindowEdit(Common.Strings.Titles.Windows.edit, this.currentUnit);
                windowEdit.ShowDialog();
                if ((bool)windowEdit.DialogResult)
                {
                    this.InitDB();
                    this.UpdateForm(this.currentProduct);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnDimensionsDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (this.currentUnit != null)
                {
                    this.DB.UnitsProducts.Remove(
                        this.DB.UnitsProducts.FirstOrDefault(x => x.id == this.currentUnit.id));
                    this.DB.SaveChanges();
                    this.UpdateForm(this.currentProduct);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        #endregion

        #region Файлы
        private void dgFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateFileSelection();
        }
        private void dgFiles_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
        private void btnFilesDownload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.currentResource != null)
            {
                var resource = this.DB.Resources.FirstOrDefault(x => x.id == this.currentResource.ID_ресурса);
                if (resource != null)
                {
                    _ = this.WDClient.DownloadFile(this.GetSaveFilePath(), resource.URL);
                    this.ShowMessage("Загрузка завершена");
                }
            }
        }
        private void btnFilesAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                ResourcesViewProducts resourcesViewProducts = new ResourcesViewProducts();
                resourcesViewProducts.ID_продукта = this.currentProduct.ID_продукта;
                if (this.IsDescriptorProductExists(this.currentProduct.ID_продукта))
                {
                    resourcesViewProducts.ID_дескриптора_объекта = this.GetDescriptorProduct(this.currentProduct.ID_продукта).id;
                    //this.ShowMessage(resourcesViewProducts.ID_дескриптора_объекта.ToString());
                }
                WindowEdit windowEdit = new WindowEdit(
                    Common.Strings.Titles.Windows.add,
                    resourcesViewProducts,
                    WindowEditModes.Create);
                windowEdit.ShowDialog();
                if ((bool)windowEdit.DialogResult)
                {
                    this.InitDB();
                    this.UpdateForm(this.currentProduct);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnFilesEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        private void btnFilesDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (this.currentResource != null)
                {
                    // Получите все записи, которые удовлетворяют вашему условию
                    var itemsToDelete = this.DB.DescriptorsResources
                                                .Where(dr => dr.idResource == this.currentResource.ID_ресурса)
                                                .ToList();

                    // Удалите эти записи
                    foreach (var item in itemsToDelete)
                    {
                        this.DB.DescriptorsResources.Remove(item);
                    }

                    // Сохраните изменения
                    this.DB.SaveChanges();
                    this.InitDB();
                    this.UpdateForm(this.currentProduct);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnRefreshFilesTable_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.InitDB();
                this.UpdateForm(this.currentProduct);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        #endregion

        #endregion

        #endregion

    }
}
