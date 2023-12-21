using BaseWindow_WPF.Classes;
using CustomControlsWPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using База_артикулов.Классы;
using База_артикулов.Классы.Дополнения;
using База_артикулов.Модели;

namespace База_артикулов.Формы.Страницы
{
    /// <summary>
    /// Логика взаимодействия для PageTables.xaml
    /// </summary>
    public partial class PageTables : CustomPage
    {
        #region Поля
        #endregion

        #region Свойства
        /// <summary>
        /// Текущий объект, выбранный в таблице
        /// </summary>
        private object SelectedItemTable
        {
            set; get;
        }
        /// <summary>
        /// Текущий объект, выбранный в иерархическом списке,
        /// само значение лежит в  поле Value
        /// </summary>
        private TreeViewItemCustom SelectedItemTreeView
        {
            set; get;
        }
        /// <summary>
        /// Данные из текущей выбранной таблицы
        /// </summary>
        private TableData CurrentTableData
        {
            set; get;
        }
        /// <summary>
        /// Хранит текущее состояние элементов иерархического списка
        /// </summary>
        private Dictionary<int, bool> TreeState
        {
            set; get;
        }
        #endregion

        #region Методы

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
            //throw new NotImplementedException();
            return true;
        }

        public override object HandleCancel(List<CustomEventArgs> args)
        {
            //throw new NotImplementedException();
            return true;
        }

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

