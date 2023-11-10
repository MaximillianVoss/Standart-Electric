using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Xunit;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Облачный_Клиент.Тесты
{
    public class TestsWDClient
    {
        /// <summary>
        /// webDAV-клиент
        /// </summary>
        public WDClient WDClient { set; get; }
        /// <summary>
        /// База данных
        /// </summary>
        public DBSEEntities DB { set; get; }
        private void Init()
        {
            this.WDClient = new WDClient(
                "devstor",
                "TE6db?lZE~8Ixc?KAtQW",
                "https://rcloud.rsk-gr.ru",
                "/remote.php/dav/files/devstor"
                );
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
                await this.WDClient.DownloadFile(localImagePath, cloudImagePath);
            }
            else
            {
                return null;
            }
            #endregion
            return localImagePath;
        }
        private async Task UpdateImageAsync(ProductsView productView)
        {
            if (productView != null)
            {
                string localImagePath = null;
                string cloudImagePath = String.Format("{0}/{1}.png", Common.Strings.Path.Cloud.images, productView.Артикул);
                bool isExists = await this.WDClient.IsFileExists(cloudImagePath);
                #region Загрузка файла
                if (isExists)
                #region Загружаем собственное изображение
                {
                    localImagePath = await this.DownloadImageAsync(productView.Артикул);
                }
                #endregion
                else
                #region Загружаем изображение подгруппы
                {
                    cloudImagePath = String.Format("{0}/{1}.png", Common.Strings.Path.Cloud.images, productView.Код_подгруппы);
                    isExists = await this.WDClient.IsFileExists(cloudImagePath);
                    if (isExists)
                    {
                        localImagePath = await this.DownloadImageAsync(productView.Код_подгруппы);
                        return;
                    }
                    cloudImagePath = String.Format("{0}/{1}.png", Common.Strings.Path.Cloud.images, productView.Код_группы);
                    isExists = await this.WDClient.IsFileExists(cloudImagePath);
                    if (isExists)
                    {
                        localImagePath = await this.DownloadImageAsync(productView.Код_группы);
                        return;
                    }
                    cloudImagePath = String.Format("{0}/{1}.png", Common.Strings.Path.Cloud.images, productView.Код_класса);
                    isExists = await this.WDClient.IsFileExists(cloudImagePath);
                    if (isExists)
                    {
                        localImagePath = await this.DownloadImageAsync(productView.Код_класса);
                        return;
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
                    //this.imgPreview.Source = bitmap;
                }
                #endregion
            }
        }

        [Fact, Trait("Category", "Integration")]
        public void TestUpdateImage()
        {
            //ProductsView productView = this.CustomDb.ProductsView.FirstOrDefault(x => x.ID_продукта == 1);
            //if (productView != null)
            //   await this.UpdateImageAsync(productView);
        }
        [Fact]
        public void TestDownloadImage()
        {
            this.Init();
            _ = this.WDClient.DownloadFile("C:\\Users\\FossW\\OneDrive\\Рабочий стол\\WDTests\\test.png", "Resources/images/1234.png");
        }

    }
}
