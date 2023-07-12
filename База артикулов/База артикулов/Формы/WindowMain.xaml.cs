﻿using System.Windows;
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
        public MainWindow()
        {
            Settings.Connections.CurrentConnectionString = "Подключение к LAPTOP-BBFM8MMD";
            this.InitializeComponent();
            #region Set window size to 3/4 of the screen size
            this.Width = SystemParameters.PrimaryScreenWidth * 0.75;
            this.Height = SystemParameters.PrimaryScreenHeight * 0.75;
            #endregion
            this.fTables.Content = new PageTables();
            this.fImport.Content = new PageImport();
            this.fSettings.Content = new PageSettings();
        }
    }
}
