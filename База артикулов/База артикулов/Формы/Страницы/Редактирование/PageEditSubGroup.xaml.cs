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

        public override void UpdateFields(List<CustomEventArgs> args)
        {
            if (this.CustomBase.Mode == EditModes.Update)
            {
                var obj = this.CurrentObject.Data;
                SubGroups currentSubGroup = null;
                if (obj.ValidateTypeOrBaseType<TreeViewItemCustom>())
                    currentSubGroup = ((TreeViewItemCustom)obj).Value as SubGroups;
                if (obj.ValidateTypeOrBaseType<CustomEventArgs>())
                    currentSubGroup = ((CustomEventArgs)obj).Data as SubGroups;
                if (obj.ValidateTypeOrBaseType<SubGroups>())
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
        }

        public override void UpdateForm(List<CustomEventArgs> args)
        {
            this.InitializeComponent();
            this.UpdateComboBoxGroup();
            this.UpdateComboBoxLoadDiagram();
            this.UpdateComboBoxApplication();
            this.btnOk.Text = this.CurrentObject != null ?
         Common.Strings.Titles.Controls.Buttons.saveChanges :
         Common.Strings.Titles.Controls.Buttons.createItem;
        }

        public override object HandleOk(List<CustomEventArgs> args)
        {
            if (this.CustomBase.Mode == EditModes.Create)
            {
                this.CustomBase.Result.Data = this.CustomBase.CustomDb.CreateSubGroup(
                    this.txbCode.Text,
                    this.txbTitle.Text,
                    this.txbTitleShort.Text,
                    this.txbDescription.Text,
                    this.cmbGroup.SelectedId ?? this.CustomBase.CustomDb.DB.Groups.First().id,
                    this.cmbLoadDiagram.SelectedId ?? this.CustomBase.CustomDb.DB.LoadDiagrams.First().id,
                    this.cmbApplication.SelectedId ?? this.CustomBase.CustomDb.DB.Applications.First().id
                    );

            }
            if (this.CustomBase.Mode == EditModes.Update)
            {
                var group = this.CustomBase.UnpackCurrentObject<SubGroups>(this.CurrentObject);
                if (group == null)
                {
                    throw new Exception(Common.Strings.Errors.failedToGetParam);
                }
                this.CustomBase.CustomDb.UpdateSubGroup(
                    group,
                    this.txbCode.Text,
                    this.txbTitle.Text,
                    this.txbTitleShort.Text,
                    this.txbDescription.Text,
                    this.cmbGroup.SelectedId ?? this.CustomBase.CustomDb.DB.Groups.First().id,
                    this.cmbLoadDiagram.SelectedId ?? this.CustomBase.CustomDb.DB.LoadDiagrams.First().id,
                    this.cmbApplication.SelectedId ?? this.CustomBase.CustomDb.DB.Applications.First().id
                    );
                this.CustomBase.Result.Data = true;
            }
            return true;
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

        public override object HandleCancel(List<CustomEventArgs> args)
        {
            return false;
        }
        #endregion

        #region Конструкторы/Деструкторы
        public PageEditSubGroup(CustomBase customBase, int width = 600, int height = 800) : base(customBase)
        {
            this.SetSize(width, height);
            this.InitializeComponent();
        }


        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ProcessOk();
        }
        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ProcessCancel();
        }
        #endregion

    }
}
