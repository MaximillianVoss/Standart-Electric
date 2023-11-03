
using BaseWindow_WPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
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
        public object CurrentItem { get; set; }
        #endregion

        #region Методы
        private void UpdateFields(object obj)
        {
            Classes currentClass = null;
            if (obj.GetType().BaseType == typeof(TreeViewItemCustom))
            {
                currentClass = ((TreeViewItemCustom)obj).Value as Classes;
            }
            if (obj.GetType().BaseType == typeof(Classes))
                currentClass = obj as Classes;
            if (currentClass != null)
            {
                ClassesView classView = this.DB.ClassesView.FirstOrDefault(x => x.ID_класса == currentClass.id);
                if (classView == null)
                    throw new System.Exception($"Не удалось найти класс с id:{currentClass.id} ");
                this.txbTitle.Text = classView.Наименование_класса;
                this.txbTitleShort.Text = classView.Сокращенное_наименование_класса;
                this.txbCode.Text = classView.Код_класса;
                this.txbDescription.Text = classView.Описание_класса;
                //this.txbUrlPicture.Text = classView.URL_изображения_класса;
            }
        }

        public void Save()
        {
            Descriptors descriptor;

            // Проверяем, задан ли CurrentItem
            if (this.CurrentItem != null)
            {
                // Проверяем, является ли CurrentItem объектом Classes
                if (!(this.CurrentItem is Classes currentClass))
                    throw new Exception("Редактируемый элемент не является классом");
                this.CustomBase.CustomDb.UpdateClass(

                    );

                // Сохраняем descriptor
                descriptor = this.Save(
                    currentClass.idDescriptor,
                    this.txbCode.Text,
                    this.txbTitle.Text,
                    this.txbTitleShort.Text,
                    "",
                    this.txbDescription.Text
                );

                // Обновляем текущий класс
                currentClass.Descriptors = descriptor;
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

                // Создаем новый объект Classes и добавляем его в базу данных
                this.DB.Classes.Add(new Classes(descriptor));
            }

            // Сохраняем изменения в базу данных
            this.DB.SaveChanges();
        }

        #endregion

        #region Конструкторы/Деструкторы
        public PageEditClass(List<CustomEventArgs> currentObjects, EditModes mode) : base()
        {
            this.InitializeComponent();
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
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {


        }
        private void gMain_Loaded(object sender, RoutedEventArgs e)

        {

        }
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
