using System.Collections.Generic;
using System.IO;
using System.Text.Json;
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
        private const string DefaultSettingsFilePath = "settings.json";
        public MainWindow() : base("MainWindow", new Settings(DefaultSettingsFilePath))
        {
            this.InitializeComponent();
            #region Set window size to 3/4 of the screen size
            this.Width = SystemParameters.PrimaryScreenWidth * 0.75;
            this.Height = SystemParameters.PrimaryScreenHeight * 0.75;
            #endregion
            this.fTables.Content = new PageTables(this.CustomBase);
            this.fImport.Content = new PageImport(this.CustomBase);
            this.fSettings.Content = new PageSettings(this.CustomBase);
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
