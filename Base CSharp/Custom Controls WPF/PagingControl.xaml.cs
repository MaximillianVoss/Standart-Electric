using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CustomControlsWPF
{
    /// <summary>
    /// Определение класса для передачи данных событию
    /// </summary>
    public class PageSizeChangedEventArgs : EventArgs
    {
        public int OldPageSize { get; }
        public int NewPageSize { get; }

        public PageSizeChangedEventArgs(int oldPageSize, int newPageSize)
        {
            this.OldPageSize = oldPageSize;
            this.NewPageSize = newPageSize;
        }
    }

    /// <summary>
    /// 4. Определение класса для передачи данных событию
    /// </summary>
    public class PageChangedEventArgs : EventArgs
    {
        public int OldPage { get; }
        public int NewPage { get; }

        public PageChangedEventArgs(int oldPage, int newPage)
        {
            this.OldPage = oldPage;
            this.NewPage = newPage;
        }
    }
    /// <summary>
    /// Логика взаимодействия для PagingControl.xaml
    /// </summary>
    public partial class PagingControl : UserControl
    {


        #region Поля
        // 1. Определение делегата для события
        public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);
        // 2. Определение события
        public event PageChangedEventHandler PageChanged;
        // Определение делегата и события для изменения размера страницы
        public delegate void PageSizeChangedEventHandler(object sender, PageSizeChangedEventArgs e);
        public event PageSizeChangedEventHandler PageSizeChanged;
        private int pagesCount;
        private int currentPage;
        private int pageSize;
        private List<int> pageSizes = new List<int>();
        #endregion

        #region Свойства
        public int PagesCount
        {
            get => this.pagesCount;
            set
            {
                if (value < 0)
                    throw new Exception("Число страниц не может быть меньше нуля");
                this.pagesCount = value;
                this.lblCurrentPage.Content = $"{this.currentPage}/{this.PagesCount}";
            }
        }

        public int CurrentPage
        {
            get => this.currentPage;
            set
            {
                if (value < 0)
                    throw new Exception("Номер текущей страницы не может быть меньше нуля");
                if (value > this.pagesCount)
                    throw new Exception("Номер текущей страницы не может больше числа страниц");

                var oldValue = this.currentPage;
                this.currentPage = value;

                this.lblCurrentPage.Content = $"{this.currentPage}/{this.PagesCount}";

                // 3. Вызов события
                PageChanged?.Invoke(this, new PageChangedEventArgs(oldValue, value));
            }
        }

        public int PageSize
        {
            get => this.pageSize;
            set
            {
                if (value < 0)
                    throw new Exception("Число элементов на странице не может быть меньше нуля");

                var oldValue = this.pageSize;
                this.pageSize = value;

                // Вызов события при изменении размера страницы
                PageSizeChanged?.Invoke(this, new PageSizeChangedEventArgs(oldValue, value));
            }
        }

        public List<int> PageSizes
        {
            set
            {
                this.pageSizes = value;
                if (this.pageSizes != null)
                {
                    foreach (var item in this.pageSizes)
                        this.cmbItemsPerPage.Add(item);
                }
            }
            get => this.pageSizes;
        }
        #endregion

        #region Методы

        #endregion

        #region Конструкторы/Деструкторы
        public PagingControl()
        {
            this.InitializeComponent();
            this.PageSizes = new List<int>() { 10, 20, 50, 100, 1000 };
            this.cmbItemsPerPage.SelectedIndex = 0;
            this.currentPage = 1;
            this.PagesCount = 10;
        }

        private void UpdateButtons()
        {
            this.btnBack.IsEnabled = this.CurrentPage > 1;
            this.btnNext.IsEnabled = this.CurrentPage < this.PagesCount;
        }


        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void btnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.CurrentPage > 1)
                this.CurrentPage = this.CurrentPage - 1;
            this.UpdateButtons();
        }

        private void btnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.CurrentPage < this.PagesCount)
                this.CurrentPage = this.CurrentPage + 1;
            this.UpdateButtons();
        }

        private void cmbItemsPerPage_SelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.cmbItemsPerPage.SelectedIndex != -1)
                this.PageSize = this.PageSizes[this.cmbItemsPerPage.SelectedIndex];
            else
                this.PageSize = 0;
        }

        #endregion

    }
}
