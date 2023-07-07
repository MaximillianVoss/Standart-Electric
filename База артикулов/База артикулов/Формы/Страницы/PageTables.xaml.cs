using BaseWindow_WPF.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using База_артикулов.Модели;

namespace База_артикулов.Формы.Страницы
{
    public class TableData
    {
        public List<string> ColumnNames { get; set; }
        public IEnumerable Rows { get; set; }

        public List<string> GetCorrectColumnNames()
        {
            if (this.ColumnNames == null)
                return null;
            return this.ColumnNames.Select(x => x.Replace('_', ' ')).ToList();
        }
    }
    /// <summary>
    /// Логика взаимодействия для PageTables.xaml
    /// </summary>
    public partial class PageTables : CustomPage
    {
        #region Поля
        private Dictionary<string, string> tablesEngRusNames = new Dictionary<string, string>()
        {
            //{ "ArticulBase","Главная таблица" },
            //{ "BIMLibraryFile","Библиотека БИМ"},
            //{ "Covers","Покрытия"},
            //{ "DrawBlocksFile","Блоки"},
            //{ "EdIzm","Единицы измерения"},
            //{ "GroupsInApplication","Группы применения"},
            //{ "LoadTypePictures","Схемы нагрузки"},
            //{ "Materials","Метриалы"},
            //{ "Norms","Нормативы"},
            //{ "PerforationSizes","Размеры перфорации"},
            //{ "TovarApplication","Применения"},
            //{ "TovarClasses","Товарные классы"},
            //{ "TovarDirections","Товарные направления"},
            //{ "TovarGroups","Товарные группы"},
            //{ "TovarSubGroups","Товарные подгруппы"},
            //{ "TypicalAlbums","Типовые альбомы"},
            //{ "vEdIzm" ,"Единицы измерения (представление)"}

        };
        private Dictionary<string, string> tableRusEngNames = new Dictionary<string, string>();
        private Object tableItems;
        private Type dbSetType;
        private int selectedItemId;
        #endregion

        #region Свойства
        public Type DbSetType { get => this.dbSetType; set => this.dbSetType = value; }
        public int SelectedItemId { get => this.selectedItemId; set => this.selectedItemId = value; }
        #endregion

