using ExcelReader.ExcelDocument;
using ExcelReader.Parser;
using Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using База_артикулов.Классы;
using Path = System.IO.Path;

namespace База_артикулов.Формы.Страницы
{
    /// <summary>
    /// Логика взаимодействия для PageImport.xaml
    /// </summary>
    public partial class PageImport : CustomPage
    {

        #region Поля
        private Log log;
        private ExcelParser excelParser;
        private bool isContainsHeaders = true;
        private bool isContainsDescription = true;
        #region Списки таблиц для начального заполнения БД
        private List<string> Таблицы_справочники = new List<string>()
        {
            "Tables.csv",
            "Views.csv",
            "Fields.csv",
            "Units.csv",
            "UnitsTypes.csv",
            "Classes.csv",
            "LoadDiagrams.csv",
            "Norms.csv",
            "Materials.csv",
            "Covers.csv",
            "Manufacturers.csv",
            "BuisnessUnits.csv",
            "Manufacturers.csv",
            "ResourceTypes.csv"
        };
        private List<string> Таблицы_справочники2 = new List<string>()
        {
            "Groups.csv",
            "SubGroups.csv",
            "Perforations.csv",
            "BuisnessUnits.csv",
            "VendorCodes.csv",
            "Applications.csv",
            "Packages.csv"
        };
        private List<string> Таблицы_связи = new List<string>()
        {
            "GroupsApplications.csv"
        };
        private List<string> Главные_таблицы = new List<string>()
        {
            "Products.csv",
            "ProductsVendorCodes.csv",
            "UnitsPerforations.csv",
            "UnitsPackages.csv",
            "UnitsProducts.csv"
        };
        #endregion
        #endregion

        #region Свойства
        /// <summary>
        /// Прогресс обработки файлов, работает с прогресс баром
        /// </summary>
        private double Progress
        {
            get => this.pbImportProgress.Value;
            set => this.pbImportProgress.Value = value;
        }
        /// <summary>
        /// Обработанное число строк в файлах для импорта
        /// </summary>
        private int CountCurrentRows { set; get; }
        /// <summary>
        /// Общее число строк в файлах для импорта
        /// </summary>
        private int CountTotalRows { set; get; }
        /// <summary>
        /// Парсер Excel
        /// </summary>
        private ExcelParser ExcelParser { get => this.excelParser; set => this.excelParser = value; }
        /// <summary>
        /// Лог для записи и вывода сообщений
        /// </summary>
        public Log Log { get => this.log; set => this.log = value; }
        /// <summary>
        /// Содержит ли таблица заголовки
        /// </summary>
        public bool IsContainsHeaders { get => this.isContainsHeaders; set => this.isContainsHeaders = value; }
        /// <summary>
        /// Содержит ли таблица строку описания заголовков - это вторая строка под заголовками
        /// </summary>
        public bool IsContainsDescription { get => this.isContainsDescription; set => this.isContainsDescription = value; }
        #endregion

        #region Методы

