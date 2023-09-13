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
using WebDAVClient.Model;
using База_артикулов.Классы;
using База_артикулов.Модели;
using System.IO;


namespace База_артикулов.Формы.Страницы.Редактирование
{
    /// <summary>
    /// Логика взаимодействия для PageEditResource.xaml
    /// </summary>
    public partial class PageEditResource : CustomPage
    {


        #region Поля
        ResourcesViewProducts currentItem;
        #endregion

        #region Свойства

        #endregion

        #region Методы
        async Task<string> UploadFile(String filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            // Получаем расширение файла без точки (например, "jpg" вместо ".jpg")
            var extension = System.IO.Path.GetExtension(filePath)?.TrimStart('.');

            if (string.IsNullOrEmpty(extension))
                throw new InvalidOperationException("File does not have an extension.");

            // Создаем папку с расширением файла, если она ещё не существует
            await this.WDClient.CreateDirectoryIfNotExistsAsync("/Resources", extension);

            // Загружаем файл в созданную папку
            var cloudFolderPath = $"{this.WDClient.BasePath}/Resources/{extension}";
            var cloudFolder = new Item { Href = cloudFolderPath, IsCollection = true };  // Предположим, что у вас есть подходящий конструктор или метод для создания объекта папки
            await this.WDClient.UploadFile(filePath, cloudFolder);
            return this.WDClient.GetFileUrl(extension, System.IO.Path.GetFileName(filePath));
        }

        private async Task Save()
        {
            #region Загрузка файла
            if (String.IsNullOrEmpty(this.lbltxbFilePath.Text))
                throw new Exception("Не указан путь до загружаемого файла!");
            var url = await this.UploadFile(this.lbltxbFilePath.Text);
            #endregion

            #region Создание/обновление данных
            if (this.IsDescriptorProductExists(this.currentItem.ID_продукта))
            {
                this.ShowMessage("Дескриптор продукта обнаружен!");
                var extension = System.IO.Path.GetExtension(this.lbltxbFilePath.Text);
                var typeView = this.DB.ResourceTypesView.FirstOrDefault(x => x.Расширение_ресурса == extension);
                #region Если типа файла нет, добавляем
                if (typeView == null)
                {
                    ResourceTypes resourceTypes = new ResourceTypes();
                    resourceTypes.title = $"Файл с расширением {extension}";
                    this.lbltxbResourceTitle.Text = resourceTypes.title;
                    resourceTypes.extension_ = extension;
                    this.DB.ResourceTypes.Add(resourceTypes);
                }
                else
                {
                    this.lbltxbResourceTitle.Text = typeView.Наименование_типа_ресурса;
                }
                #endregion
                #region Если тип найден, продолжаем работу с ним

                if (!this.IsDescriptorProductExists(this.currentItem.ID_продукта))
                    throw new Exception("Не удалось найти дескриптор продукта!");
                var productDescriptor = this.GetDescriptorProduct(this.currentItem.ID_продукта);
                this.ShowMessage(productDescriptor.id.ToString());

                var productDescriptorResource = this.GetDescriptorsResources(productDescriptor.id);
                if (!this.IsDescriptorResourcesExists(productDescriptor.id))
                {
                    this.ShowMessage("Создание и привязка к ресурсам");
                    var desriptorResource = new DescriptorsResources();
                    desriptorResource.idDescriptor = productDescriptor.id;
                    productDescriptorResource = this.DB.DescriptorsResources.Add(desriptorResource);
                }
                Resources resource = new Resources();
                resource.URL = url;
                resource = this.DB.Resources.Add(resource);
                productDescriptorResource.title = this.lbltxbResourceTitle.Text;
                productDescriptorResource.idResource = resource.id;
                productDescriptorResource.Resources = resource;
                #endregion

            }
            else
            {

            }
            #endregion
            this.DB.SaveChanges();
        }

        void Update(ResourcesViewProducts item)
        {
            var descriptor = this.DB.ResourcesView.FirstOrDefault(x => x.ID_дескриптора_объекта == this.currentItem.ID_продукта);
            if (descriptor != null)
            {

            }
            else
            {

            }
        }
        #endregion

        #region Конструкторы/Деструкторы
        public PageEditResource(object item = null)
        {
            InitializeComponent();
            this.currentItem = (ResourcesViewProducts)item;
            this.btnOk.Text =
                this.currentItem != null ||
                this.DB.ResourcesViewProducts.FirstOrDefault(x => x.ID_ресурса == this.currentItem.ID_ресурса) == null ?
            Common.Strings.Titles.Controls.Buttons.saveChanges :
            Common.Strings.Titles.Controls.Buttons.createItem;
            this.Update(this.currentItem);
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ = this.Save();
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

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> fileNames = this.GetLoadFilePath("Все файлы|*.*").ToList();
                if (fileNames.Count > 0)
                {
                    this.lbltxbFilePath.Text = fileNames[0];
                    var extension = System.IO.Path.GetExtension(this.lbltxbFilePath.Text);
                    var typeView = this.DB.ResourceTypesView.FirstOrDefault(x => x.Расширение_ресурса == extension);
                    if (typeView != null)
                    {
                        this.lbltxbResourceTitle.Text = typeView.Наименование_типа_ресурса.ToString();
                    }
                    else
                    {
                        var fileName = System.IO.Path.GetFileName(this.lbltxbFilePath.Text);
                        this.lbltxbResourceTitle.Text = fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        #endregion

    }
}
