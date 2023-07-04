using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using База_артикулов.Классы;

namespace База_артикулов.Формы
{
    /// <summary>
    /// Логика взаимодействия для FilesWindow.xaml
    /// </summary>
    public partial class FilesWindow : CustomWindow
    {


        #region Поля

        #endregion

        #region Свойства
        /// <summary>
        /// Улучшенный WebDAV-клиент
        /// </summary>
        private WDClient WDClient { get; set; }
        #endregion

        #region Методы


        private async Task InitTreeView()
        {
            this.WDClient.ShowDirectoryInTreeViewAsync(this.tvFiles);
        }

        private void TestDownload(string filePath)
        {
            //Создание диалогового окна для выбора места сохранения
            var saveFileDialog = new SaveFileDialog
            {
                DefaultExt = Path.GetExtension(filePath),
                Filter = "All files (*.*)|*.*"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                this.WDClient.DownloadFile(saveFileDialog.FileName, filePath);
            }
        }

        private void TestUpload(string folderPath)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "All files (*.*)|*.*"
            };
            // Открытие диалогового окна и отправка файла на сервер
            if (openFileDialog.ShowDialog() == true)
            {
                var fileName = openFileDialog.FileName;
                this.WDClient.UploadFile(fileName, folderPath);
            }
        }

        private void TestDelete(string filePath)
        {
            this.WDClient.DeleteFile(filePath);
        }

        #endregion

        #region Конструкторы/Деструкторы
        public FilesWindow()
        {
            this.InitializeComponent();

        }


        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void CustomWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.InitClient();
                this.InitTreeView();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnUpload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.TestUpload(this.txbFilePath.Text);
                this.InitTreeView();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void btnDownload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.TestDownload(this.txbFilePath.Text);
                this.InitTreeView();
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
                this.TestDelete(this.txbFilePath.Text);
                this.InitTreeView();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
    }
    #endregion


}