        #region Работа с логом
        /// <summary>
        /// Обновляет лог
        /// </summary>
        /// <returns></returns>
        private void UpdateLog()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.gvLog.DataContext = null;
                this.gvLog.DataContext = this.Log.ToTable().DefaultView;
                if (this.gvLog.Items.Count > 0)
                {
                    var border = VisualTreeHelper.GetChild(this.gvLog, 0) as Decorator;
                    if (border != null)
                    {
                        var scroll = border.Child as ScrollViewer;
                        scroll?.ScrollToEnd();
                    }
                }

            });
        }
        #endregion

        #region Импорт файлов
        /// <summary>
        /// Импортирует данные из указанных файлов
        /// </summary>
        /// <param name="filesPath">список файлов</param>
        /// <exception cref="Exception"></exception>
        private async Task<List<string>> Import(string path, bool isContainsHeaders, bool isContainsDescription)
        {

            List<string> queries = new List<string>();
            await Task.Run(() =>
            {
                ExcelDocument document = this.ExcelParser.Parse(path, isHasHeaders: isContainsHeaders, isHasFieldsDescription: isContainsDescription);
                if (document != null)
                {
                    this.Log.Add(this.ExcelParser.Log.Messages.ToList());
                    List<string> dbTableColumns = this.GetDBTableColumns(document.Title);
                    #region Если есть столбец idDescriptors
                    if (!String.IsNullOrEmpty(dbTableColumns.FirstOrDefault(x => x.ToLower() == Common.Strings.Columns.idDescriptor.ToLower())))
                    {
                        #region Удаление столбца idDescriptor
                        document.Remove(Common.Strings.Columns.idDescriptor);
                        #endregion

                        var connectionString = this.CustomBase.CustomDb.Settgins.CurrentConnectionString.Value;
                        string providerConnectionString = new EntityConnectionStringBuilder(connectionString).ProviderConnectionString;
                        using (SqlConnection connection = new SqlConnection(providerConnectionString))
                        {
                            List<string> descriptorsIds = new List<string>();
                            #region Получение списка Id дескрипторов
                            connection.Open();
                            List<string> dbDescriptorsColumns = this.GetDBTableColumns("Descriptors");
                            dbDescriptorsColumns.Remove(Common.Strings.Columns.id);
                            ExcelDocument descriptorsDocument = this.ExcelParser.Parse(path,
                                isHasHeaders: isContainsHeaders, isHasFieldsDescription: isContainsDescription);
                            List<string> descriptorsQueries = descriptorsDocument.ToSQLScript(dbDescriptorsColumns, "Descriptors");
                            foreach (var query in descriptorsQueries)
                            {
                                SqlCommand command = new SqlCommand(query, connection);
                                SqlDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    descriptorsIds.Add(reader[Common.Strings.Columns.id].ToString());
                                }
                                reader.Close();
                            }
                            connection.Close();
                            #endregion
                            #region Заполнение сущностей в таблицу с привязкой к дескрипторам
                            ExcelField idDescriptorField = new ExcelField(title: Common.Strings.Columns.idDescriptor, description: "ID дескриптора");
                            document.Add(idDescriptorField);
                            if (descriptorsIds.Count != document.RowsCount)
                            {
                                this.Log.Add(Common.Strings.Errors.incorrectDescriptorsCount, MessageType.Error);
                            }
                            else
                            {
                                for (int i = 0; i < document.RowsCount; i++)
                                {
                                    document.Rows[i].Set(Common.Strings.Columns.idDescriptor, descriptorsIds[i]);
                                }
                                document.Remove(Common.Strings.Columns.id);
                                queries = document.ToSQLScript(dbTableColumns);
                            }
                            #endregion

                        }
                    }
                    #endregion
                    #region Если нет столбца idDescriptors
                    else
                    {
                        this.Log.Add("Парсинг файла {0} завершен", Path.GetFileName(path));
                        #region Реализация через EF
                        //var tableProperty = db.GetType().GetProperties().FirstOrDefault(p => p.PropertyType.Name.StartsWith("DbSet") && p.Name == document.Title);
                        //var dbSet = tableProperty.GetValue(db, null);
                        //Type dbSetType = dbSet.GetType().GetGenericArguments().First();
                        //var collectionWithType = typeof(List<>).MakeGenericType(dbSetType);
                        //ConstructorInfo ctor = dbSetType.GetConstructor(new[] { typeof(int) });
                        //object instance = ctor.Invoke(new object[] { 10 });
                        #endregion
                        #region Реализация через Raw SQL                       
                        dbTableColumns.Remove(Common.Strings.Columns.id);
                        this.Log.Add("Создание запросов для вставки в таблицу {0}", document.Title);
                        queries = document.ToSQLScript(dbTableColumns);
                        #endregion
                    }
                    #endregion
                    this.CountCurrentRows += this.ExcelParser.CountLinesCurrent;
                }
                else
                {
                    this.Log.Add("Не удалось создать документ из файла '{0}'", Path.GetFileNameWithoutExtension(path));
                }
                return Task.CompletedTask;
            });
            return queries;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="filesNames"></param>
        /// <param name="isContainsHeaders"></param>
        /// <param name="isContainsDescription"></param>
        /// <returns></returns>
        private Task<List<string>> Import(string folderPath, List<string> filesNames, bool isContainsHeaders, bool isContainsDescription)
        {
            if (String.IsNullOrEmpty(folderPath))
            {
                throw new Exception(Common.Strings.Errors.incorrectPath);
            }
            if (filesNames == null)
            {
                throw new ArgumentNullException(Common.Strings.Errors.incorrectList);
            }
            List<string> queries = new List<string>();
            foreach (var fileName in filesNames)
            {
                queries.AddRange((IEnumerable<string>)this.Import(String.Format("{0}\\{1}", folderPath, fileName), isContainsHeaders, isContainsDescription));
            }
            return Task.FromResult(queries);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filesPath"></param>
        /// <exception cref="Exception"></exception>
        private async void ImportFiles(List<string> filesPath, bool isHasHeaders, bool isHasDescriptions)
        {
            this.pbImportProgress.Title = Common.Strings.Messages.importStarted;
            this.Log.Clear();
            this.ShowProgress(true);
            if (filesPath == null || filesPath.Count == 0)
            {
                throw new Exception("Указан некорректный путь до файлов!");
            }
            foreach (string path in filesPath)
            {
                int rowsCount = File.ReadLines(path).Count();
                if (this.IsContainsHeaders)
                    rowsCount--;
                if (this.IsContainsDescription)
                    rowsCount--;
                this.CountTotalRows += rowsCount;
            }
            foreach (string path in filesPath)
            {
                this.Log.Add("Обработка файла {0}", Path.GetFileName(path));

                var tableProperty = this.DB.GetType().GetProperties().FirstOrDefault(p => p.PropertyType.Name.StartsWith("DbSet") && p.Name == Path.GetFileNameWithoutExtension(path));
                #region Если таблицы нет в БД
                if (tableProperty == null)
                {
                    this.Log.Add("Таблица с именем '{0}' не найдена в базе данных!", Path.GetFileNameWithoutExtension(path));
                }
                #endregion
                #region Если таблица есть в БД
                else
                {
                    List<string> queries = await this.Import(path, this.IsContainsHeaders, this.IsContainsDescription);
                    if (queries != null)
                    {
                        foreach (var query in queries)
                        {
                            await Task.Run(() =>
                            {
                                try
                                {
                                    this.DB.Database.ExecuteSqlCommand(query);
                                }
                                catch (Exception ex)
                                {
                                    this.Log.Add(new LogMessage(ex.Message,
                                        String.Format("Таблица '{0}'\n Запрос '{1}'",
                                        Path.GetFileNameWithoutExtension(path), query),
                                        MessageType.Error));
                                }

                                return Task.CompletedTask;
                            });
                        }
                        this.DB.SaveChanges();
                    }
                    this.Log.Add("Импорт данных из файла {0} завершен", Path.GetFileName(path));
                }
                #endregion
            }
            this.Log.Add("Обработка файлов завершена!");
        }
        #endregion

        #region Работа с флажковыми переключателями для заголовков и описания 
        /// <summary>
        /// 
        /// </summary>
        private void UpdateChbHeaders()
        {
            this.chbIsHasHeaders.IsChecked = this.IsContainsHeaders;
            if (this.IsContainsHeaders)
            {
                this.chbIsHasHeaders.Content = Common.Strings.Controls.isContainsHeaders;
            }
            else
            {
                this.chbIsHasHeaders.Content = Common.Strings.Controls.isNotContainsHeaders;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateChbDescriptions()
        {
            this.chbIsHasDescriptions.IsChecked = this.IsContainsDescription;
            if (this.IsContainsDescription)
            {
                this.chbIsHasDescriptions.Content = Common.Strings.Controls.isContainsDescription;
            }
            else
            {
                this.chbIsHasDescriptions.Content = Common.Strings.Controls.isNotContainsDescription;
            }
        }
        #endregion

        #region Работа со списком выбранных файлов
        /// <summary>
        /// Получение списка файлов из указанной папки
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        private List<string> GetFilesFromFolder(string folderPath, string extension = "*.csv")
        {
            // Получение списка имен файлов из указанной папки напрямую, без создания промежуточного списка
            return Directory.GetFiles(folderPath, extension)
                            .Select(filePath => Path.GetFileName(filePath))
                            .ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        private async Task UpdateSelectedItems(List<string> fileNames)
        {
            await Task.Run(() =>
            {
                try
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.lvSelectedItems.Items.Clear();
                        foreach (var item in fileNames)
                        {
                            this.lvSelectedItems.Items.Add(item);
                        }
                    }
                    );
                }
                catch (Exception ex)
                {
                    this.ShowError(ex);
                }
            });
        }
        #endregion

        #region Работа с прогресс баром имопрта файлов
        /// <summary>
        /// Обновляет прогресс обработки файла
        /// </summary>
        /// <returns></returns>
        private async Task UpdateProgress()
        {
            await Task.Run(() =>
            {
                try
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        if (this.excelParser != null && this.excelParser.CountLinesTotal != 0)
                        {
                            if (this.pbImportProgress.Value > 0)
                                this.pbImportProgress.Header = String.Format("Выполнено: {0:F0}%", this.pbImportProgress.Value);
                            if (this.CountTotalRows != 0)
                                this.Progress = 100 * (double)this.CountCurrentRows / this.CountTotalRows;
                            else
                                this.Progress = 100;

                        }
                        else
                        {
                            this.Progress = 0;
                        }
                    });
                }
                catch (Exception ex)
                {
                    this.ShowError(ex);
                }
            });
        }


        /// <summary>
        /// Скрывает или показывает элемент управления прогресса импорта файлов
        /// </summary>
        /// <param name="isShow">Показывать прогресс импорта файлов или нет</param>
        private void ShowProgress(bool isShow = true)
        {
            if (isShow)
            {
                this.pbImportProgress.Visibility = System.Windows.Visibility.Visible;
                this.pbImportProgress.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.pbImportProgress.Visibility = System.Windows.Visibility.Collapsed;
                this.pbImportProgress.Visibility = System.Windows.Visibility.Collapsed;
            }

        }
        #endregion

        #region Прочее
        /// <summary>
        /// Получает список названий столбоц таблицы из БД
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private List<string> GetDBTableColumns(string tableName)
        {
            var tableProperty = this.DB.GetType()
                                       .GetProperties()
                                       .FirstOrDefault(p => p.PropertyType.Name.StartsWith("DbSet") && p.Name == tableName);
            var dbSet = tableProperty?.GetValue(this.DB, null);
            if (dbSet == null)
                return new List<string>();
            Type dbSetType = dbSet.GetType().GetGenericArguments().First();
            return dbSetType.GetProperties()
                            .Select(property => property.Name)
                            .ToList();
        }
        #endregion

        #endregion

        #region Конструкторы/Деструкторы
        public PageImport(CustomBase customBase) : base(customBase)
        {
            this.InitializeComponent();
            this.UpdateChbHeaders();
            this.UpdateChbDescriptions();
            this.CountTotalRows = 0;
            this.pbImportProgress.Maximum = 100;
            this.pbImportProgress.Title = Common.Strings.Messages.importNotStarted;
            this.ExcelParser = new ExcelParser();
            this.Log = new Log();
            this.Log.Messages.CollectionChanged += this.Log_CollectionChanged;


            #region Подсчет прогресса выполнения
            System.Timers.Timer timer = new System.Timers.Timer(100);
            timer.Elapsed += async (sender, e) => await this.UpdateProgress();
            //timer.Elapsed += async (sender, e) => await UpdateLog();
            timer.Start();
            #endregion
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void CustomPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ShowProgress(false);
        }
        private void Log_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //list changed - an item was added.
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                this.UpdateLog();
            }

        }
        private void btnImport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                #region Получение списка файлов от пользователя
                List<string> fileNames = this.GetLoadFilePath("Файлы Excel(*.xlsx,*.csv)|*.xlsx;*.csv|Книга Excel|*.xlsx|CSV UTF-8|*.csv", true, 1).ToList();
                #endregion

                #region Получение списка файлов из тестовых массивов
                //List<string> fileNamesFromUser = GetTestFilesList(this.Таблицы_справочники);
                //fileNamesFromUser.AddRange(GetTestFilesList(this.Таблицы_справочники2));
                //fileNamesFromUser.AddRange(GetTestFilesList(this.Таблицы_связи));
                //fileNamesFromUser.AddRange(GetTestFilesList(this.Главные_таблицы));
                #endregion
                _ = this.UpdateSelectedItems(fileNames);
                this.ImportFiles(fileNames, this.IsContainsHeaders, this.IsContainsDescription);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void chbIsHasHeaders_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        private void chbIsHasDescriptions_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        private void chbIsHasHeaders_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.IsContainsHeaders = (bool)this.chbIsHasHeaders.IsChecked;
            this.UpdateChbHeaders();
        }
        private void chbIsHasDescriptions_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.IsContainsDescription = (bool)this.chbIsHasDescriptions.IsChecked;
            this.UpdateChbDescriptions();
        }
        private void btnShowTables_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //TablesWindow tablesWindow = new TablesWindow();
            //tablesWindow.ShowDialog();
        }
        private void btnImport_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        private void CustomWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                #region Получение списка файлов от пользователя
                //List<string> fileNamesFromUser = GetLoadFilePath("Файлы Excel(*.xlsx,*.csv)|*.xlsx;*.csv|Книга Excel|*.xlsx|CSV UTF-8|*.csv", true, 1).ToList();
                #endregion

                #region Получение списка файлов из тестовых массивов
                //List<string> fileNamesFromUser = GetTestFilesList(this.Таблицы_справочники);
                //fileNamesFromUser.AddRange(GetTestFilesList(this.Таблицы_справочники2));
                //fileNamesFromUser.AddRange(GetTestFilesList(this.Таблицы_связи));
                //fileNamesFromUser.AddRange(GetTestFilesList(this.Главные_таблицы));
                //List<string> fileNamesFromUser = new List<string>() { "C:\\Users\\ivan.ivanov\\Desktop\\Данные для импорта\\csv\\Norms.csv" };
                #endregion

                //this.UpdateSelectedItems(fileNamesFromUser);
                //this.ImportFiles(fileNamesFromUser, this.isContainsHeaders, this.isContainsDescription);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnImportOne_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                #region Получение списка файлов от пользователя
                List<string> fileNames = this.GetLoadFilePath("Файлы Excel(*.xlsx,*.csv)|*.xlsx;*.csv|Книга Excel|*.xlsx|CSV UTF-8|*.csv", true, 1).ToList();
                #endregion

                _ = this.UpdateSelectedItems(fileNames);
                this.ImportFiles(fileNames, this.IsContainsHeaders, this.IsContainsDescription);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnImportAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                #region Получение списка файлов для строгого порядка импорта таблиц
                List<string> fileNames = this.Таблицы_справочники;
                fileNames.AddRange(this.Таблицы_справочники2);
                fileNames.AddRange(this.Таблицы_связи);
                fileNames.AddRange(this.Главные_таблицы);
                #endregion

                #region Получение списка файлов от пользователя
                string folderPath = this.GetFolderPath();   // Получение пути к папке от пользователя
                List<string> fileNamesFromUser = this.GetFilesFromFolder(folderPath); // Получение списка файлов из указанной папки
                //fileNames.Sort((a, b) => fileNames.IndexOf(a).CompareTo(fileNames.IndexOf(b))); // Сортировка файлов в соответствии с порядком в fileNamesFromUser               
                #endregion
                for (int i = 0; i < fileNames.Count; i++)
                {
                    fileNames[i] = String.Format("{0}\\{1}", folderPath, fileNames[i]);
                }

                _ = this.UpdateSelectedItems(fileNames);
                this.ImportFiles(fileNames, this.IsContainsHeaders, this.IsContainsDescription);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        public override void UpdateFields(List<CustomEventArgs> args)
        {
            //throw new NotImplementedException();
        }

        public override void UpdateForm(List<CustomEventArgs> args)
        {
            //throw new NotImplementedException();
        }

        public override object HandleOk(List<CustomEventArgs> args)
        {
            throw new NotImplementedException();
        }

        public override object HandleCancel(List<CustomEventArgs> args)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