        #region Методы

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private Type GetTableItemsType(String tableName)
        {
            var tableView = this.DB.ViewsTablesView.FirstOrDefault(x => x.Отображаемое_название_таблицы.StartsWith(tableName) && x.Тип == "Пользовательский");
            if (tableView != null)
            {
                var tableProperty = this.DB.GetType().GetProperties().FirstOrDefault(
                p => p.PropertyType.Name.StartsWith("DbSet") &&
                p.Name == tableView.Наименование_таблицы);
                if (tableProperty != null)
                {
                    var dbSet = tableProperty.GetValue(this.DB, null);
                    return dbSet.GetType().GetGenericArguments().First();
                }
            }
            return typeof(object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private TableData GetTable(String tableName)
        {
            var view = this.DB.ViewsTablesView.FirstOrDefault(x => x.Отображаемое_название_таблицы == tableName);
            var tableProperty = this.DB.GetType().GetProperties().FirstOrDefault(
                p => p.PropertyType.Name.StartsWith("DbSet") &&
                p.Name == view.Наименование_представления);

            if (tableProperty != null)
            {
                var dbSet = tableProperty.GetValue(this.DB, null);
                Type dbSetType = dbSet.GetType().GetGenericArguments().First();
                var collectionWithType = typeof(ObservableCollection<>).MakeGenericType(dbSetType);
                this.tableItems = Activator.CreateInstance(collectionWithType, dbSet);
                var columnNames = dbSetType.GetProperties().Select(p => p.Name).ToList();

                return new TableData { ColumnNames = columnNames, Rows = (IEnumerable)this.tableItems };
            }

            return null;
        }

        /// <summary>
        /// Экспорт таблицы в CSV
        /// TODO: сделать экспорт для каждой таблицы
        /// </summary>
        /// <param name="outputFolderPath"></param>
        private void Export(string outputFolderPath)
        {
            if (this.cmbTables.SelectedIndex != -1)
            {
                var selectedTableName = this.cmbTables.SelectedItem.ToString();
                var table = this.GetTable(selectedTableName);
                var entityType = this.GetTableItemsType(selectedTableName);

                if (table != null && entityType != null)
                {
                    var filePath = System.IO.Path.Combine(outputFolderPath, $"{selectedTableName}.csv");

                    using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        var entities = table.Rows.OfType<object>().ToList();
                        if (entities.Any())
                        {
                            var propertyNames = entityType.GetProperties().Select(p => p.Name);
                            var header = string.Join(",", propertyNames);
                            writer.WriteLine(header);

                            foreach (var entity in entities)
                            {
                                var values = propertyNames.Select(p => entity.GetType().GetProperty(p)?.GetValue(entity)?.ToString());
                                var line = string.Join(",", values);
                                writer.WriteLine(line);
                            }
                        }
                    }

                    this.ShowMessage("Таблица успешно экспортирована!");
                }
            }
        }


        #region Выпадающий список

        /// <summary>
        /// Заполняет данные из указанной таблицы в DataGrid
        /// </summary>
        /// <param name="selectedTableName">имя таблицы на русском</param>
        /// <exception cref="Exception"></exception>
        private void SelectTable(String selectedTableName)
        {
            if (this.cmbTables.SelectedIndex != -1)
            {
                this.DbSetType = this.GetTableItemsType(selectedTableName);
                //this.dgTable.ItemsSource = this.GetTable(selectedTableName);
                this.UpdateDataGrid(selectedTableName);
            }
        }

        private void UpdateDataGrid(string tableName)
        {
            var tableData = this.GetTable(tableName);
            // Clear previous columns
            this.dgTable.AutoGenerateColumns = false;
            this.dgTable.Columns.Clear();
            // Set ItemsSource
            this.dgTable.ItemsSource = tableData.Rows;

            // Create columns based on column names
            foreach (var columnName in tableData.ColumnNames)
            {
                var column = new DataGridTextColumn
                {
                    Header = columnName.Replace('_', ' '),
                    Binding = new System.Windows.Data.Binding(columnName)
                };

                this.dgTable.Columns.Add(column);
            }
        }

        /// <summary>
        /// Добавляет элементов в ComboBox
        /// </summary>
        private void UpdateTablesComboBox()
        {
            var tables = this.DB.ViewsTablesView.OrderBy(x => x.Отображаемое_название_таблицы).Where(x => x.Тип.ToLower() == "Пользовательский".ToLower()).ToList();
            foreach (var table in tables)
                this.cmbTables.Add(table.Отображаемое_название_таблицы);
            this.cmbTables.Select("Продукты");
        }
        #endregion

        #region Иерархический список
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableType"></param>
        /// <param name="object"></param>
        private void FilterTableByTreeView(Type tableType, object @object)
        {
            if (this.cmbTables.SelectedIndex != -1)
            {
                IEnumerable table = this.GetTable(this.cmbTables.SelectedItem).Rows;
                if (tableType == typeof(Products))
                {
                    List<Products> products = (List<Products>)this.ToList(table, tableType);
                    products = products.Where(x => x.SubGroups != null).ToList();
                    products = products.Where(x => x.SubGroups.Groups != null).ToList();
                    products = products.Where(x => x.SubGroups.Groups.Classes != null).ToList();
                    if (@object.GetType().BaseType == typeof(Classes))
                    {
                        Classes @class = (Classes)@object;
                        products = products.Where(x => x.SubGroups.Groups.Classes.Descriptors.title == @class.Descriptors.title).ToList();
                    }
                    if (@object.GetType().BaseType == typeof(Groups))
                    {
                        Groups group = (Groups)@object;
                        products = products.Where(x => x.SubGroups.Groups.Descriptors.title == group.Descriptors.title).ToList();
                    }
                    if (@object.GetType().BaseType == typeof(SubGroups))
                    {
                        SubGroups group = (SubGroups)@object;
                        products = products.Where(x => x.SubGroups.Descriptors.title == group.Descriptors.title).ToList();
                    }
                    this.dgTable.ItemsSource = products;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="class"></param>
        private void AddToTreeView(Classes @class)
        {
            TreeViewItemCustom classItem = new TreeViewItemCustom(@class.Descriptors.title, @class);
            foreach (var group in @class.Groups)
            {
                TreeViewItemCustom groupItem = new TreeViewItemCustom(group.Descriptors.title, group);
                foreach (var subGroup in group.SubGroups)
                {
                    groupItem.Add(new TreeViewItemCustom(subGroup.Descriptors.title, subGroup));
                }
                classItem.Add(groupItem);
            }
            this.tvGroups.Items.Add(classItem);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowTreeView()
        {
            this.tvGroups.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        private void HideTreeView()
        {
            this.tvGroups.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateTreeView()
        {
            if (this.DbSetType == typeof(Products))
            {
                this.tvGroups.Items.Clear();
                foreach (var @class in this.DB.Classes)
                {
                    this.AddToTreeView(@class);
                }
                this.ShowTreeView();
            }
            else
            {
                this.HideTreeView();
            }
        }
        #endregion

        #endregion

        #region Конструкторы/Деструкторы
        public PageTables()
        {
            this.InitializeComponent();
            this.UpdateTablesComboBox();
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void CustomPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.UpdateTreeView();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void dgTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            this.DB.SaveChanges();
        }
        private void dgTable_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DB.SaveChanges();
        }
        private void dgTable_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            this.DB.SaveChanges();
        }
        private void cmbTables_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                this.SelectTable(this.cmbTables.SelectedItem);
                this.UpdateTreeView();

            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void dgTable_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                if (this.DbSetType != null && this.cmbTables.SelectedIndex != -1)
                {
                    var item = this.dgTable.SelectedItem;
                    this.SelectedItemId = (int)this.GetObjectFieldValue(item, "id");
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }

        }
        private void tvGroups_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                this.FilterTableByTreeView(this.DbSetType, ((TreeViewItemCustom)e.NewValue).Value);
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.DbSetType == typeof(Products))
            {
                ProductWindow productWindow = new ProductWindow(this.DbSetType, this.selectedItemId);
                productWindow.ShowDialog();
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ImportWindow importWindow = new ImportWindow();
                importWindow.ShowDialog();

            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "Select Output Folder";
                    dialog.ShowNewFolderButton = true;

                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        var outputFolderPath = dialog.SelectedPath;
                        this.Export(outputFolderPath);
                        this.ShowMessage("Экспорт завершен!");
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }

        }

        private void miAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    #endregion
}
