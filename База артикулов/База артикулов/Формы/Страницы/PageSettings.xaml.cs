using System;
using System.Configuration;
using База_артикулов.Классы;

namespace База_артикулов.Формы.Страницы
{
    /// <summary>
    /// Логика взаимодействия для PageSettings.xaml
    /// </summary>
    public partial class PageSettings : CustomPage
    {


        #region Поля

        #endregion

        #region Свойства

        #endregion

        #region Методы
        private void UpdateConnectionsComboBox()
        {
            ConnectionStringSettingsCollection connectionStrings = ConfigurationManager.ConnectionStrings;
            this.cmbConnectionStrings.Clear();
            foreach (ConnectionStringSettings connectionString in connectionStrings)
            {
                this.cmbConnectionStrings.Add(connectionString.Name);
            }
            if (Settings.Connections.CurrentConnectionString != null)
            {
                this.cmbConnectionStrings.Select(Settings.Connections.CurrentConnectionString);
            }
            else
            {
                if (connectionStrings != null && connectionStrings.Count > 0)
                {
                    this.cmbConnectionStrings.Select(connectionStrings[0].Name);
                }
            }
        }
        #endregion

        #region Конструкторы/Деструкторы
        public PageSettings()
        {
            this.InitializeComponent();
        }


        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.UpdateConnectionsComboBox();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void cmbConnectionStrings_SelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.cmbConnectionStrings.SelectedItem != null)
            {
                Settings.Connections.CurrentConnectionString = this.cmbConnectionStrings.SelectedItem;
            }
        }
        #endregion

    }
}
