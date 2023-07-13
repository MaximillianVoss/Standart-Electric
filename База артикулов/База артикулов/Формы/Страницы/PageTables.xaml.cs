using BaseWindow_WPF.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        public String title { set; get; }
        public String titleDisplay { set; get; }
        /// <summary>
        /// Тип элементов, которые хранятся в коллекции
        /// </summary>
        public Type itemsType { set; get; }
        public List<string> ColumnNames { get; set; }
        public IEnumerable Rows { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для PageTables.xaml
    /// </summary>
    public partial class PageTables : CustomPage
    {
        #region Поля

        #endregion

        #region Свойства
        public Type DbSetType { set; get; }
        public object TreeViewSelectedObject { set; get; }
        /// <summary>
        /// ID элемента, выбранного в таблице
        /// </summary>
        public int IdSelectedItem { set; get; }
        /// <summary>
        /// Элемент, выбранный в таблице
        /// </summary>
        public object SelectedItem { set; get; }
        #endregion

        #region Методы

        #region Работа с таблицей
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
        private TableData GetTable(string tableName)
        {
            var view = this.DB.ViewsTablesView.FirstOrDefault(x => x.Отображаемое_название_таблицы == tableName);
            if (view == null)
                throw new Exception($"Не найдена таблица с именем {tableName}");

            var tableProperty = this.DB.GetType()
                .GetProperties()
                .FirstOrDefault(p => p.PropertyType.Name.StartsWith("DbSet") && p.Name == view.Наименование_представления);

            if (tableProperty == null)
                throw new Exception($"Не найдено свойство DbSet для таблицы {tableName}");

            var dbSet = tableProperty.GetValue(this.DB, null);
            if (dbSet == null)
                throw new Exception($"Не удалось получить значение DbSet для таблицы {tableName}");

            if (!dbSet.GetType().IsGenericType || !dbSet.GetType().GetGenericArguments().Any())
                throw new Exception("Не удалось получить тип данных для этой таблицы!");

            Type dbSetType = dbSet.GetType().GetGenericArguments().First();
            var collectionWithType = typeof(ObservableCollection<>).MakeGenericType(dbSetType);
            var tableItems = Activator.CreateInstance(collectionWithType, dbSet);
            var columnNames = dbSetType.GetProperties().Select(p => p.Name).ToList();

            return new TableData
            {
                title = view.Наименование_представления,
                titleDisplay = view.Отображаемое_название_представления,
                itemsType = dbSetType,
                ColumnNames = columnNames,
                Rows = (IEnumerable)tableItems
            };
        }

        /// <summary>
        /// Экспорт таблицы в CSV
        /// TODO: сделать экспорт для каждой таблицы
        /// </summary>
        /// <param name="outputFolderPath"></param>
        private void Export(string outputFilePath, bool isDeleteUnderline = true)
        {
            if (this.cmbTables.SelectedIndex != -1)
            {
                var selectedTableName = this.cmbTables.SelectedItem.ToString();
                var table = this.GetTable(selectedTableName);

                if (table != null && table.itemsType != null)
                {
                    using (var writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
                    {
                        var entities = table.Rows.OfType<object>().ToList();
                        if (entities.Any())
                        {
                            var propertyNames = table.itemsType.GetProperties().Select(p => isDeleteUnderline ? p.Name.Replace("_", " ") : p.Name);

                            string separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

                            var header = string.Join(separator, propertyNames);
                            writer.WriteLine(header);

                            foreach (var entity in entities)
                            {
                                var values = propertyNames.Select(p =>
                                {
                                    var value = entity.GetType().GetProperty(isDeleteUnderline ? p.Replace(" ", "_") : p)?.GetValue(entity)?.ToString();
                                    return string.IsNullOrEmpty(value) ? value : $"\"{value.Replace("\"", "\"\"")}\"";
                                });

                                var line = string.Join(separator, values);
                                writer.WriteLine(line);
                            }
                        }
                    }
                }
            }
        }
        #endregion

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
            this.UpdateDataGrid(this.GetTable(tableName));
        }

        private void UpdateDataGrid(TableData tableData)
        {
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
            if (this.cmbTables.SelectedIndex == -1)
                return;
            TableData table = this.GetTable(this.cmbTables.SelectedItem);
            if (table.itemsType != typeof(ProductsView) && !table.titleDisplay.Equals("Продукты", StringComparison.OrdinalIgnoreCase))
            {
                this.UpdateDataGrid(this.cmbTables.SelectedItem);
                return;
            }
            IEnumerable<ProductsView> productsViews = table.Rows.OfType<ProductsView>();
            switch (@object)
            {
                case Classes @class:
                    productsViews = productsViews.Where(x => x.Наименование_класса == @class.Descriptors.title);
                    break;

                case Groups group:
                    productsViews = productsViews.Where(x => x.Наименование_группы == group.Descriptors.title);
                    break;

                case SubGroups subGroup:
                    productsViews = productsViews.Where(x => x.Наименование_подгруппы == subGroup.Descriptors.title);
                    break;
            }

            table.Rows = productsViews.ToList();
            this.UpdateDataGrid(table);
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
        public PageTables() : base()
        {
            try
            {
                this.InitializeComponent();
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
        private void CustomPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.UpdateTablesComboBox();
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
                    if (this.dgTable.SelectedCells.Count > 0)
                    {
                        var table = this.GetTable(this.cmbTables.SelectedItem);
                        int columnIndex = this.dgTable.SelectedCells[0].Column.DisplayIndex;
                        if (table.ColumnNames.Count == 0)
                            throw new Exception("В таблице нет столбцов!");
                        this.SelectedItem = this.dgTable.SelectedItem;
                        this.IdSelectedItem = (int)this.GetObjectFieldValue(this.SelectedItem, table.ColumnNames[columnIndex]);
                        // Now you have the selected column index

                    }
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
                this.TreeViewSelectedObject = e.NewValue;
                this.FilterTableByTreeView(this.DbSetType, ((TreeViewItemCustom)e.NewValue).Value);
            }
        }

        #region Кнопки контекстного меню таблицы

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var windowEdit = new WindowEdit();
                windowEdit.ShowDialog();
                //this.SelectTable(this.cmbTables.SelectedItem);
                //this.UpdateTreeView();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var windowEdit = new WindowEdit(this.SelectedItem);
                windowEdit.ShowDialog();
                //this.SelectTable(this.cmbTables.SelectedItem);
                //this.UpdateTreeView();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        #endregion

        #region Кнопки верхнего меню

        private void miExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = "CSV files (*.csv)|*.csv|Excel Files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    dialog.FilterIndex = 1;
                    dialog.RestoreDirectory = true;

                    var selectedTableName = "";
                    if (this.cmbTables.SelectedIndex != -1)
                    {
                        selectedTableName = this.cmbTables.SelectedItem.ToString();
                        dialog.FileName = selectedTableName;
                    }
                    else
                    {
                        throw new Exception("Не выбрана таблица для экспорта");
                    }

                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        var outputFilePath = dialog.FileName;
                        this.Export(outputFilePath);
                        this.ShowMessage($"Экспорт таблицы {selectedTableName} завершен!");
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
            try
            {
                if (this.DbSetType != typeof(ProductsView))
                    throw new Exception("Редактирование других таблиц недоступно");

            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
            ProductWindow productWindow = new ProductWindow(this.DbSetType, this.IdSelectedItem);
            productWindow.ShowDialog();
        }
        private void miDelete_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Контекстное меню иерархического дерева
        private void miTreeAdd_Click(object sender, RoutedEventArgs e)
        {
            var windowEdit = new WindowEdit();
            windowEdit.ShowDialog();
        }
        private void miTreeEdit_Click(object sender, RoutedEventArgs e)
        {
            var windowEdit = new WindowEdit(this.TreeViewSelectedObject);
            windowEdit.ShowDialog();
        }
        #endregion

    }
    #endregion
}
