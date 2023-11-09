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

        public override void UpdateFields(List<CustomEventArgs> args)
        {
            Groups currentGroup = null;
            //if (obj.GetType().BaseType == typeof(TreeViewItemCustom))
            //{
            //    currentGroup = ((TreeViewItemCustom)obj).Value as Groups;
            //}
            if (this.CurrentObject.Data.IsTypeOrBaseEqual(typeof(Groups)))
                currentGroup = this.CurrentObject.Data as Groups;
            if (currentGroup != null)
            {
                GroupsView groupView = this.DB.GroupsView.FirstOrDefault(x => x.ID_группы == currentGroup.id);
                if (groupView == null)
                    throw new System.Exception($"Не удалось найти группу с id:{currentGroup.id} ");
                this.txbTitle.Text = groupView.Наименование_группы;
                this.txbTitleShort.Text = groupView.Сокращенное_наименование_группы;
                this.txbCode.Text = groupView.Код_группы;
                this.txbDescription.Text = groupView.Описание_группы;
                ClassesView @class = this.DB.ClassesView.FirstOrDefault(x => x.ID_класса == groupView.ID_класса);
                if (@class != null)
                    this.cmbClass.Select(@class.ID_класса);
                throw new Exception("Проверить этот код с URL класса");
                //this.txbUrlPicture.Text = classView.URL_изображения_класса;
            }
        }

        public override void UpdateForm(List<CustomEventArgs> args)
        {
            this.btnOk.Text = this.CurrentObject != null ?
          Common.Strings.Titles.Controls.Buttons.saveChanges :
          Common.Strings.Titles.Controls.Buttons.createItem;
            this.UpdateComboBox();
        }

        public override object HandleOk(List<CustomEventArgs> args)
        {
            if (this.CustomBase.Mode == EditModes.Create)
            {
                this.CustomBase.Result.Data = this.CustomBase.CustomDb.CreateGroup(
                    this.txbCode.Text,
                    this.txbTitle.Text,
                    this.txbTitleShort.Text,
                    this.txbDescription.Text,
                    this.cmbClass.SelectedId ?? this.CustomBase.CustomDb.DB.Classes.First().id
                );
            }
            if (this.CustomBase.Mode == EditModes.Edit)
            {
                if (!this.CurrentObject.Data.IsTypeOrBaseEqual(typeof(Groups)))
                    throw new Exception("Редактируемый элемент не является группой");
                var group = this.CurrentObject.Data as Groups;
                this.CustomBase.CustomDb.UpdateGroup(
                    group.id,
                     this.txbCode.Text,
                    this.txbTitle.Text,
                    this.txbTitleShort.Text,
                    this.txbDescription.Text,
                    this.cmbClass.SelectedId ?? this.CustomBase.CustomDb.DB.Classes.First().id
                    );
            }
            this.CustomBase.Result.Data = true;
            return true;
        }

        public override object HandleCancel(List<CustomEventArgs> args)
        {
            return false;
        }
        #endregion

        #region Конструкторы/Деструкторы
        public PageEditGroup(CustomBase customBase, int width = 600, int height = 800) : base(customBase)
        {
            this.SetSize(width, height);
            this.InitializeComponent();
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.ProcessOk();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.ProcessCancel();
        }
        #endregion

    }
}
