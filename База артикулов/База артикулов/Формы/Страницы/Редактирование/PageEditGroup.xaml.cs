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

        public override void UpdateFields(List<CustomEventArgs> args)
        {
            Groups currentGroup = this.CustomBase.UnpackCurrentObject<Groups>(this.CurrentObject);
            if (currentGroup != null)
            {
                if (this.CustomBase.Mode == EditModes.Update)
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
                    //throw new Exception("Проверить этот код с URL класса");
                    //this.txbUrlPicture.Text = classView.URL_изображения_класса;
                }
                if (this.CustomBase.Mode == EditModes.Create)
                {
                    if (currentGroup.Classes != null)
                        this.cmbClass.Select(currentGroup.Classes.id);

                }
            }
        }

        public override void UpdateForm(List<CustomEventArgs> args)
        {
            this.InitializeComponent();
            this.CustomBase.UpdateOkButton(this.btnOk);
            this.CustomBase.UpdateComboBox(this.cmbClass, this.CustomBase.ToList(this.DB.Classes.ToList()));
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
            if (this.CustomBase.Mode == EditModes.Update)
            {
                Groups group = this.CustomBase.UnpackCurrentObject<Groups>(this.CurrentObject);
                if (group == null)
                {
                    throw new Exception(Common.Strings.Errors.failedToGetParam);
                }
                this.CustomBase.CustomDb.UpdateGroup(
                    group.id,
                     this.txbCode.Text,
                    this.txbTitle.Text,
                    this.txbTitleShort.Text,
                    this.txbDescription.Text,
                    this.cmbClass.SelectedId ?? this.CustomBase.CustomDb.DB.Classes.First().id
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
