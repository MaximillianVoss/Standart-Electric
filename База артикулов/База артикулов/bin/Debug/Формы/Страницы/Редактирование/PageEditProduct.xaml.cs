using CustomControlsWPF;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using База_артикулов.Классы;
using База_артикулов.Классы.Дополнения;
using База_артикулов.Модели;

namespace База_артикулов.Формы.Страницы.Редактирование
{
    /// <summary>
    /// Логика взаимодействия для PageEditProduct.xaml
    /// </summary>
    public partial class PageEditProduct : CustomPage
    {
        #region Поля
        /// <summary>
        /// Тип коллекции объектов
        /// </summary>
        private Type dbSetType;
        /// <summary>
        /// ID продукта
        /// </summary>
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
        /// <summary>
        /// Получает или устанавливает идентификатор продукта.
        /// </summary>
        public int? IdProduct
        {
            get => this.idProduct; set => this.idProduct = (int)value;
        }

        /// <summary>
        /// Получает или устанавливает тип DbSet.
        /// </summary>
        public Type DbSetType
        {
            get => this.dbSetType; set => this.dbSetType = value;
        }

        /// <summary>
        /// Получает или устанавливает текущий продукт.
        /// </summary>
        public ProductsView CurrentProduct
        {
            get => this.currentProduct; set => this.currentProduct = value;
        }
        /// <summary>
        /// Используется при создании объекта
        /// </summary>
        public VendorCodes CurrentVendorCode
        {
            set; get;
        }
        #endregion

        #region Методы
        /// <summary>
        /// Получает путь до текущего каталога с продуктом.
        /// </summary>
        /// <param name="product">Продукт</param>
        /// <returns>Путь к каталогу</returns>
        /// <exception cref="Exception">Выбрасывает исключение, если продукт равен null.</exception>
        private string GetPath(ProductsView product)
        {
            String result = String.Empty;
            if (product != null)
            {

                result = String.Format("{0}\u2192{1}\u2192{2}",
                    product.Наименование_подгруппы == null ? String.Empty : product.Наименование_подгруппы,
                    product.Наименование_группы == null ? String.Empty : product.Наименование_группы,
                    product.Наименование_класса == null ? String.Empty : product.Наименование_класса
                    );
            }
            return result;
        }

        /// <summary>
        /// Обновляет названия полей.
        /// </summary>
        private void UpdateFieldsTitles()
        {
            foreach (object childControl in this.gridFields.Children)
            {
                if (childControl.GetType() == typeof(LabeledTextBox))
                {
                    var control = childControl as LabeledTextBox;
                    Fields field = this.DB.Fields.FirstOrDefault(x => x.Descriptors.title.ToLower() == control.Name.Replace("txb", "").ToLower());
                    if (field != null)
                    {
                        control.Title = field.titleRus;
                    }
                }
                if (childControl.GetType() == typeof(LabeledComboBox))
                {
                    var control = childControl as LabeledComboBox;
                    Fields field = this.DB.Fields.FirstOrDefault(x => x.Descriptors.title.ToLower() == control.Name.Replace("cmb", "").ToLower());
                    if (field != null)
                    {
                        control.Title = field.titleRus;
                    }
                }
                if (childControl.GetType() == typeof(LabeledCheckBox))
                {
                    var control = childControl as LabeledCheckBox;
                    Fields field = this.DB.Fields.FirstOrDefault(x => x.Descriptors.title.ToLower() == control.Name.Replace("chb", "").ToLower());
                    if (field != null)
                    {
                        control.Title = field.titleRus;
                    }
                }
            }
        }

