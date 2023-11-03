using BaseWindow_WPF.Classes;
using CustomControlsWPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы.Страницы
{


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
        /// Текущий объект, выбранный в таблице
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
            try
            {
                ////this.SelectedItemTreeView.Value
                //var product = CreateEmptyProduct("Новый продукт", this.DB.SubGroups.FirstOrDefault(x => x.id > 0));
                //WindowEdit windowEdit = new WindowEdit(this.DB.ProductsView.FirstOrDefault(x => x.ID_продукта == product.id));
                //windowEdit.ShowDialog();
                //this.DB.SaveChanges();

                ////var windowEdit = new WindowEdit("Создать продукт", new ProductsView(), EditModes.Edit, 600, 800);
                ////windowEdit.ShowDialog();
                //if ((bool)windowEdit.DialogResult)
                //{
                //    this.CurrentTableData = this.GetTable(this.cmbTables.SelectedItem);
                //    this.FilterTableByTreeView(this.CurrentTableData.ItemsType, this.SelectedItemTreeView.Value);
                //    this.UpdateDataGrid(this.CurrentTableData);
                //}
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }


        private void Update()
        {
            try
            {
                var windowEdit = new WindowEdit("Редактировать продукт", this.SelectedItemTable, EditModes.Edit, 600, 800);
                windowEdit.ShowDialog();
                if ((bool)windowEdit.DialogResult)
                {
                    this.CurrentTableData = this.GetTable(this.cmbTables.SelectedItem);
                    this.FilterTableByTreeView(this.CurrentTableData.ItemsType, this.SelectedItemTreeView.Value);
                    this.UpdateDataGrid(this.CurrentTableData);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void Delete()
        {
            try
            {
                throw new Exception(Common.Strings.Messages.functionalityDisabled);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        #endregion

        #region Работа с таблицей
        /// <summary>
        /// Получает данные таблицы с указанными именем
        /// </summary>
        /// <param name="tableName">имя таблицы</param>
        /// <returns></returns>
        private TableData GetTable(string tableName)
        {
            // Предполагаем, что ViewsTablesView имеет индекс по Отображаемое_название_таблицы для ускорения поиска
            var view = this.DB.ViewsTablesView.FirstOrDefault(x => x.Отображаемое_название_таблицы == tableName) ?? throw new Exception($"Не найдена таблица с именем {tableName}");

            // Кэшируем свойства
            var dbProperties = this.DB.GetType().GetProperties().Where(p => p.PropertyType.Name.StartsWith("DbSet")).ToDictionary(p => p.Name, p => p);

            if (!dbProperties.TryGetValue(view.Наименование_представления, out var tableProperty))
                throw new Exception($"Не найдено свойство DbSet для таблицы {tableName}");

            var dbSet = tableProperty.GetValue(this.DB) as IEnumerable ?? throw new Exception($"Не удалось получить значение DbSet для таблицы {tableName}");

            // Допускаем только один вызов GetGenericArguments
            var genericArguments = dbSet.GetType().GetGenericArguments();

            if (!genericArguments.Any())
                throw new Exception("Не удалось получить тип данных для этой таблицы!");

            Type dbSetType = genericArguments.First();
            var columnNames = dbSetType.GetProperties().Select(p => p.Name).ToList();

            TableData tableData = new TableData(
                view.Наименование_представления,
                view.Отображаемое_название_таблицы,
                dbSetType,
                columnNames
            );


            tableData.AddRange(dbSet);

            // Сброс фильтра
            tableData.ResetFilter();

            return tableData;
        }

        /// <summary>
        /// Экспорт таблицы в CSV
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
        /// <param name="tableData">имя таблицы</param>
        private Task UpdateDataGrid(TableData tableData)
        {
            this.dgTable.TableData = tableData;
            if (this.CurrentTableData.ItemsType == typeof(ProductsViewLite))
            {
                // Создание списка с одним элементом
                var displayColumns = new List<string> {
                    "Артикул",
                    "Наименование_продукта",
                    "Вес",
                    "Наименование_единицы_измерения"
                };
                this.dgTable.TableData.SetDisplayColumns(displayColumns);
            }
            return Task.CompletedTask;
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
        private Task FilterTableByTreeView(Type tableType, object @object)
        {
            if (this.cmbTables.SelectedIndex == -1)
                return Task.CompletedTask;
            if (this.CurrentTableData.ItemsType != typeof(ProductsViewLite))
                return Task.CompletedTask;


            string className = null;
            string groupName = null;
            string subGroupName = null;

            switch (@object)
            {
                case Classes @class:
                    className = @class.Descriptors.title;
                    break;

                case Groups group:
                    groupName = group.Descriptors.title;
                    break;

                case SubGroups subGroup:
                    subGroupName = subGroup.Descriptors.title;
                    break;
            }

            //List<GetFilteredProductsLite_Result> results = this.CustomDb.GetFilteredProductsLite(
            //    groupName,
            //    className,
            //    subGroupName
            //).ToList();
            //List<ProductsViewLite> filteredProducts = results.Select(result => new ProductsViewLite(result)).ToList();
            var filteredProducts = this.DB.GetFilteredProducts(groupName, className, subGroupName);
            this.CurrentTableData.ItemsAll = new ObservableCollection<object>(filteredProducts);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="class"></param>
        private TreeViewItemCustom AddToTreeView(Classes @class)
        {
            TreeViewItemCustom classItem = new TreeViewItemCustom(@class.id, @class.Descriptors.title, @class);
            foreach (var group in @class.Groups)
            {
                TreeViewItemCustom groupItem = new TreeViewItemCustom(group.id, group.Descriptors.title, group);
                foreach (var subGroup in group.SubGroups)
                {
                    groupItem.Add(new TreeViewItemCustom(subGroup.id, subGroup.Descriptors.title, subGroup));
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
        /// <summary>
        /// Сохраняет состояние "открыто"/"закрыто" для веток иерархического списка
        /// TODO: доделать для групп, подгрупп, сохранение идет только для классов
        /// </summary>
        /// <param name="items"></param>
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
        private async Task UpdateTreeView()
        {
            if (this.CurrentTableData.ItemsType == typeof(Products) ||
                this.CurrentTableData.ItemsType == typeof(ProductsViewLite))
            {
                // Сохраняем текущее состояние дерева перед его очисткой
                this.treeState = new Dictionary<int, bool>();
                this.SaveTreeState(this.tvGroups.Items);

                this.tvGroups.Items.Clear();
                this.tvGroups.UpdateLayout();
                //this.InitDB(this.IsCollectionUpdated); // Если этот метод может быть асинхронным, добавьте await

                // Если доступ к базе данных является блокирующей операцией, используйте асинхронные методы.
                var classes = await Task.Run(() => this.DB.Classes.ToList());

                foreach (var @class in classes)
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
                this.dgTable.Title = string.Empty;
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private async void cmbTables_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.cmbTables.SelectedItem != null)
                {
                    this.CurrentTableData = this.GetTable(this.cmbTables.SelectedItem);
                    _ = this.UpdateTreeView();
                    _ = this.UpdateDataGrid(this.CurrentTableData);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        #region События иерархического списка

        #region Нажатия левой/правой кнопкой мыши
        private async void tvGroups_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (e.NewValue != null)
                {
                    this.SelectedItemTreeView = (TreeViewItemCustom)e.NewValue;
                    await this.FilterTableByTreeView(this.CurrentTableData.ItemsType, this.SelectedItemTreeView.Value);
                    await this.UpdateDataGrid(this.CurrentTableData);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
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
            try
            {
                var windowEdit = new WindowEdit("Создание", new Classes(), EditModes.Create);
                windowEdit.ShowDialog();
                _ = this.UpdateTreeView();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void miTreeEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var windowEdit = new WindowEdit("Редактирование", this.SelectedItemTreeView);
                windowEdit.ShowDialog();
                _ = this.UpdateTreeView();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void miTreeDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                throw new Exception(Common.Strings.Messages.functionalityDisabled);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        #endregion

        #endregion

        #region Кнопки контекстного меню таблицы

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
        private void dgTable_RightClickSelectedCellChanged(object sender, EventArgs e)
        {

        }
        private void dgTable_LeftClickSelectedCellChanged(object sender, EventArgs e)
        {
            try
            {
                this.SelectedItemTable = this.dgTable.SelectedItem;
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void dgTable_AddMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                this.Create();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void dgTable_DeleteMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                this.Delete();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void dgTable_EditMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                this.Update();
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
                if (this.CurrentTableData.ItemsType != typeof(ProductsViewLite))
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
                //this.UpdateDataGrid(this.CurrentTableData, this.pcProducts.CurrentPage, this.pcProducts.PageSize);
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
                //int rowCount = this.CurrentTableData.GetRowsCount();
                //int pageSize = this.pcProducts.PageSize;
                //this.pcProducts.PagesCount = rowCount / pageSize;
                //if (rowCount % pageSize != 0)
                //{
                //    this.pcProducts.PagesCount++;
                //}
                //this.UpdateDataGrid(this.CurrentTableData, this.pcProducts.CurrentPage, this.pcProducts.PageSize);
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
