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
            this.fTables.Content = new PageTables();
            this.fImport.Content = new PageImport();
        }
    }
}