        /// <summary>
        /// Асинхронно загружает изображение.
        /// </summary>
        /// <param name="code">Код изображения</param>
        /// <returns>Путь к загруженному изображению</returns>
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
        /// Обновляет изображение продукта.
        /// </summary>
        /// <param name="productsView">Представление продукта</param>
        private async Task UpdateImageAsync(ProductsView productsView)
        {
            try
            {
                if (productsView == null)
                    return;

                var cloudPaths = new List<string>()
    {
        productsView.Артикул,
        productsView.Код_подгруппы,
        productsView.Код_группы,
        productsView.Код_класса,
        //Изображение заглушка 0000.png
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
        /// Обновляет выбор единицы продукции.
        /// </summary>
        private void UpdateUnitSelection()
        {
            var rowView = this.dgDimensions.SelectedItem as DataRowView;
            if (rowView != null)
            {
                // Для получения значения определенного столбца используйте:
                int idProduct = Convert.ToInt32(rowView["ID товара"]);
                int idUnitProduct = Convert.ToInt32(rowView["ID связи продукт-измерение"]);
                this.currentUnit = this.DB.UnitsProducts.FirstOrDefault(x => x.idProduct == idProduct && x.id == idUnitProduct);
                this.DB.Entry(this.currentUnit).Reload();
            }
        }

        /// <summary>
        /// Обновляет выбор файла.
        /// </summary>
        private void UpdateFileSelection()
        {
            this.currentResource = this.dgFiles.SelectedItem as ResourcesViewProducts;
        }

        /// <summary>
        /// Фильтрует поля в зависимости от имени поля.
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        private void FilterFields(string fieldName)
        {
            if (this.gridFields == null)
                return;

            #region Показать элементы, если поле пустое
            if (string.IsNullOrEmpty(fieldName))
            {
                foreach (var control in this.gridFields.Children.OfType<UserControl>())
                {
                    control.Visibility = Visibility.Visible;
                }
            }
            #endregion

            #region Скрыть элементы, если поле не пустое
            else
            {
                var typesToCheck = new Type[]
                {
            typeof(LabeledTextBox),
            typeof(LabeledComboBox),
            typeof(LabeledTextBoxAndComboBox),
            typeof(LabeledCheckBox),
            typeof(PagedDataGrid),
            typeof(LabeledTextBoxAndButton)
                };

                foreach (var childControl in this.gridFields.Children)
                {
                    if (typesToCheck.Contains(childControl.GetType()))
                    {
                        var control = childControl as dynamic;
                        if (control != null && !control.Title.ToLower().Contains(fieldName.ToLower()))
                        {
                            control.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
            #endregion
        }


        /// <summary>
        /// Сохраняет изменения, внесенные в продукт.
        /// </summary>
        /// <param name="idProduct">ID продукта</param>
        private void Save(int? idProduct)
        {
            Products product = null;
            #region Получаем из БД или создаем новый продукт
            if (idProduct != null)
            {
                product = this.DB.Products.FirstOrDefault(x => x.id == idProduct);
            }
            #endregion
            if (product != null)
            {
                #region Дескриптор
                product.Descriptors.title = this.txbTitle.Text;
                product.Descriptors.titleShort = this.txbTitleShort.Text;
                product.Descriptors.description = this.txbDescription.Text;
                product.Descriptors.code = this.txbVendorCode.Text;
                #endregion
                #region Артикул
                ProductsVendorCodes productVendorCode = this.DB.ProductsVendorCodes.FirstOrDefault(x => x.idProduct == product.id);
                if (productVendorCode != null)
                {
                    VendorCodes vendorCode = this.DB.VendorCodes.FirstOrDefault(x => x.id == productVendorCode.idCode);
                    if (vendorCode != null)
                    {
                        vendorCode.Descriptors.title = this.txbVendorCode.Text;
                    }
                }
                else
                {
                    //var descriptor = new Descriptors(this.txbVendorCode.Text, this.txbVendorCode.Text, null, null);
                    //this.CustomDb.Descriptors.CreateClass(descriptor);
                    //var vendorCode = new VendorCodes(descriptor);
                    //this.CustomDb.VendorCodes.CreateClass(vendorCode);
                    //productVendorCode = new ProductsVendorCodes(product, vendorCode);
                    //this.CustomDb.ProductsVendorCodes.CreateClass(productVendorCode);
                    //productVendorCode = new ProductsVendorCodes(product, this.CurrentVendorCode);
                    productVendorCode = new ProductsVendorCodes();
                    productVendorCode.idProduct = product.id;
                    if (this.CurrentVendorCode == null)
                    {
                        throw new Exception("Не указан Артикул!");
                    }
                    productVendorCode.idCode = this.CurrentVendorCode.id;
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

        public void UpdateDimensionsGrid(Products product)
        {
            if (product != null)
            {
                var productUnitsViewCustoms = this.CustomBase.CustomDb.ExecuteSqlQuery<ProductUnitsViewCustom>(
                    $"SELECT * FROM ProductUnitsView where [ID продукта] = {product.id}");
                ObservableCollection<Object> ocProductUnitsViewCustoms = new ObservableCollection<Object>(productUnitsViewCustoms);

                this.dgDimensions.TableData =
                    new TableData(
                        typeof(ProductUnitsView).GetType().Name,
                        typeof(ProductUnitsView).GetType().Name,
                        typeof(ProductUnitsView),
                        typeof(ProductUnitsView).GetProperties().Select(p => p.Name).ToList(),
                         ocProductUnitsViewCustoms
                        );
                this.dgDimensions.TableData.DisplayColumnNames = new List<string>()
                        {
                            "Наименование типа единицы измерения",
                            "Значение",
                            "Сокращенное наименование единицы измерения"
                        };
                this.dgDimensions.Update();
            }
        }

        public void UpdateFilesGrid(Products product)
        {
            if (product != null)
            {
                var resourcesViewProducts = this.CustomBase.CustomDb.ExecuteSqlQuery<ResourcesViewProductsCustom>(
                    $"SELECT * FROM ResourcesViewProducts where [ID продукта] = {product.id}");
                ObservableCollection<Object> ocResourcesViewProducts = new ObservableCollection<Object>(resourcesViewProducts);
                this.dgFiles.TableData =
                   new TableData(
                       typeof(ResourcesViewProductsCustom).GetType().Name,
                       typeof(ResourcesViewProductsCustom).GetType().Name,
                       typeof(ResourcesViewProductsCustom),
                       typeof(ResourcesViewProductsCustom).GetProperties().Select(p => p.Name).ToList(),
                        ocResourcesViewProducts
                       );

                this.dgFiles.TableData.DisplayColumnNames = new List<string>()
                {
                    "URL ресурса",
                    "Наименование ресурса",
                    "Наименование типа ресурса",
                    "Расширение ресурса"
                };
                this.dgFiles.Update();
            }
        }

        public override void UpdateFields(List<CustomEventArgs> args)
        {
            try
            {
                this.UpdateFieldsTitles();
                Products product = this.CustomBase.UnpackCurrentObject<Products>(this.CurrentObject);
                if (product != null)
                {
                    if (this.CustomBase.Mode == EditModes.Update)
                    {
                        ProductsView productView = this.CustomBase.CustomDb.DB.ProductsView.FirstOrDefault(x => x.ID_продукта == product.id);
                        if (productView != null)
                        {
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
                            this.txbVendorCode.Text = productView.Артикул;
                            #endregion
                            #region Бухгалтерский код
                            this.txbCodeAccountant.IsEnabled = this.CustomBase.Mode == EditModes.Create;
                            this.txbCodeAccountant.Text = productView.Бухгалтерский_код;
                            #endregion
                            #region Короткое название
                            this.txbTitleShort.Text = productView.Сокращенное_наименование_продукта;
                            #endregion
                            #region Нормы
                            this.cmbNorm.Select(product.idNorm);
                            #endregion
                            #region Подгруппа
                            this.cmbSubGroup.Select(product.idSubGroup);
                            #endregion
                            #region Покрытия
                            this.cmbCover.Select(product.idCover);
                            #endregion
                            #region Материалы
                            this.cmbMaterial.Select(product.idMaterial);
                            #endregion
                            #region Упаковки
                            this.cmbPackage.Select(product.idPackage);
                            #endregion
                            #region Перфорации
                            this.cmbPerforation.Select(product.idPerforation);
                            #endregion
                            #region Отметка "На складе"
                            this.CustomBase.UpdateCheckBox(this.chbInStock, "На складе", "Под заказ", product.isInStock);
                            #endregion
                            #region Таблица измерений
                            this.UpdateDimensionsGrid(product);
                            #endregion
                            #region Таблица файлов
                            this.UpdateFilesGrid(product);
                            #endregion
                        }
                    }
                    if (this.CustomBase.Mode == EditModes.Create)
                    {
                        //this.txbVendorCode.IsEnabled = false;
                        //this.txbVendorCode.Error = "Вы сможете указать код после сохранения товара";
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        public override void UpdateForm(List<CustomEventArgs> args)
        {
            this.InitializeComponent();
            this.txbVendorCode.TitleButton = Common.EditModesDescriptions.ActionDescriptions[this.CustomBase.Mode];
            this.dgDimensions.Title = Common.EntityRussianNames.NamesNominative[typeof(Units)];
            this.dgFiles.Title = Common.EntityRussianNames.NamesNominative[typeof(Resources)];

            this.txbVendorCode.IsEnabled = true;
            this.txbVendorCode.IsEnableTextBox = false;

            this.CustomBase.UpdateOkButton(this.btnOk);
            this.CustomBase.UpdateComboBox(this.cmbNorm, this.CustomBase.ToList(this.CustomBase.CustomDb.DB.Norms.ToList()));
            this.CustomBase.UpdateComboBox(this.cmbSubGroup, this.CustomBase.ToList(this.CustomBase.CustomDb.DB.SubGroups.ToList()));
            this.CustomBase.UpdateComboBox(this.cmbCover, this.CustomBase.ToList(this.CustomBase.CustomDb.DB.Covers.ToList()));
            this.CustomBase.UpdateComboBox(this.cmbMaterial, this.CustomBase.ToList(this.CustomBase.CustomDb.DB.Materials.ToList()));
            this.CustomBase.UpdateComboBox(this.cmbPackage, this.CustomBase.ToList(this.CustomBase.CustomDb.DB.Packages.ToList()));
            this.CustomBase.UpdateComboBox(this.cmbPerforation, this.CustomBase.ToList(this.CustomBase.CustomDb.DB.Perforations.ToList()));

            this.cmbCover.SelectFirst();
            this.cmbMaterial.SelectFirst();
            this.cmbNorm.SelectFirst();
            this.cmbSubGroup.SelectFirst();
            this.cmbMaterial.SelectFirst();
            this.cmbPackage.SelectFirst();
            this.cmbPerforation.SelectFirst();
        }

        public override object HandleOk(List<CustomEventArgs> args)
        {
            Products product = this.CustomBase.UnpackCurrentObject<Products>(this.CurrentObject);
            if (product != null)
            {
                if (this.CustomBase.Mode == EditModes.Create)
                {
                    this.CustomBase.CustomDb.CreateProduct(
                         this.txbTitle.Text,
                         this.txbTitleShort.Text,
                         this.txbDescription.Text,
                         this.txbVendorCode.Text,
                         (int)this.cmbNorm.SelectedId,
                         (int)this.cmbSubGroup.SelectedId,
                         (int)this.cmbCover.SelectedId,
                         (int)this.cmbPerforation.SelectedId,
                         (int)this.cmbPackage.SelectedId,
                         (int)this.cmbPackage.SelectedId,
                         this.chbInStock.IsChecked ?? false
                        );
                }
                if (this.CustomBase.Mode == EditModes.Update)
                {
                    this.CustomBase.CustomDb.UpdateProduct(
                        product.id,
                        this.txbTitle.Text,
                        this.txbTitleShort.Text,
                        this.txbDescription.Text,
                        this.txbVendorCode.Text,
                        (int)this.cmbNorm.SelectedId,
                        (int)this.cmbSubGroup.SelectedId,
                        (int)this.cmbCover.SelectedId,
                        (int)this.cmbPerforation.SelectedId,
                        (int)this.cmbPackage.SelectedId,
                        (int)this.cmbPackage.SelectedId,
                        this.chbInStock.IsChecked ?? false
                        );
                }
                this.CustomBase.Result.Data = true;
                return true;
            }
            return false;
        }

        public override object HandleCancel(List<CustomEventArgs> args)
        {
            return false;
        }

        public void HandleUpdateVendorCode()
        {
            if (this.CustomBase.Mode == EditModes.Create || this.CustomBase.Mode == EditModes.Update)
            {
                Products product = this.CustomBase.UnpackCurrentObject<Products>(this.CurrentObject);
                if (product != null)
                {
                    ProductsVendorCodes productVendorCode = this.DB.ProductsVendorCodes.FirstOrDefault(x => x.idProduct == product.id);
                    this.CustomBase.Mode = EditModes.Update;
                    this.CustomBase.AddCurrentObject(new CustomEventArgs(productVendorCode));
                    var windowEdit = new WindowEdit(
                   Common.Strings.Titles.Windows.add,
                   this.CustomBase,
                   Common.WindowSizes.SmallH320W400.Width,
                   Common.WindowSizes.SmallH320W400.Height
                   );
                    windowEdit.ShowDialog();
                }
            }
        }
        #endregion

        #region Конструкторы/Деструкторы
        /// <summary>
        /// Создаёт страницу с данным товаром
        /// </summary>
        /// <param name="product">
        /// Здесь ожидается либо ProductsView - представление товара
        /// </param>
        public PageEditProduct(CustomBase customBase, int width = 600, int height = 800) : base(customBase)
        {
            this.InitializeComponent();
            this.SetSize(width, height);
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void CustomPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        private void imgPreview_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _ = this.UpdateImageAsync(this.CurrentProduct);
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
            //            this.txbCmbCurrentUnit.Title = currentUnitProduct.UnitsTypes.Title;
            //            this.txbCmbCurrentUnit.Select(currentUnitProduct.idUnit);
            //            this.txbCmbCurrentUnit.Text = currentUnitProduct.value.ToString();
            //        }
            //    }
            //}
        }
        private void btnAddDimension_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var productUnitsWindow = new ProductUnitsWindow((int)this.IdProduct);
            productUnitsWindow.ShowDialog();
            this.UpdateForm(this.CustomBase.CurrentObjects);
        }
        private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ProcessOk();
        }
        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ProcessCancel();
        }
        private void txbVendorCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.HandleUpdateVendorCode();
                this.UpdateFields(this.CustomBase.CurrentObjects);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        #region Контекстное меню таблиц

        #region Измерения
        private void dgDimensions_RightClickSelectedCellChanged(object sender, EventArgs e)
        {

        }

        private void dgDimensions_LeftClickSelectedCellChanged(object sender, EventArgs e)
        {

        }

        private void dgDimensions_AddMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                Products product = this.CustomBase.UnpackCurrentObject<Products>(this.CurrentObject);
                if (product != null)
                {
                    UnitsProducts unitsProducts = new UnitsProducts();
                    unitsProducts.idProduct = product.id;
                    unitsProducts.value = 0;

                    this.CustomBase.AddCurrentObject(new CustomEventArgs(unitsProducts));
                    this.CustomBase.Mode = EditModes.Create;
                    var windowEdit = new WindowEdit(
                       Common.Strings.Titles.Windows.add,
                       this.CustomBase,
                       Common.WindowSizes.SmallH320W400.Width,
                       Common.WindowSizes.SmallH320W400.Height
                    );
                    _ = windowEdit.ShowDialog();
                    if ((bool)windowEdit.DialogResult)
                    {
                        this.UpdateDimensionsGrid(product);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void dgDimensions_EditMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                Products product = this.CustomBase.UnpackCurrentObject<Products>(this.CurrentObject);
                if (product != null)
                {
                    var selectedUnit = this.dgDimensions.SelectedItem as ProductUnitsViewCustom;
                    UnitsProducts unitsProducts = new UnitsProducts();
                    unitsProducts.id = selectedUnit.ID_связи_продуктИзмерение;
                    unitsProducts.idProduct = selectedUnit.ID_продукта;
                    unitsProducts.idType = selectedUnit.ID_типа_измерения;
                    unitsProducts.idUnit = selectedUnit.ID_единицы_измерения;
                    unitsProducts.value = selectedUnit.Значение;

                    this.CustomBase.AddCurrentObject(new CustomEventArgs(unitsProducts));
                    this.CustomBase.Mode = EditModes.Update;
                    var windowEdit = new WindowEdit(
                       Common.Strings.Titles.Windows.add,
                       this.CustomBase,
                       Common.WindowSizes.SmallH320W400.Width,
                       Common.WindowSizes.SmallH320W400.Height
                    );
                    _ = windowEdit.ShowDialog();
                    if ((bool)windowEdit.DialogResult)
                    {
                        this.UpdateDimensionsGrid(product);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void dgDimensions_DeleteMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                Products product = this.CustomBase.UnpackCurrentObject<Products>(this.CurrentObject);
                if (product != null)
                {
                    var selectedUnit = this.dgDimensions.SelectedItem as ProductUnitsViewCustom;
                    this.CustomBase.CustomDb.DeleteUnitProduct(selectedUnit.ID_связи_продуктИзмерение);
                    this.UpdateDimensionsGrid(product);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }

        }

        private void dgDimensions_RefreshMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                Products product = this.CustomBase.UnpackCurrentObject<Products>(this.CurrentObject);
                if (product != null)
                {
                    this.UpdateDimensionsGrid(product);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        #endregion

        #region Файлы
        private void dgFiles_RightClickSelectedCellChanged(object sender, EventArgs e)
        {

        }

        private void dgFiles_LeftClickSelectedCellChanged(object sender, EventArgs e)
        {

        }

        private void dgFiles_AddMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                Products product = this.CustomBase.UnpackCurrentObject<Products>(this.CurrentObject);
                if (product != null)
                {
                    ResourcesViewProducts resourcesViewProducts = new ResourcesViewProducts();
                    resourcesViewProducts.ID_продукта = product.id;
                    this.CustomBase.AddCurrentObject(new CustomEventArgs(resourcesViewProducts));
                    this.CustomBase.Mode = EditModes.Create;
                    var windowEdit = new WindowEdit(
                       Common.Strings.Titles.Windows.add,
                       this.CustomBase,
                       Common.WindowSizes.SmallH320W400.Width,
                       Common.WindowSizes.SmallH320W400.Height
                    );
                    _ = windowEdit.ShowDialog();
                    if ((bool)windowEdit.DialogResult)
                    {
                        this.UpdateFilesGrid(product);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void dgFiles_EditMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                Products product = this.CustomBase.UnpackCurrentObject<Products>(this.CurrentObject);
                var selectedItem = this.dgFiles.SelectedItem;
                if (selectedItem != null)
                {
                    ResourcesViewProductsCustom selectedItemCustom = selectedItem as ResourcesViewProductsCustom;
                    ResourcesViewProducts resourcesViewProducts = this.CustomBase.CustomDb.DB.ResourcesViewProducts.FirstOrDefault(
                        x => x.ID_ресурса == selectedItemCustom.ID_ресурса);
                    this.CustomBase.AddCurrentObject(new CustomEventArgs(resourcesViewProducts));
                    this.CustomBase.Mode = EditModes.Update;
                    var windowEdit = new WindowEdit(
                       Common.Strings.Titles.Windows.add,
                       this.CustomBase,
                       Common.WindowSizes.SmallH320W400.Width,
                       Common.WindowSizes.SmallH320W400.Height
                    );
                    _ = windowEdit.ShowDialog();
                    if ((bool)windowEdit.DialogResult)
                    {
                        this.UpdateFilesGrid(product);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void dgFiles_DeleteMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                Products product = this.CustomBase.UnpackCurrentObject<Products>(this.CurrentObject);
                var selectedItem = this.dgFiles.SelectedItem;
                if (product != null && selectedItem != null)
                {
                    ResourcesViewProductsCustom selectedItemCustom = selectedItem as ResourcesViewProductsCustom;
                    this.CustomBase.WDClient.DeleteFile(selectedItemCustom.URL_ресурса);
                    this.CustomBase.CustomDb.DeleteResource(product.id);
                    this.UpdateFilesGrid(product);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void dgFiles_RefreshMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                Products product = this.CustomBase.UnpackCurrentObject<Products>(this.CurrentObject);
                if (product != null)
                {
                    this.UpdateFilesGrid(product);
                }
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
