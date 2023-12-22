using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using WebDAVClient.Model;
using База_артикулов.Классы;
using База_артикулов.Модели;


namespace База_артикулов.Формы.Страницы.Редактирование
{
    /// <summary>
    /// Логика взаимодействия для PageEditResource.xaml
    /// </summary>
    public partial class PageEditResource : CustomPage
    {
        //Из-за сложности БД лучше передавать в форму объект ResourcesViewProducts

        #region Поля

        #endregion

        #region Свойства
        /// <summary>
        /// Загружал ли пользователь новый файл на облако
        /// </summary>
        bool IsCloudPathChaged
        {
            set; get;
        }
        #endregion

        #region Методы
        async Task<string> UploadFile(String filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            // Получаем расширение файла без точки (например, "jpg" вместо ".jpg")
            string extension = System.IO.Path.GetExtension(filePath)?.TrimStart('.');

            if (string.IsNullOrEmpty(extension))
                throw new InvalidOperationException("File does not have an extension.");

            // Создаем папку с расширением файла, если она ещё не существует
            await this.WDClient.CreateDirectoryIfNotExistsAsync("/Resources", extension);

            // Загружаем файл в созданную папку
            string cloudFolderPath = $"{this.WDClient.BasePath}/Resources/{extension}";
            var cloudFolder = new Item { Href = cloudFolderPath, IsCollection = true };  // Предположим, что у вас есть подходящий конструктор или метод для создания объекта папки
            //this.ShowMessage("Загрузка по пути " + cloudFolderPath);
            await this.WDClient.UploadFile(filePath, cloudFolder);
            this.IsCloudPathChaged = true;
            return this.WDClient.GetFileUrl("Resources/" + extension, System.IO.Path.GetFileName(filePath));
        }

        public override void UpdateFields(List<CustomEventArgs> args)
        {
            ResourcesViewProducts resourcesViewProducts = this.CustomBase.UnpackCurrentObject<ResourcesViewProducts>(this.CurrentObject);
            if (resourcesViewProducts != null)
            {
                if (this.CustomBase.Mode == EditModes.Update)
                {
                    this.lbltxbResourceTitle.Text = resourcesViewProducts.Наименование_ресурса;
                    this.lbltxbFilePath.Text = resourcesViewProducts.URL_ресурса;
                    this.lbltxbFilePath.Error = "Файл уже загружен в облако, вместо локального пути, отображен путь в облачном хранилище";
                }
                if (this.CustomBase.Mode == EditModes.Create)
                {

                }
            }

        }

        public override void UpdateForm(List<CustomEventArgs> args)
        {
            this.InitializeComponent();
            this.CustomBase.UpdateOkButton(this.btnOk);
        }

        private async Task HandleCreate(ResourcesViewProducts resourcesViewProducts)
        {
            if (resourcesViewProducts == null)
            {
                throw new Exception(Common.Strings.Errors.emptyObject);
            }
            #region Загрузка файла
            if (String.IsNullOrEmpty(this.lbltxbFilePath.Text))
                throw new Exception("Не указан путь до загружаемого файла!");
            string url = await this.UploadFile(this.lbltxbFilePath.Text);
            #endregion
            #region Создание/обновление данных
            this.CustomBase.CustomDb.CreateResource(
                resourcesViewProducts.ID_продукта,
                url,
                this.lbltxbFilePath.Text,
                this.lbltxbResourceTitle.Text
                );
            #endregion
        }

        private async Task HandleUpdate(ResourcesViewProducts resourcesViewProducts)
        {
            if (resourcesViewProducts == null)
            {
                throw new Exception(Common.Strings.Errors.emptyObject);
            }
            if (IsCloudPathChaged)
            {
                this.CustomBase.CustomDb.DeleteResource(resourcesViewProducts.ID_продукта);
                await this.HandleCreate(resourcesViewProducts);
            }
            else
            {
                this.CustomBase.CustomDb.UpdateResource(
                    resourcesViewProducts.ID_продукта,
                    this.lbltxbResourceTitle.Text
                    );
            }
        }

        public override object HandleOk(List<CustomEventArgs> args)
        {
            ResourcesViewProducts resourcesViewProducts = this.CustomBase.UnpackCurrentObject<ResourcesViewProducts>(this.CurrentObject);
            if (this.CustomBase.Mode == EditModes.Create)
            {
                _ = HandleCreate(resourcesViewProducts);
            }
            if (this.CustomBase.Mode == EditModes.Update)
            {
                _ = HandleUpdate(resourcesViewProducts);
            }
            this.CustomBase.Result.Data = true;
            return true;
        }

        public override object HandleCancel(List<CustomEventArgs> args)
        {
            return false;
        }
        #endregion

        #region Конструкторы/Деструкторы
        public PageEditResource(CustomBase customBase, int width = 600, int height = 800) : base(customBase)
        {
            this.SetSize(width, height);
            this.InitializeComponent();
            this.IsCloudPathChaged = false;
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

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileNames = this.GetLoadFilePath("Все файлы|*.*").ToList();
                if (fileNames.Count > 0)
                {
                    this.lbltxbFilePath.Text = fileNames[0];
                    string extension = System.IO.Path.GetExtension(this.lbltxbFilePath.Text);
                    ResourceTypesView typeView = this.DB.ResourceTypesView.FirstOrDefault(x => x.Расширение_ресурса == extension);
                    if (typeView != null)
                    {
                        this.lbltxbResourceTitle.Text = typeView.Наименование_типа_ресурса.ToString();
                    }
                    else
                    {
                        string fileName = System.IO.Path.GetFileName(this.lbltxbFilePath.Text);
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
