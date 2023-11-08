using BaseWindow_WPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы.Страницы.Редактирование
{
    /// <summary>
    /// Логика взаимодействия для PageEditSubGroup.xaml
    /// </summary>
    public partial class PageEditSubGroup : CustomPage
    {


        #region Поля

        #endregion

        #region Свойства
        public object CurrentItem { get; set; }
        #endregion

        #region Методы
        private void UpdateComboBoxGroup()
        {
            this.cmbGroup.Items = this.CustomBase.ToList(this.DB.Groups.ToList());
        }
        private void UpdateComboBoxLoadDiagram()
        {
            this.cmbLoadDiagram.Items = this.CustomBase.ToList(this.DB.LoadDiagrams.ToList());
        }
        private void UpdateComboBoxApplication()
        {
            this.cmbApplication.Items = this.CustomBase.ToList(this.DB.Applications.ToList());
        }
        private void UpdateFields(object obj)
        {
            SubGroups currentSubGroup = null;
            if (obj.GetType().BaseType == typeof(TreeViewItemCustom))
            {
                currentSubGroup = ((TreeViewItemCustom)obj).Value as SubGroups;
            }
            if (obj.GetType().BaseType == typeof(SubGroups))
                currentSubGroup = obj as SubGroups;
            if (currentSubGroup != null)
            {
                SubGroupsView subGroupView = this.DB.SubGroupsView.FirstOrDefault(x => x.ID_подгруппы == currentSubGroup.id);
                if (subGroupView == null)
                    throw new System.Exception($"Не удалось найти группу с id:{currentSubGroup.id} ");
                this.txbTitle.Text = subGroupView.Наименование_подгруппы;
                this.txbTitleShort.Text = subGroupView.Сокращенное_наименование_подгруппы;
                this.txbCode.Text = subGroupView.Код_подгруппы;
                this.txbDescription.Text = subGroupView.Описание_подгруппы;
                //this.txbUrlPicture.Text = classView.URL_изображения_класса;
                GroupsView group = this.DB.GroupsView.FirstOrDefault(x => x.ID_группы == subGroupView.ID_группы);
                if (group != null)
                    this.cmbGroup.Select(group.ID_группы);
                LoadDiagramsView loadDiagram = this.DB.LoadDiagramsView.FirstOrDefault(x => x.ID_схемы_нагрузок == subGroupView.ID_схемы_нагрузок);
                if (loadDiagram != null)
                    this.cmbLoadDiagram.Select(loadDiagram.ID_схемы_нагрузок);
                GroupsApplications groupsApplications = this.DB.GroupsApplications.FirstOrDefault(x => x.SubGroups.id == subGroupView.ID_подгруппы);
                if (groupsApplications != null)
                    this.cmbApplication.Select(groupsApplications.Applications.id);
            }
        }
        private void Save()
        {
            //// Объявляем объект descriptor здесь, так как он будет использоваться в обоих случаях
            //Descriptors descriptor;

            //// Проверяем, задан ли CurrentObject
            //if (this.CurrentObject != null)
            //{
            //    // Проверяем, является ли CurrentObject объектом Groups
            //    if (!(this.CurrentObject is SubGroups currentSubGroup))
            //        throw new Exception("Редактируемый элемент не является классом");

            //    // Извлекаем соответствующую группу из базы данных
            //    currentSubGroup = this.DB.SubGroups.FirstOrDefault(x => x.id == currentSubGroup.id);

            //    сохраняем descriptor
            //    descriptor = this.save(
            //        currentsubgroup.iddescriptor,
            //        this.txbcode.text,
            //        this.txbtitle.text,
            //        this.txbtitleshort.text,
            //        "",
            //        this.txbdescription.text
            //        );

            //    // Обновляем текущую группу
            //    currentSubGroup.Descriptors = descriptor;
            //    currentSubGroup.Groups = this.DB.Groups.FirstOrDefault(x => x.id == this.cmbGroup.SelectedId);
            //    currentSubGroup.LoadDiagrams = this.DB.LoadDiagrams.FirstOrDefault(x => x.id == this.cmbLoadDiagram.SelectedId);
            //    var subGroupApplication = this.DB.GroupsApplications.FirstOrDefault(x => x.idSubGroup == currentSubGroup.id);
            //    if (subGroupApplication != null)
            //    {
            //        subGroupApplication.Applications = this.DB.Applications.FirstOrDefault(x => x.id == this.cmbApplication.SelectedId);
            //    }
            //    else
            //    {
            //        this.DB.GroupsApplications.Add(new GroupsApplications(
            //            currentSubGroup,
            //            this.DB.Applications.FirstOrDefault(x => x.id == this.cmbApplication.SelectedId)
            //            ));
            //    }
            //}
            //else
            //{
            //    // Создаем новый объект Descriptors и добавляем его в базу данных
            //    descriptor = new Descriptors(
            //        this.txbCode.Text,
            //        this.txbTitle.Text,
            //        this.txbTitleShort.Text,
            //        this.txbDescription.Text
            //        );

            //    descriptor = this.DB.Descriptors.Add(descriptor);

            //    // Создаем новый объект Groups и добавляем его в базу данных
            //    this.DB.SubGroups.Add(new SubGroups(
            //        descriptor,
            //        this.DB.Groups.FirstOrDefault(x => x.id == this.cmbGroup.SelectedId),
            //        this.DB.LoadDiagrams.FirstOrDefault(x => x.id == this.cmbLoadDiagram.SelectedId)
            //        ));

            //}

            //// Сохраняем изменения в базе данных
            //this.DB.SaveChanges();
        }
        #endregion

        #region Конструкторы/Деструкторы
        public PageEditSubGroup(object item = null)
        {
            this.InitializeComponent();
            this.UpdateComboBoxGroup();
            this.UpdateComboBoxLoadDiagram();
            this.UpdateComboBoxApplication();
            this.CurrentItem = item;
            this.btnOk.Text = this.CurrentItem != null ?
            Common.Strings.Titles.Controls.Buttons.saveChanges :
            Common.Strings.Titles.Controls.Buttons.createItem;
            if (this.CurrentItem != null)
                this.UpdateFields(this.CurrentItem);
        }


        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
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
        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CloseWindow();
        }

        public override void UpdateFields(List<CustomEventArgs> args)
        {
            throw new NotImplementedException();
        }

        public override void UpdateForm(List<CustomEventArgs> args)
        {
            throw new NotImplementedException();
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
