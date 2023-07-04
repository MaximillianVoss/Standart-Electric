using System.Linq;
using System.Windows;
using База_артикулов.Модели;

namespace База_артикулов.Формы
{
    /// <summary>
    /// Тестовое окно для тестирования функуионала других окон
    /// </summary>
    public partial class TestWindow : CustomWindow
    {


        #region Поля

        #endregion

        #region Свойства

        #endregion

        #region Методы

        #endregion

        #region Конструкторы/Деструкторы
        public TestWindow()
        {
            this.InitializeComponent();
        }


        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ProductUnitsWindow productUnitsWindow = new ProductUnitsWindow(1);
            //productUnitsWindow.Show();
            //FilesWindow filesWindow = new FilesWindow();
            //filesWindow.Show();
            ProductWindow productWindow = new ProductWindow(typeof(Products), this.DB.Products.First().id);
            productWindow.ShowDialog();

        }
        #endregion

    }
}
