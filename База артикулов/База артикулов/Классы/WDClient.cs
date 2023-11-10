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
        public async Task DownloadFile(string saveFilePath, string cloudFilePath)
        {
            Item file = await this.Client.GetFile(cloudFilePath).ConfigureAwait(false);
            await this.DownloadFile(saveFilePath, file).ConfigureAwait(false);
        }
        public async Task DownloadFile(string saveFilePath, Item file)
        {
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }

            using (var tempFile = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var stream = await this.Client.Download(file.Href).ConfigureAwait(false))
            {
                await stream.CopyToAsync(tempFile).ConfigureAwait(false);
            }
        }
        public async Task UploadFile(string openFilePath, string cloudFolderPath)
        {
            Item folder = await this.Client.GetFolder(cloudFolderPath).ConfigureAwait(false);
            await this.UploadFile(openFilePath, folder).ConfigureAwait(false);
        }
        public async Task UploadFile(string openFilePath, Item folder)
        {
            //if (await IsFileExists(folder.Href + Path.GetFileName(openFilePath)))
            //    return;
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

        public string GetFileUrl(string cloudFolder, string fileName)
        {
            return $"{this.Server}{this.BasePath}/{cloudFolder}/{fileName}";
        }
        public async Task CreateDirectoryIfNotExistsAsync(string cloudPath, string folderName)
        {
            string fullPath = $"{this.BasePath}{cloudPath}/{folderName}";
            try
            {
                // Пытаемся получить папку
                var folder = await this.Client.GetFolder(fullPath);
            }
            catch
            {
                // Если возникает исключение (предполагаем, что из-за отсутствия папки), создаем ее
                await this.Client.CreateDir($"{this.BasePath}{cloudPath}", folderName);
            }
        }
        #endregion

        #region Конструкторы/Деструкторы

        /// <summary>
        /// Конструктор по умолчанию, создающий WDClient с настройками по умолчанию.
        /// </summary>
        public WDClient()
        {
            // Необходимо указать фактические значения по умолчанию для userName, password, server, и basePath.
            // Эти значения являются лишь местозаполнителями.
            this.UserName = "defaultUserName"; // Замените на реальное значение по умолчанию.
            this.Password = "defaultPassword"; // Замените на реальное значение по умолчанию.
            this.Server = "defaultServer"; // Замените на реальное значение по умолчанию.
            this.BasePath = "defaultBasePath"; // Замените на реальное значение по умолчанию.

            this.Client = new Client(new NetworkCredential
            {
                UserName = this.UserName,
                Password = this.Password
            })
            {
                Server = this.Server,
                BasePath = this.BasePath
            };
        }

        /// <summary>
        /// Конструктор, инициализирующий WDClient с использованием предоставленных учетных данных и адреса сервера.
        /// </summary>
        /// <param name="userName">Имя пользователя для доступа к серверу.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <param name="server">Адрес сервера.</param>
        /// <param name="basePath">Базовый путь на сервере.</param>
        /// <exception cref="ArgumentNullException">Бросается, если любой из параметров null.</exception>
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

        /// <summary>
        /// Конструктор, инициализирующий WDClient с использованием настроек из экземпляра SettingsNew.
        /// </summary>
        /// <param name="settings">Настройки для инициализации клиента.</param>
        /// <exception cref="ArgumentNullException">Бросается, если параметр settings является null.</exception>
        public WDClient(SettingsNew settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.UserName = settings.UserNameWDClient ?? throw new ArgumentNullException(nameof(settings.UserNameWDClient));
            this.Password = settings.PasswordWDClient ?? throw new ArgumentNullException(nameof(settings.PasswordWDClient));
            this.Server = settings.ServerWDClient ?? throw new ArgumentNullException(nameof(settings.ServerWDClient));
            this.BasePath = settings.BasePathWDClient ?? throw new ArgumentNullException(nameof(settings.BasePathWDClient));

            this.Client = new Client(new NetworkCredential
            {
                UserName = this.UserName,
                Password = this.Password
            })
            {
                Server = this.Server,
                BasePath = this.BasePath
            };
        }

        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion


    }
}
