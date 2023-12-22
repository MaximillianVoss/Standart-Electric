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
        private static string modelDefaultConnectionString = "DBSEEntities";
        #endregion

        #region Свойства

        #endregion

        #region Методы
        private void Save()
        {
            if (this.cmbConnectionStrings.SelectedItem != null)
            {
                this.CustomBase.CustomDb.Settgins.CurrentConnectionString.Name = this.cmbConnectionStrings.SelectedItem;
                this.CustomBase.CustomDb.Settgins.CurrentConnectionString.Value = ConfigurationManager.ConnectionStrings[this.cmbConnectionStrings.SelectedItem].ConnectionString;
                this.CustomBase.CustomDb.Settgins.SaveToFile(Settings.DEFAULT_FILE_PATH);
                this.CustomBase.CustomDb = new CustomDB(new Settings(Settings.DEFAULT_FILE_PATH));

                //ConfigurationManager.ConnectionStrings[modelDefaultConnectionString].ConnectionString = this.CustomBase.CustomDb.Settgins.CurrentConnectionString.Value;
                //this.CustomBase.CustomDb.InitDB(true);
            }
        }
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
        public override void UpdateFields(List<CustomEventArgs> args)
        {
            //throw new NotImplementedException();
        }
        public override void UpdateForm(List<CustomEventArgs> args)
        {
            this.InitializeComponent();
            this.UpdateConnectionsComboBox();
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

        #region Конструкторы/Деструкторы
        public PageSettings(CustomBase customBase) : base(customBase)
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

            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
        private void cmbConnectionStrings_SelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            Save();
        }
        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Save();
        }

        #endregion


    }
}
