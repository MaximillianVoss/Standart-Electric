using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using База_артикулов.Классы;
using База_артикулов.Модели;

namespace База_артикулов.Формы.Страницы.Редактирование
{
    /// <summary>
    /// Логика взаимодействия для PageEditClass.xaml
    /// </summary>
    public partial class PageEditClass : CustomPage
    {


        #region Поля

        #endregion

        #region Свойства

        #endregion

        #region Методы
        public override void UpdateFields(List<CustomEventArgs> args)
        {
            this.InitializeComponent();
            if (this.CustomBase.Mode == EditModes.Update)
            {
                var currentClass = this.CustomBase.UnpackCurrentObject<Classes>(this.CurrentObject);
                if (currentClass != null)
                {
                    ClassesView classView = this.DB.ClassesView.FirstOrDefault(x => x.ID_класса == currentClass.id);
                    if (classView == null)
                        throw new Exception($"Не удалось найти класс с id:{currentClass.id} ");
                    this.txbTitle.Text = classView.Наименование_класса;
                    this.txbTitleShort.Text = classView.Сокращенное_наименование_класса;
                    this.txbCode.Text = classView.Код_класса;
                    this.txbDescription.Text = classView.Описание_класса;
                }
            }
        }

        public override void UpdateForm(List<CustomEventArgs> args)
        {
            this.InitializeComponent();
            this.CustomBase.UpdateOkButton(this.btnOk);
        }

        public override object HandleOk(List<CustomEventArgs> args)
        {
            if (this.CustomBase.Mode == EditModes.Create)
            {
                this.CustomBase.CustomDb.CreateClass(
                        this.txbCode.Text,
                        this.txbTitle.Text,
                        this.txbTitleShort.Text,
                        this.txbDescription.Text
                    );

            }
            if (this.CustomBase.Mode == EditModes.Update)
            {
                var currentClass = this.CustomBase.UnpackCurrentObject<Classes>(this.CurrentObject);
                if (currentClass == null)
                {
                    throw new Exception(Common.Strings.Errors.failedToGetParam);
                }
                this.CustomBase.Result.Data = this.CustomBase.CustomDb.UpdateClass(
                        currentClass.id,
                        this.txbCode.Text,
                        this.txbTitle.Text,
                        this.txbTitleShort.Text,
                        this.txbDescription.Text
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
        public PageEditClass(CustomBase customBase, int width = 600, int height = 800) : base(customBase)
        {
            this.InitializeComponent();
            this.SetSize(width, height);
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {


        }
        private void gMain_Loaded(object sender, RoutedEventArgs e)
        {

        }
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
