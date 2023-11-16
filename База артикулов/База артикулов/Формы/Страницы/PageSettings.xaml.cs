using System;
using System.Collections.Generic;
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
            if (this.CustomBase.CustomDb.Settgins.CurrentConnectionString.Name != null)
            {
                this.cmbConnectionStrings.Select(this.CustomBase.CustomDb.Settgins.CurrentConnectionString.Name);
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
                this.CustomBase.CustomDb.Settgins.CurrentConnectionString.Name = this.cmbConnectionStrings.SelectedItem;
            }
        }

        public override void UpdateFields(List<CustomEventArgs> args)
        {
            //throw new NotImplementedException();
        }

        public override void UpdateForm(List<CustomEventArgs> args)
        {
            //throw new NotImplementedException();
        }

        public override object HandleOk(List<CustomEventArgs> args)
        {
            throw new NotImplementedException();
        }

        public override object HandleCancel(List<CustomEventArgs> args)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
