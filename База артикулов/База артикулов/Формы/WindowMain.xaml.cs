using System.Collections.Generic;
using System.Windows;
using База_артикулов.Классы;
//using База_артикулов.Properties;
using База_артикулов.Формы.Страницы;

namespace База_артикулов.Формы
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : CustomWindow
    {
        public MainWindow() : base("MainWindow", new Settings("settings.json"))
        {
            this.InitializeComponent();
            #region Set window size to 3/4 of the screen size
            this.Width = SystemParameters.PrimaryScreenWidth * 0.75;
            this.Height = SystemParameters.PrimaryScreenHeight * 0.75;
            #endregion
            this.fTables.Content = new PageTables(this.CustomBase);
            this.fImport.Content = new PageImport();
            this.fSettings.Content = new PageSettings();
        }

        public override object HandleCancel()
        {
            //throw new System.NotImplementedException();
            return true;
        }

        public override object HandleOk()
        {
            //throw new System.NotImplementedException();
            return true;
        }

        public override void UpdateFields(List<CustomEventArgs> args = null)
        {
            //throw new System.NotImplementedException();
        }

        public override void UpdateForm(List<CustomEventArgs> args = null)
        {
            //throw new System.NotImplementedException();
        }
    }
}
