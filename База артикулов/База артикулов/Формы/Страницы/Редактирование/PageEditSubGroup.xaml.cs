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

        public override void UpdateFields(List<CustomEventArgs> args)
        {
            SubGroups currentSubGroup = this.CustomBase.UnpackCurrentObject<SubGroups>(this.CurrentObject);
            if (currentSubGroup != null)
            {
                if (this.CustomBase.Mode == EditModes.Update)
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
                if (this.CustomBase.Mode == EditModes.Create)
                {
                    if (currentSubGroup.Groups != null)
                        this.cmbGroup.Select(currentSubGroup.Groups.id);
                }
            }
        }

        public override void UpdateForm(List<CustomEventArgs> args)
        {
            this.InitializeComponent();
            this.CustomBase.UpdateComboBox(this.cmbGroup, this.CustomBase.ToList(this.CustomBase.CustomDb.DB.Groups.Where(x => x.Classes != null).ToList()));
            this.CustomBase.UpdateComboBox(this.cmbApplication, this.CustomBase.ToList(this.CustomBase.CustomDb.DB.Applications.ToList()));
            this.CustomBase.UpdateComboBox(this.cmbLoadDiagram, this.CustomBase.ToList(this.CustomBase.CustomDb.DB.LoadDiagrams.ToList()));
            this.CustomBase.UpdateOkButton(this.btnOk);
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
                SubGroups group = this.CustomBase.UnpackCurrentObject<SubGroups>(this.CurrentObject);
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