                ////var windowEdit = new WindowEdit("Создать продукт", new ProductsView(), EditModes.Update, 600, 800);
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
                var selectedProductViewCustom = this.SelectedItemTable as ProductsViewLiteWrappedCustom;
                Products product = this.CustomBase.CustomDb.DB.Products.FirstOrDefault(x => x.id == selectedProductViewCustom.ID_продукта);
                this.CustomBase.AddWithClearCurrentObjects(new CustomEventArgs(product));
                this.CustomBase.Mode = EditModes.Update;
                var windowEdit = new WindowEdit(
                    this.CustomBase,
                    Common.WindowSizes.SmallH320W400.Width,
                    Common.WindowSizes.SmallH320W400.Height
                    );
                _ = windowEdit.ShowDialog();
                if ((bool)windowEdit.DialogResult)
                {
                    this.CurrentTableData = this.GetTable(this.cmbTables.SelectedItem);
                    _ = this.FilterTableByTreeView(this.CurrentTableData.ItemsType, this.SelectedItemTreeView.Value);
                    _ = this.UpdateDataGrid(this.CurrentTableData);
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
            ViewsTablesView view = this.DB.ViewsTablesView.FirstOrDefault(x => x.Отображаемое_название_таблицы == tableName) ?? throw new Exception($"Не найдена таблица с именем {tableName}");

            // Кэшируем свойства
            var dbProperties = this.DB.GetType().GetProperties().Where(p => p.PropertyType.Name.StartsWith("DbSet")).ToDictionary(p => p.Name, p => p);

            if (!dbProperties.TryGetValue(view.Наименование_представления, out System.Reflection.PropertyInfo tableProperty))
                throw new Exception($"Не найдено свойство DbSet для таблицы {tableName}");

            IEnumerable dbSet = tableProperty.GetValue(this.DB) as IEnumerable ?? throw new Exception($"Не удалось получить значение DbSet для таблицы {tableName}");

            // Допускаем только один вызов GetGenericArguments
            Type[] genericArguments = dbSet.GetType().GetGenericArguments();

            if (!genericArguments.Any())
                throw new Exception("Не удалось получить тип данных для этой таблицы!");

            Type dbSetType = genericArguments.First();
            var columnNames = dbSetType.GetProperties().Select(p => p.Name).ToList();

            var tableData = new TableData(
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
                string selectedTableName = this.cmbTables.SelectedItem.ToString();
                TableData table = this.GetTable(selectedTableName);

                if (table != null && table.ItemsType != null)
                {
                    using (var writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
                    {
                        var entities = table.ItemsAll.OfType<object>().ToList();
                        if (entities.Any())
                        {
                            IEnumerable<string> propertyNames = table.ItemsType.GetProperties().Select(p => isDeleteUnderline ? p.Name.Replace("_", " ") : p.Name);

                            string separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

                            string header = string.Join(separator, propertyNames);
                            writer.WriteLine(header);

                            foreach (object entity in entities)
                            {
                                IEnumerable<string> values = propertyNames.Select(p =>
                                {
                                    string value = entity.GetType().GetProperty(isDeleteUnderline ? p.Replace(" ", "_") : p)?.GetValue(entity)?.ToString();
                                    return string.IsNullOrEmpty(value) ? value : $"\"{value.Replace("\"", "\"\"")}\"";
                                });

                                string line = string.Join(separator, values);
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
            if (this.CurrentTableData.ItemsType == typeof(ProductsViewLiteWrapped))
            {
                // Создание списка с одним элементом
                //var displayColumns = new List<string> {
                //    "Артикул",
                //    "Наименование_продукта",
                //    "Вес",
                //    "Количество_в_упаковке",
                //    "Вес_упаковки_с_товаром"
                //};
                //this.dgTable.TableData.SetDisplayColumns(displayColumns);
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
            foreach (ViewsTablesView table in tables)
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
            if (this.CurrentTableData.ItemsType != typeof(ProductsViewLiteWrapped))
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

            // Строим SQL запрос на основе фильтров
            var sqlQueryBuilder = new StringBuilder("SELECT * FROM ProductsViewLiteWrapped WHERE 1=1");

            if (!string.IsNullOrEmpty(className))
            {
                _ = sqlQueryBuilder.Append(" AND [Наименование класса] = @className");
            }
            if (!string.IsNullOrEmpty(groupName))
            {
                _ = sqlQueryBuilder.Append(" AND [Наименование группы] = @groupName");
            }
            if (!string.IsNullOrEmpty(subGroupName))
            {
                _ = sqlQueryBuilder.Append(" AND [Наименование подгруппы] = @subGroupName");
            }

            string sqlQuery = sqlQueryBuilder.ToString();
            var parameters = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(className))
            {
                parameters.Add(new SqlParameter("@className", className));
            }
            if (!string.IsNullOrEmpty(groupName))
            {
                parameters.Add(new SqlParameter("@groupName", groupName));
            }
            if (!string.IsNullOrEmpty(subGroupName))
            {
                parameters.Add(new SqlParameter("@subGroupName", subGroupName));
            }

            // Выполнение SQL запроса с параметрами
            List<ProductsViewLiteWrappedCustom> filteredProducts =
                this.CustomBase.CustomDb.ExecuteSqlQuery<ProductsViewLiteWrappedCustom>(sqlQuery, parameters);

            // Обновление ItemsAll с отфильтрованными продуктами
            this.CurrentTableData.ItemsAll = new ObservableCollection<object>(filteredProducts);
            return Task.CompletedTask;
        }



        /// <summary>
        /// Добавляет класс и связанные с ним группы и подгруппы в дерево представления.
        /// </summary>
        /// <param name="class">Экземпляр класса <see cref="Classes"/>, который будет добавлен в дерево представления.</param>
        /// <returns>Элемент дерева представления <see cref="TreeViewItemCustom"/> для добавленного класса.</returns>
        /// <exception cref="ArgumentNullException">Бросается, если переданный класс равен null.</exception>
        private TreeViewItemCustom AddToTreeView(Classes @class)
        {
            // Создание нового элемента для класса
            var classItem = new TreeViewItemCustom(@class.id, @class.Descriptors.title, @class);

            // Проверка, что свойство Groups класса не равно null и содержит элементы
            if (@class.Groups != null)
            {
                foreach (Groups group in @class.Groups)
                {
                    // Создание нового элемента для группы
                    var groupItem = new TreeViewItemCustom(group.id, group.Descriptors.title, group);

                    // Проверка, что свойство SubGroups группы не равно null и содержит элементы
                    if (group.SubGroups != null)
                    {
                        foreach (SubGroups subGroup in group.SubGroups)
                        {
                            // Создание и добавление нового элемента для подгруппы
                            groupItem.Add(new TreeViewItemCustom(subGroup.id, subGroup.Descriptors.title, subGroup));
                        }
                    }

                    // Добавление группы к классу
                    classItem.Add(groupItem);
                }
            }

            // Добавление класса к дереву представления
            _ = this.tvGroups.Items.Add(classItem);

            // Возвращение созданного элемента TreeViewItemCustom
            return classItem;
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
            foreach (object obj in items)
            {
                var item = obj as TreeViewItemCustom;
                if (item == null)
                    continue;

                if (item.Value is Classes @class)
                {
                    this.TreeState[@class.id] = item.IsExpanded;
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
                this.CurrentTableData.ItemsType == typeof(ProductsViewLiteWrapped))
            {
                // Сохраняем текущее состояние дерева перед его очисткой
                this.TreeState = new Dictionary<int, bool>();
                this.SaveTreeState(this.tvGroups.Items);

                this.tvGroups.Items.Clear();
                this.tvGroups.UpdateLayout();
                //this.InitDB(this.IsCollectionUpdated); // Если этот метод может быть асинхронным, добавьте await

                // Если доступ к базе данных является блокирующей операцией, используйте асинхронные методы.
                var classes = await Task.Run(() => this.DB.Classes.ToList());

                foreach (Classes @class in classes)
                {
                    TreeViewItemCustom classItem = this.AddToTreeView(@class);
                    // Восстанавливаем состояние для каждого элемента
                    if (this.TreeState.TryGetValue(@class.id, out bool isExpanded))
                    {
                        classItem.IsExpanded = isExpanded;
                    }
                }

                //Выбор первого элемента, если есть
                if (this.tvGroups.Items.Count > 0)
                {
                    var firstItem = (TreeViewItem)this.tvGroups.ItemContainerGenerator.ContainerFromIndex(0);
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
        /// <summary>
        /// Обновляет кнопки контекстного меню иерархического списка
        /// </summary>
        private void UpdateContextMenu()
        {
            Dictionary<System.Windows.Controls.MenuItem, ControlState> controlStates =
                new Dictionary<System.Windows.Controls.MenuItem, ControlState>()
                {
                    { this.miTreeAddPrimary, new ControlState() },
                    { this.miTreeAddSecondary, new ControlState() },
                    { this.miTreeUpdate, new ControlState() },
                    { this.miTreeDelete, new ControlState() }
                };
            object selectedItemTreeViewObject = this.CustomBase.UnpackCurrentObject(this.SelectedItemTreeView);
            if (selectedItemTreeViewObject.ValidateTypeOrBaseType<Classes>())
            {
                controlStates[this.miTreeAddPrimary] = new ControlState(this.CustomBase.GetTitle(EditModes.Create, new Classes()), true, true);
                controlStates[this.miTreeAddSecondary] = new ControlState(this.CustomBase.GetTitle(EditModes.Create, new Groups()), true, true);
                controlStates[this.miTreeUpdate] = new ControlState(this.CustomBase.GetTitle(EditModes.Update, new Classes()), true, true);
                controlStates[this.miTreeDelete] = new ControlState(this.CustomBase.GetTitle(EditModes.Delete, new Classes()), true, true);
            }
            if (selectedItemTreeViewObject.ValidateTypeOrBaseType<Groups>())
            {
                controlStates[this.miTreeAddPrimary] = new ControlState(this.CustomBase.GetTitle(EditModes.Create, new SubGroups()), true, true);
                controlStates[this.miTreeAddSecondary] = new ControlState(this.CustomBase.GetTitle(EditModes.Create, new Groups()), false, false);
                controlStates[this.miTreeUpdate] = new ControlState(this.CustomBase.GetTitle(EditModes.Update, new Groups()), true, true);
                controlStates[this.miTreeDelete] = new ControlState(this.CustomBase.GetTitle(EditModes.Delete, new Groups()), true, true);
            }
            if (selectedItemTreeViewObject.ValidateTypeOrBaseType<SubGroups>())
            {
                controlStates[this.miTreeAddPrimary] = new ControlState(this.CustomBase.GetTitle(EditModes.Create, new SubGroups()), true, true);
                controlStates[this.miTreeAddSecondary] = new ControlState(this.CustomBase.GetTitle(EditModes.Create, new SubGroups()), false, false);
                controlStates[this.miTreeUpdate] = new ControlState(this.CustomBase.GetTitle(EditModes.Update, new SubGroups()), true, true);
                controlStates[this.miTreeDelete] = new ControlState(this.CustomBase.GetTitle(EditModes.Delete, new SubGroups()), true, true);
            }
            #region Актвация кнопок
            foreach (var item in controlStates)
            {
                item.Value.UpdateControl(item.Key);
            }
            #endregion
        }
        /// <summary>
        /// Обрабатывает нажатие кнопки "Добавить" в контекстном меню иерархического списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEventHandler(object sender, RoutedEventArgs e)
        {
            object selectedItemTreeViewObject = this.CustomBase.UnpackCurrentObject(this.SelectedItemTreeView);
            if (selectedItemTreeViewObject != null)
            {
                if (selectedItemTreeViewObject.ValidateTypeOrBaseType<Classes>())
                {
                    var group = new Groups();
                    group.Classes = selectedItemTreeViewObject as Classes;
                    this.CustomBase.AddWithClearCurrentObjects(group);
                }
                if (selectedItemTreeViewObject.ValidateTypeOrBaseType<Groups>() || selectedItemTreeViewObject.ValidateTypeOrBaseType<SubGroups>())
                {
                    var subGroup = new SubGroups();
                    subGroup.Groups = selectedItemTreeViewObject as Groups;
                    this.CustomBase.AddWithClearCurrentObjects(subGroup);
                }
            }
            else
            {
                this.CustomBase.AddWithClearCurrentObjects(new Classes());
            }
            //this.CustomBase.AddWithClearCurrentObjects(new CustomEventArgs(this.SelectedItemTreeView));
            this.CustomBase.Mode = EditModes.Create;
            var windowEdit = new WindowEdit(
                Common.Strings.Titles.Windows.add,
                this.CustomBase,
                Common.WindowSizes.SmallH320W400.Width,
                Common.WindowSizes.SmallH320W400.Height
                );
            _ = windowEdit.ShowDialog();
            if ((bool)windowEdit.DialogResult)
                _ = this.UpdateTreeView();
        }

        #endregion

        #endregion

        #region Конструкторы/Деструкторы
        public PageTables(CustomBase customBase) : base(customBase)
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

        private void cmbTables_SelectionChanged(object sender, RoutedEventArgs e)
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

                    string header = this.CustomBase.GetTitle(EditModes.Create, new Classes());
                    var selectedObject = this.CustomBase.UnpackCurrentObject(this.SelectedItemTreeView);
                    if (selectedObject != null)
                    {
                        if (selectedObject.ValidateTypeOrBaseType<Classes>())
                            header = this.CustomBase.GetTitle(EditModes.Create, new Groups());
                        if (selectedObject.ValidateTypeOrBaseType<Groups>() || selectedObject.ValidateTypeOrBaseType<SubGroups>())
                            header = this.CustomBase.GetTitle(EditModes.Create, new SubGroups());
                    }
                    this.miTreeAddPrimary.Header = header;
                    await this.FilterTableByTreeView(this.CurrentTableData.ItemsType, selectedObject);
                    await this.UpdateDataGrid(this.CurrentTableData);
                }
                this.UpdateContextMenu();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void tvGroups_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

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
                AddEventHandler(sender, e);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void miTreeAddSecondary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddEventHandler(sender, e);
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
                this.CustomBase.AddWithClearCurrentObjects(new CustomEventArgs(this.SelectedItemTreeView.Value));
                this.CustomBase.Mode = EditModes.Update;
                var windowEdit = new WindowEdit(
                    Common.Strings.Titles.Windows.edit,
                    this.CustomBase,
                    Common.WindowSizes.SmallH320W400.Width,
                    Common.WindowSizes.SmallH320W400.Height
                    );
                _ = windowEdit.ShowDialog();
                if ((bool)windowEdit.DialogResult)
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
                if (this.SelectedItemTreeView == null || this.SelectedItemTreeView.Value == null)
                    throw new Exception("Не выбран элемент для удаления!");

                MessageBoxResult result = System.Windows.MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    object obj = this.SelectedItemTreeView.Value;
                    if (obj.ValidateTypeOrBaseType<Classes>())
                    {
                        _ = this.CustomBase.CustomDb.DeleteClass(((Classes)obj).id);
                    }
                    if (obj.ValidateTypeOrBaseType<Groups>())
                    {
                        this.CustomBase.CustomDb.DeleteGroup(((Groups)obj).id);
                    }
                    if (obj.ValidateTypeOrBaseType<SubGroups>())
                    {
                        this.CustomBase.CustomDb.DeleteSubGroup(((SubGroups)obj).id);
                    }
                    _ = this.UpdateTreeView();
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        #endregion

        #endregion

        #region Кнопки контекстного меню таблицы
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

                    string selectedTableName = "";
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
                        string outputFilePath = dialog.FileName;
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
                if (this.CurrentTableData.ItemsType != typeof(ProductsViewLiteWrapped))
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
