using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using WebDAVClient;
using WebDAVClient.Model;

namespace База_артикулов.Классы
{
    public class WDClient
    {

        #region Поля

        #endregion

        #region Свойства
        public Client Client { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public string Server { set; get; }
        public string BasePath { set; get; }
        #endregion

        #region Методы
        public async Task ShowDirectoryInTreeViewAsync(TreeView treeView)
        {
            if (treeView == null)
            {
                throw new Exception(Common.Strings.Errors.emptyObject);
            }
            treeView.Items.Clear();
            // Получение списка файлов и папок
            var items = await this.GetFilesList();
            // Создание корневого узла
            var rootNode = new TreeViewItem { Header = System.IO.Path.GetFileName(this.BasePath.TrimEnd('/')) };
            treeView.Items.Add(rootNode);
            // Заполнение TreeView
            this.ShowDirectoryInTreeView(this.Client, items, rootNode);

        }
        private async void ShowDirectoryInTreeView(Client client, IEnumerable<Item> items, TreeViewItem parentItem)
        {
            foreach (var item in items)
            {
                // Создание узла для элемента
                var itemNode = new TreeViewItem { Header = item.DisplayName };
                parentItem.Items.Add(itemNode);
                // Если элемент является папкой, рекурсивно получаем ее содержимое и добавляем его к узлу
                if (item.IsCollection)
                {
                    var folderFiles = await client.List(item.Href);
                    this.ShowDirectoryInTreeView(client, folderFiles, itemNode);
                }
            }
        }
        public Task<IEnumerable<Item>> GetFilesList(string path = "/")
        {
            return this.Client.List(path);
        }
        public async Task<bool> IsFileExists(string cloudFilePath)
        {
            //try
            //{
            //    var file = await this.Client.GetFile(cloudFilePath);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
            var folderPath = Path.GetDirectoryName(cloudFilePath);
            var fileName = Path.GetFileName(cloudFilePath);
            var items = await this.Client.List(folderPath);

            foreach (var item in items)
            {
                if (item.DisplayName == fileName)
                {
                    return true;
                }
            }

            return false;
        }
        public void DownloadFile(string saveFilePath, string cloudFilePath)
        {
            Item file = this.Client.GetFile(cloudFilePath).Result;
            this.DownloadFile(saveFilePath, file);
        }
        public void DownloadFile(string saveFilePath, Item file)
        {
            //using (var tempFile = File.OpenWrite(saveFilePath))
            //using (var stream = await this.Client.Download(file.Href))
            //{
            //    await stream.CopyToAsync(tempFile);
            //    stream.Dispose();
            //    stream.Close();
            //}
            if (File.Exists(saveFilePath))
            {
                using (var fileStream = new FileStream(saveFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fileStream.Close();
                }
            }

            using (var tempFile = File.OpenWrite(saveFilePath))
            using (var stream = this.Client.Download(file.Href).Result)
            {
                stream.CopyToAsync(tempFile).Wait();
                stream.Dispose();
                stream.Close();
            }

        }
        public async void UploadFile(string openFilePath, string cloudFolderPath)
        {
            Item folder = await this.Client.GetFolder(cloudFolderPath);
            await this.UploadFile(openFilePath, folder);
            // Получение папки на сервере
            //Item file = this.Client.GetFolder(cloudFolderPath).Result;
            // Загрузка файла на сервер
            //this.UploadFile(openFilePath, file).Wait();
        }
        public async Task UploadFile(string openFilePath, Item folder)
        {
            if (!folder.IsCollection)
            {
                throw new Exception(Common.Strings.Errors.notFolder);
            }
            using (var fileStream = new FileStream(openFilePath, FileMode.Open, FileAccess.Read))
            {
                // Отправка файла на сервер
                await this.Client.Upload(folder.Href, fileStream, System.IO.Path.GetFileName(openFilePath));
            }
        }
        public async void DeleteFile(string filePath)
        {
            var file = await this.Client.GetFile(filePath);
            await this.DeleteFile(file);
        }
        public async Task DeleteFile(Item file)
        {
            // Удаление файла
            await this.Client.DeleteFile(file.Href);
        }
        #endregion

        #region Конструкторы/Деструкторы
        public WDClient(string userName, string password, string server, string basePath)
        {
            this.UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            this.Password = password ?? throw new ArgumentNullException(nameof(password));
            this.Server = server ?? throw new ArgumentNullException(nameof(server));
            this.BasePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
            this.Client = new Client(new NetworkCredential
            {
                UserName = userName,
                Password = password
            })
            {
                Server = server,
                BasePath = basePath
            };
        }

        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion


    }
}
