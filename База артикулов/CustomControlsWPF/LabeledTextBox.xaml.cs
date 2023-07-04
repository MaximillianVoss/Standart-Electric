using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControlsWPF
{
    /// <summary>
    /// Логика взаимодействия для LabeledTextBox.xaml
    /// </summary>
    public partial class LabeledTextBox : UserControl
    {

        #region Поля

        #endregion

        #region Свойства
        public string Title
        {
            set { this.lblTitle.Content = value; }
            get { return this.lblTitle.Content.ToString(); }
        }
        public string Text
        {
            set { this.txbValue.Text = value; }
            get { return this.txbValue.Text; }
        }
        public string Error
        {
            set
            {
                if (value == null || value == String.Empty)
                {
                    this.lblError.Content = String.Empty;
                }
                else
                {
                    this.lblError.Content = value;
                }
            }
            get
            {
                return this.lblError.Content.ToString();
            }
        }
        public Brush BackgroundColor
        {
            set { this.gMain.Background = value; }
            get { return this.gMain.Background; }
        }
        #endregion

        #region Методы

        #endregion

        #region Конструкторы/Деструкторы
        public LabeledTextBox() : this("Заголовок", "Значение")
        {
            this.InitializeComponent();
        }
        public LabeledTextBox(string title, string text, string error = null, Brush backgroundColor = null)
        {
            this.InitializeComponent();
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
            this.BackgroundColor = backgroundColor;
            this.Error = error;
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion

    }
}
