namespace Менеджер_полей
{
    partial class MainWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.txbTitle = new Custom_Controls_WF.Controls.LabeledTextBox();
            this.txbAlias = new Custom_Controls_WF.Controls.LabeledTextBox();
            this.txbTable = new Custom_Controls_WF.Controls.LabeledTextBox();
            this.txbTitleOld = new Custom_Controls_WF.Controls.LabeledTextBox();
            this.gpFieldsList = new System.Windows.Forms.GroupBox();
            this.chlbFields = new System.Windows.Forms.CheckedListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрзуитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.полеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.правкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заменаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txbDescription = new Custom_Controls_WF.Controls.LabeledTextBox();
            this.txbSearch = new Custom_Controls_WF.Controls.LabeledTextBox();
            this.gpFieldsList.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbTitle
            // 
            this.txbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTitle.BackColor = System.Drawing.SystemColors.Control;
            this.txbTitle.BackgroundColor = System.Drawing.SystemColors.Control;
            this.txbTitle.CurrentText = "";
            this.txbTitle.Error = "";
            this.txbTitle.Location = new System.Drawing.Point(7, 124);
            this.txbTitle.Margin = new System.Windows.Forms.Padding(4);
            this.txbTitle.Name = "txbTitle";
            this.txbTitle.RegEx = null;
            this.txbTitle.Size = new System.Drawing.Size(386, 92);
            this.txbTitle.TabIndex = 0;
            this.txbTitle.Title = "Название";
            this.txbTitle.ValidationText = null;
            this.txbTitle.CurrentTextChanged += new System.EventHandler(this.txbTitle_CurrentTextChanged);
            // 
            // txbAlias
            // 
            this.txbAlias.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbAlias.BackColor = System.Drawing.SystemColors.Control;
            this.txbAlias.BackgroundColor = System.Drawing.SystemColors.Control;
            this.txbAlias.CurrentText = "";
            this.txbAlias.Error = "";
            this.txbAlias.Location = new System.Drawing.Point(7, 224);
            this.txbAlias.Margin = new System.Windows.Forms.Padding(4);
            this.txbAlias.Name = "txbAlias";
            this.txbAlias.RegEx = null;
            this.txbAlias.Size = new System.Drawing.Size(386, 92);
            this.txbAlias.TabIndex = 1;
            this.txbAlias.Title = "Отображаемое название";
            this.txbAlias.ValidationText = null;
            this.txbAlias.CurrentTextChanged += new System.EventHandler(this.txbAlias_CurrentTextChanged);
            // 
            // txbTable
            // 
            this.txbTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTable.BackColor = System.Drawing.SystemColors.Control;
            this.txbTable.BackgroundColor = System.Drawing.SystemColors.Control;
            this.txbTable.CurrentText = "";
            this.txbTable.Enabled = false;
            this.txbTable.Error = "";
            this.txbTable.Location = new System.Drawing.Point(7, 436);
            this.txbTable.Margin = new System.Windows.Forms.Padding(4);
            this.txbTable.Name = "txbTable";
            this.txbTable.RegEx = null;
            this.txbTable.Size = new System.Drawing.Size(386, 92);
            this.txbTable.TabIndex = 2;
            this.txbTable.Title = "Таблица";
            this.txbTable.ValidationText = null;
            this.txbTable.CurrentTextChanged += new System.EventHandler(this.txbTable_CurrentTextChanged);
            // 
            // txbTitleOld
            // 
            this.txbTitleOld.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbTitleOld.BackColor = System.Drawing.SystemColors.Control;
            this.txbTitleOld.BackgroundColor = System.Drawing.SystemColors.Control;
            this.txbTitleOld.CurrentText = "";
            this.txbTitleOld.Error = "";
            this.txbTitleOld.Location = new System.Drawing.Point(7, 25);
            this.txbTitleOld.Margin = new System.Windows.Forms.Padding(4);
            this.txbTitleOld.Name = "txbTitleOld";
            this.txbTitleOld.RegEx = null;
            this.txbTitleOld.Size = new System.Drawing.Size(386, 92);
            this.txbTitleOld.TabIndex = 3;
            this.txbTitleOld.Title = "Старое название";
            this.txbTitleOld.ValidationText = null;
            this.txbTitleOld.CurrentTextChanged += new System.EventHandler(this.txbTitleOld_CurrentTextChanged);
            // 
            // gpFieldsList
            // 
            this.gpFieldsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFieldsList.Controls.Add(this.chlbFields);
            this.gpFieldsList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gpFieldsList.Location = new System.Drawing.Point(418, 104);
            this.gpFieldsList.Name = "gpFieldsList";
            this.gpFieldsList.Size = new System.Drawing.Size(732, 458);
            this.gpFieldsList.TabIndex = 5;
            this.gpFieldsList.TabStop = false;
            this.gpFieldsList.Text = "Список полей";
            // 
            // chlbFields
            // 
            this.chlbFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chlbFields.FormattingEnabled = true;
            this.chlbFields.Location = new System.Drawing.Point(3, 21);
            this.chlbFields.Name = "chlbFields";
            this.chlbFields.Size = new System.Drawing.Size(726, 434);
            this.chlbFields.TabIndex = 0;
            this.chlbFields.SelectedIndexChanged += new System.EventHandler(this.chlbFields_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.полеToolStripMenuItem,
            this.правкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1155, 25);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьToolStripMenuItem,
            this.загрзуитьToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(50, 21);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // загрзуитьToolStripMenuItem
            // 
            this.загрзуитьToolStripMenuItem.Name = "загрзуитьToolStripMenuItem";
            this.загрзуитьToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.загрзуитьToolStripMenuItem.Text = "Загрзуить";
            this.загрзуитьToolStripMenuItem.Click += new System.EventHandler(this.загрзуитьToolStripMenuItem_Click);
            // 
            // полеToolStripMenuItem
            // 
            this.полеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьToolStripMenuItem1});
            this.полеToolStripMenuItem.Name = "полеToolStripMenuItem";
            this.полеToolStripMenuItem.Size = new System.Drawing.Size(51, 21);
            this.полеToolStripMenuItem.Text = "Поле";
            // 
            // сохранитьToolStripMenuItem1
            // 
            this.сохранитьToolStripMenuItem1.Name = "сохранитьToolStripMenuItem1";
            this.сохранитьToolStripMenuItem1.Size = new System.Drawing.Size(139, 22);
            this.сохранитьToolStripMenuItem1.Text = "Сохранить";
            this.сохранитьToolStripMenuItem1.Click += new System.EventHandler(this.сохранитьToolStripMenuItem1_Click);
            // 
            // правкаToolStripMenuItem
            // 
            this.правкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.заменаToolStripMenuItem});
            this.правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            this.правкаToolStripMenuItem.Size = new System.Drawing.Size(64, 21);
            this.правкаToolStripMenuItem.Text = "Правка";
            // 
            // заменаToolStripMenuItem
            // 
            this.заменаToolStripMenuItem.Name = "заменаToolStripMenuItem";
            this.заменаToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.заменаToolStripMenuItem.Text = "Замена";
            this.заменаToolStripMenuItem.Click += new System.EventHandler(this.заменаToolStripMenuItem_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.txbDescription);
            this.groupBox2.Controls.Add(this.txbTitleOld);
            this.groupBox2.Controls.Add(this.txbTitle);
            this.groupBox2.Controls.Add(this.txbAlias);
            this.groupBox2.Controls.Add(this.txbTable);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(12, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(400, 535);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Свойства поля";
            // 
            // txbDescription
            // 
            this.txbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbDescription.BackColor = System.Drawing.SystemColors.Control;
            this.txbDescription.BackgroundColor = System.Drawing.SystemColors.Control;
            this.txbDescription.CurrentText = "";
            this.txbDescription.Error = "";
            this.txbDescription.Location = new System.Drawing.Point(8, 325);
            this.txbDescription.Margin = new System.Windows.Forms.Padding(5);
            this.txbDescription.Name = "txbDescription";
            this.txbDescription.RegEx = null;
            this.txbDescription.Size = new System.Drawing.Size(385, 102);
            this.txbDescription.TabIndex = 4;
            this.txbDescription.Title = "Описание";
            this.txbDescription.ValidationText = null;
            this.txbDescription.CurrentTextChanged += new System.EventHandler(this.txbDescription_CurrentTextChanged);
            // 
            // txbSearch
            // 
            this.txbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbSearch.BackColor = System.Drawing.SystemColors.Control;
            this.txbSearch.BackgroundColor = System.Drawing.SystemColors.Control;
            this.txbSearch.CurrentText = "";
            this.txbSearch.Error = "";
            this.txbSearch.Location = new System.Drawing.Point(418, 28);
            this.txbSearch.Name = "txbSearch";
            this.txbSearch.RegEx = null;
            this.txbSearch.Size = new System.Drawing.Size(732, 70);
            this.txbSearch.TabIndex = 8;
            this.txbSearch.Title = "Поиск поля";
            this.txbSearch.ValidationText = null;
            this.txbSearch.CurrentTextChanged += new System.EventHandler(this.txbSearch_CurrentTextChanged);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 568);
            this.Controls.Add(this.txbSearch);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gpFieldsList);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "MainWindow";
            this.Text = "Менеджер полей";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            this.gpFieldsList.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Custom_Controls_WF.Controls.LabeledTextBox txbTitle;
        private Custom_Controls_WF.Controls.LabeledTextBox txbAlias;
        private Custom_Controls_WF.Controls.LabeledTextBox txbTable;
        private Custom_Controls_WF.Controls.LabeledTextBox txbTitleOld;
        private System.Windows.Forms.GroupBox gpFieldsList;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрзуитьToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox chlbFields;
        private System.Windows.Forms.ToolStripMenuItem правкаToolStripMenuItem;
        private Custom_Controls_WF.Controls.LabeledTextBox txbDescription;
        private Custom_Controls_WF.Controls.LabeledTextBox txbSearch;
        private System.Windows.Forms.ToolStripMenuItem заменаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem полеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem1;
    }
}

