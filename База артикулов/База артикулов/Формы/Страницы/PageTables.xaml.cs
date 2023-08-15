using BaseWindow_WPF.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
        public Type ItemsType { set; get; }
        public List<string> ColumnNames { get; set; }

        // Коллекция всех элементов
        public ObservableCollection<object> ItemsAll { get; set; } = new ObservableCollection<object>();

        // Коллекция отфильтрованных элементов
        public ObservableCollection<object> ItemsFiltered { get; set; } = new ObservableCollection<object>();

        // Получение элементов указанной страницы из отфильтрованных элементов.
        // Нумерация страниц начинается с 1.
        public IEnumerable GetPage(int pageNumber, int itemsPerPage)
        {
            return this.ItemsFiltered
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();
        }

        public int GetRowsCount()
        {
            return this.ItemsFiltered.Count;
        }

        // Проверяет, существует ли указанная страница
        public bool HasPage(int pageNumber, int itemsPerPage)
        {
            return this.ItemsFiltered.Skip((pageNumber - 1) * itemsPerPage).Any();
        }

        // Метод фильтрации элементов по определенному условию. Вы можете его изменить под ваши нужды.
        public void ApplyFilter(Predicate<object> filterCriteria)
        {
            this.ItemsFiltered.Clear();

            foreach (var item in this.ItemsAll)
            {
                if (filterCriteria(item))
                {
                    this.ItemsFiltered.Add(item);
                }
            }
        }

        // Если вы хотите вернуть все элементы в FilteredItems, например, при сбросе фильтра
        public void ResetFilter()
        {
            this.ItemsFiltered.Clear();
            foreach (var item in this.ItemsAll)
            {
                this.ItemsFiltered.Add(item);
            }
        }
    }

    /// <summary>
    /// Логика взаимодействия для PageTables.xaml
    /// </summary>
    public partial class PageTables : CustomPage
    {
        #region Поля
        private Dictionary<int, bool> treeState;
        #endregion

        #region Свойства

        /// <summary>
        /// ID элемента, выбранного в таблице
        /// </summary>
        private int SelectedItemTableId { set; get; }
        /// <summary>
        /// Элемент, выбранный в таблице
        /// </summary>
        private object SelectedItemTable { set; get; }
        /// <summary>
        /// Текущий объект, выбранный в иерархическом списке
        /// Само значение лежит в  поле Value
        /// </summary>
        private TreeViewItemCustom SelectedItemTreeView { set; get; }
        /// <summary>
        /// Данные из текущей выбранной таблицы
        /// </summary>
        private TableData CurrentTableData { set; get; }

        #endregion

        #region Методы

        #region Работа с элементами таблиц
        private void Create()
        {

        }

        private void Update()
        {

        }

        private void Delete()
        {

        }
        #endregion

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
            var columnNames = dbSetType.GetProperties().Select(p => p.Name).ToList();

            TableData tableData = new TableData
            {
                title = view.Наименование_представления,
                titleDisplay = view.Отображаемое_название_таблицы,
                ItemsType = dbSetType,
                ColumnNames = columnNames,
            };

            foreach (var item in (IEnumerable)dbSet)
            {
                tableData.ItemsAll.Add(item);
            }

            // Сброс фильтра, чтобы скопировать все элементы из AllItems в FilteredItems
            tableData.ResetFilter();

            return tableData;
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

                if (table != null && table.ItemsType != null)
                {
                    using (var writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
                    {
                        var entities = table.ItemsAll.OfType<object>().ToList();
                        if (entities.Any())
                        {
                            var propertyNames = table.ItemsType.GetProperties().Select(p => isDeleteUnderline ? p.Name.Replace("_", " ") : p.Name);

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
        /// <summary>
        /// Заполняет элемент управления DataGrid записями из указанной таблицы
        /// </summary>
        /// <param name="tableName">имя таблицы</param>
        private void UpdateDataGrid(string tableName)
        {
            this.UpdateDataGrid(this.GetTable(tableName), this.pcProducts.CurrentPage, this.pcProducts.PageSize);
        }
        /// <summary>
        /// Заполняет элемент управления DataGrid записями из указанной таблицы
        /// </summary>
        /// <param name="tableData">имя таблицы</param>
        private void UpdateDataGrid(TableData tableData, int pageNumber, int itemsPerPage)
        {
            #region Обновление фильтра
            if (this.tvGroups.Visibility == Visibility.Visible && this.SelectedItemTreeView != null && this.SelectedItemTreeView.Value != null)
            {
                this.FilterTableByTreeView(this.CurrentTableData.ItemsType, this.SelectedItemTreeView.Value);
            }
            #endregion
            this.InitDB(this.IsCollectionUpdated);
            // Clear previous columns
            this.dgTable.AutoGenerateColumns = false;
            this.dgTable.Columns.Clear();
            // Set ItemsSource
            //this.dgTable.ItemsSource = tableData.GetPage(pageNumber, itemsPerPage);
            this.dgTable.DataContext = tableData;
            this.dgTable.ItemsSource = tableData.ItemsFiltered;
            //Обновить количество страниц
            int rowCount = this.CurrentTableData.GetRowsCount();
            int pageSize = this.pcProducts.PageSize;
            this.pcProducts.PagesCount = rowCount / pageSize;

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
        #endregion

        #region Выпадающий список


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

            //TableData table = this.GetTable(this.cmbTables.SelectedItem);

            if (this.CurrentTableData.ItemsType != typeof(ProductsView)
                && !this.CurrentTableData.titleDisplay.Equals("Продукты", StringComparison.OrdinalIgnoreCase))
            {
                //this.UpdateDataGrid(this.cmbTables.SelectedItem);
                return;
            }

            IEnumerable<ProductsView> productsViews = this.CurrentTableData.ItemsAll.OfType<ProductsView>();

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

            this.CurrentTableData.ItemsFiltered = new ObservableCollection<object>(productsViews);

            // Обновите ваш DataGrid (если это необходимо) 
            //this.UpdateDataGrid(table, this.pcProducts.CurrentPage, this.pcProducts.PageSize);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="class"></param>
        private TreeViewItemCustom AddToTreeView(Classes @class)
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
            return classItem; // Возвращаем созданный объект TreeViewItemCustom
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

        private void SaveTreeState(ItemCollection items)
        {
            foreach (var obj in items)
            {
                var item = obj as TreeViewItemCustom;
                if (item == null)
                    continue;

                if (item.Value is Classes @class)
                {
                    this.treeState[@class.id] = item.IsExpanded;
                }
                // Сохраняем состояние каждого дочернего элемента
                if (item.Items.Count > 0)
                {
                    this.SaveTreeState(item.Items);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateTreeView()
        {
            if (this.CurrentTableData.ItemsType == typeof(Products) ||
                this.CurrentTableData.ItemsType == typeof(ProductsView))
            {
                // Сохраняем текущее состояние дерева перед его очисткой
                this.treeState = new Dictionary<int, bool>();
                this.SaveTreeState(this.tvGroups.Items);

                this.tvGroups.Items.Clear();
                this.tvGroups.UpdateLayout();
                this.InitDB(this.IsCollectionUpdated);

                foreach (var @class in this.DB.Classes)
                {
                    var classItem = this.AddToTreeView(@class);
                    // Восстанавливаем состояние для каждого элемента
                    if (this.treeState.TryGetValue(@class.id, out bool isExpanded))
                    {
                        classItem.IsExpanded = isExpanded;
                    }
                }
                //Выбор первого элемента, если есть
                if (this.tvGroups.Items.Count > 0)
                {
                    TreeViewItem firstItem = (TreeViewItem)this.tvGroups.ItemContainerGenerator.ContainerFromIndex(0);
                    if (firstItem != null)
                    {
                        firstItem.IsSelected = true;
                    }
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
                if (this.cmbTables.SelectedItem != null)
                {
                    this.CurrentTableData = this.GetTable(this.cmbTables.SelectedItem);
                    this.UpdateDataGrid(this.CurrentTableData, this.pcProducts.CurrentPage, this.pcProducts.PageSize);
                    this.UpdateTreeView();
                }
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
                if (this.CurrentTableData.ItemsType != null && this.cmbTables.SelectedIndex != -1)
                {
                    if (this.dgTable.SelectedCells.Count > 0)
                    {
                        int columnIndex = this.dgTable.SelectedCells[0].Column.DisplayIndex;
                        if (this.CurrentTableData.ColumnNames.Count == 0)
                            throw new Exception("В таблице нет столбцов!");
                        this.SelectedItemTable = this.dgTable.SelectedItem;
                        this.SelectedItemTableId = (int)this.GetObjectFieldValue(this.SelectedItemTable, this.CurrentTableData.ColumnNames[columnIndex]);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
        #region События иерархического списка

        #region Нажатия левой/правой кнопкой мыши
        private void tvGroups_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                this.SelectedItemTreeView = (TreeViewItemCustom)e.NewValue;
                this.UpdateDataGrid(this.CurrentTableData, this.pcProducts.CurrentPage, this.pcProducts.PageSize);
            }
        }

        private void tvGroups_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var item = this.tvGroups.SelectedItem as TreeViewItemCustom;
            //if (item != null)
            //{
            //    this.TreeViewSelectedObject = item;
            //    this.FilterTableByTreeView(this.CurrentTableData.ItemsType, item.Value);
            //}
        }

        private void tvGroups_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        #endregion

        #region Кнопки контекстного меню иерархического списка
        private void miTreeAdd_Click(object sender, RoutedEventArgs e)
        {
            var windowEdit = new WindowEdit("Создание", new Classes(), WindowEditModes.Create);
            windowEdit.ShowDialog();
            this.UpdateTreeView();
        }
        private void miTreeEdit_Click(object sender, RoutedEventArgs e)
        {
            var windowEdit = new WindowEdit("Редактирование", this.SelectedItemTreeView);
            windowEdit.ShowDialog();
            this.UpdateTreeView();
        }
        #endregion

        #endregion

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
                var windowEdit = new ProductWindow(this.SelectedItemTable);
                windowEdit.ShowDialog();
                this.CurrentTableData = this.GetTable(this.cmbTables.SelectedItem);
                this.UpdateDataGrid(this.CurrentTableData, this.pcProducts.CurrentPage, this.pcProducts.PageSize);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        #endregion

        #region Верхнее меню
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
                if (this.CurrentTableData.ItemsType != typeof(ProductsView))
                    throw new Exception("Редактирование других таблиц недоступно");

            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
            //ProductWindow productWindow = new ProductWindow(this.DbSetType, this.IdSelectedItem);
            //productWindow.ShowDialog();
        }
        private void miDelete_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Обработчики нажатий страниц
        private void PagingControl_PageChanged(object sender, CustomControlsWPF.PageChangedEventArgs e)
        {
            try
            {
                this.UpdateDataGrid(this.CurrentTableData, this.pcProducts.CurrentPage, this.pcProducts.PageSize);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void pcProducts_PageSizeChanged(object sender, CustomControlsWPF.PageSizeChangedEventArgs e)
        {
            try
            {
                int rowCount = this.CurrentTableData.GetRowsCount();
                int pageSize = this.pcProducts.PageSize;
                this.pcProducts.PagesCount = rowCount / pageSize;
                if (rowCount % pageSize != 0)
                {
                    this.pcProducts.PagesCount++;
                }
                this.UpdateDataGrid(this.CurrentTableData, this.pcProducts.CurrentPage, this.pcProducts.PageSize);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        #endregion


    }
    #endregion
}
