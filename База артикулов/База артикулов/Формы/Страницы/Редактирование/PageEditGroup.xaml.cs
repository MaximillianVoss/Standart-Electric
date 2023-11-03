using BaseWindow_WPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы.Страницы.Редактирование
{
    /// <summary>
    /// Логика взаимодействия для PageEditGroup.xaml
    /// </summary>
    public partial class PageEditGroup : CustomPage
    {

        #region Поля

        #endregion

        #region Свойства
        public object CurrentItem { get; set; }
        #endregion

        #region Методы
        private void UpdateComboBox()
        {
            this.cmbClass.Items.Clear();
            List<object> objClasses = new List<object>();
            foreach (var item in this.DB.Classes.ToList())
            {
                objClasses.Add(item.ToObject());
            }
            this.cmbClass.Items = objClasses;
        }
        private void UpdateFields(object obj)
        {
            Groups currentGroup = null;
            if (obj.GetType().BaseType == typeof(TreeViewItemCustom))
            {
                currentGroup = ((TreeViewItemCustom)obj).Value as Groups;
            }
            if (obj.GetType().BaseType == typeof(Groups))
                currentGroup = obj as Groups;
            if (currentGroup != null)
            {
                GroupsView groupView = this.DB.GroupsView.FirstOrDefault(x => x.ID_группы == currentGroup.id);
                if (groupView == null)
                    throw new System.Exception($"Не удалось найти группу с id:{currentGroup.id} ");
                this.txbTitle.Text = groupView.Наименование_группы;
                this.txbTitleShort.Text = groupView.Сокращенное_наименование_группы;
                this.txbCode.Text = groupView.Код_группы;
                this.txbDescription.Text = groupView.Описание_группы;
                //this.txbUrlPicture.Text = classView.URL_изображения_класса;
                ClassesView @class = this.DB.ClassesView.FirstOrDefault(x => x.ID_класса == groupView.ID_класса);
                if (@class != null)
                    this.cmbClass.Select(@class.ID_класса);
            }
        }
        private void Save()
        {
            // Объявляем объект descriptor здесь, так как он будет использоваться в обоих случаях
            Descriptors descriptor;

            // Проверяем, задан ли CurrentItem
            if (this.CurrentItem != null)
            {
                // Проверяем, является ли CurrentItem объектом Groups
                if (!(this.CurrentItem is Groups currentGroup))
                    throw new Exception("Редактируемый элемент не является классом");

                // Извлекаем соответствующую группу из базы данных
                currentGroup = this.DB.Groups.FirstOrDefault(x => x.id == currentGroup.id);

                // Сохраняем descriptor
                descriptor = this.Save(
                    currentGroup.idDescriptor,
                    this.txbCode.Text,
                    this.txbTitle.Text,
                    this.txbTitleShort.Text,
                    "",
                    this.txbDescription.Text
                    );

                // Обновляем текущую группу
                currentGroup.Descriptors = descriptor;
                currentGroup.Classes = this.DB.Classes.FirstOrDefault(x => x.id == this.cmbClass.SelectedId);
            }
            else
            {
                // Создаем новый объект Descriptors и добавляем его в базу данных
                descriptor = new Descriptors(
                    this.txbCode.Text,
                    this.txbTitle.Text,
                    this.txbTitleShort.Text,
                    this.txbDescription.Text
                    );

                descriptor = this.DB.Descriptors.Add(descriptor);

                // Создаем новый объект Groups и добавляем его в базу данных
                this.DB.Groups.Add(new Groups(descriptor, this.DB.Classes.FirstOrDefault(x => x.id == this.cmbClass.SelectedId)));
            }

            // Сохраняем изменения в базе данных
            this.DB.SaveChanges();
        }

        #endregion

        #region Конструкторы/Деструкторы
        public PageEditGroup(object item = null)
        {
            this.InitializeComponent();
            this.CurrentItem = item;
            this.btnOk.Text = this.CurrentItem != null ?
            Common.Strings.Titles.Controls.Buttons.saveChanges :
            Common.Strings.Titles.Controls.Buttons.createItem;
            this.UpdateComboBox();
            if (this.CurrentItem != null)
                this.UpdateFields(this.CurrentItem);
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Save();
                this.CloseWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.CloseWindow();
        }
        #endregion

    }
}
