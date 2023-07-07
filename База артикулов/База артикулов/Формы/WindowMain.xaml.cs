using System.Windows;
using База_артикулов.Формы.Страницы;

namespace База_артикулов.Формы
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : CustomWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            #region Set window size to 3/4 of the screen size
            this.Width = SystemParameters.PrimaryScreenWidth * 0.75;
            this.Height = SystemParameters.PrimaryScreenHeight * 0.75;
            #endregion
            this.fTables.Content = new PageTables();
            this.fImport.Content = new PageImport();
        }
    }
}
