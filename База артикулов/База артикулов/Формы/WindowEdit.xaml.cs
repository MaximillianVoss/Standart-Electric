using System;
using System.Windows;
using База_артикулов.Классы;

namespace База_артикулов.Формы
{
    /// <summary>
    /// Логика взаимодействия для WindowEdit.xaml
    /// </summary>
    public partial class WindowEdit : Window
    {
        public Type itemType { set; get; }
        public object currentItem { set; get; }
        public WindowEdit()
        {
            this.InitializeComponent();
            this.Title = Common.Strings.Titles.Windows.noAction;
            this.itemType = null;
        }
        public WindowEdit(Type itemType)
        {
            this.itemType = itemType;
        }
        public WindowEdit(Type itemType, object item)
        {
            this.itemType = itemType;
        }

        public WindowEdit(string title, Type itemType, object item)
        {
            this.InitializeComponent();
            this.itemType = itemType;
            this.Title = title;
            this.currentItem = item;
        }
    }
}
