using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Path = System.IO.Path;

namespace Менеджер_полей
{
    public partial class MainWindow : BaseWindow.BaseWindow
    {

        #region Поля

        #endregion

        #region Свойства
        List<Field> Fields { set; get; }
        Field SelectedField { set; get; }
        #endregion

        #region Методы
        void ParseJSON(string path)
        {
            this.Fields = JsonConvert.DeserializeObject<List<Field>>(File.ReadAllText(path));
        }
        void ParseXML(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            var tablesTags = doc.DocumentElement.SelectNodes("table");
            int idCounter = 0;
            foreach (XmlNode table in tablesTags)
            {
                var rowsTags = table.SelectNodes("row");
                foreach (XmlNode row in rowsTags)
                {
                    XmlNode comment = row.SelectSingleNode("comment");
                    XmlNode displayName = row.SelectSingleNode("displayName");
                    this.Fields.Add(new Field(
                        idCounter++,
                        row.Attributes["name"].Value,
                        row.Attributes["name"].Value,
                        displayName == null ? "" : displayName.InnerText,
                        comment == null ? "" : comment.InnerText,
                        table.Attributes["name"].Value
                        ));
                }
            }
        }
        void UpdateField(Field field)
        {
            this.txbTitle.CurrentText = field.Title;
            this.txbTitleOld.CurrentText = field.TitleOld;
            this.txbAlias.CurrentText = field.TitleAlias;
            this.txbTable.CurrentText = field.Table;
            this.txbDescription.CurrentText = field.Description;
        }
        Field SaveField()
        {
            Field field = new Field(
                this.SelectedField == null ? -1 : this.SelectedField.id,
                this.txbTitleOld.CurrentText,
                this.txbTitle.CurrentText,
                this.txbAlias.CurrentText,
                this.txbDescription.CurrentText,
                this.txbTable.CurrentText
                );
            return field;
        }
        void UpdateList(List<Field> fields)
        {
            int prevSelected = this.chlbFields.SelectedIndex;
            this.chlbFields.Items.Clear();
            if (fields != null)
            {
                List<String> listStr = new List<string>();
                foreach (var item in fields)
                {
                    listStr.Add(item.ToString());
                    this.chlbFields.Items.Insert(0, item);
                }
                if (prevSelected >= 0 && prevSelected < this.chlbFields.Items.Count)
                {
                    this.chlbFields.SelectedIndex = prevSelected;
                }
            }

        }
        void LoadFile(string path)
        {
            this.Fields.Clear();
            string extenstion = Path.GetExtension(path);
            if (extenstion == ".xml")
            {
                ParseXML(path);
            }
            else if (extenstion == ".json")
            {
                ParseJSON(path);
            }
            else
            {
                throw new Exception("Не поддерживаемое расширение файла!");
            }
            this.UpdateList(this.Fields);
        }
        void Save(List<Field> fields, String path)
        {
            var json = JsonConvert.SerializeObject(fields);
            File.WriteAllText(path, json);
        }
        void Save(Field field)
        {
            int index = this.Fields.FindIndex(x => x.id == field.id);
            if (index >= 0 && index < this.Fields.Count)
            {
                this.Fields[index] = field;
            }
        }
        XmlDocument Replace(string pathXML, List<Field> fields)
        {
            if (String.IsNullOrEmpty(pathXML))
            {
                throw new Exception("Некорректный путь до XML файла!");
            }

            if (fields == null)
            {
                throw new Exception("Пустой список полей!");
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(pathXML);
            var tablesTags = doc.DocumentElement.SelectNodes("table");
            foreach (XmlNode table in tablesTags)
            {
                var rowsTags = table.SelectNodes("row");
                foreach (XmlNode row in rowsTags)
                {
                    var newField = this.Fields.FirstOrDefault(x => x.TitleOld == row.Attributes["name"].Value);
                    if (newField != null)
                    {
                        row.Attributes["name"].Value = newField.Title;
                        XmlNode comment = row.SelectSingleNode("comment");
                        if (comment == null)
                        {
                            row.AppendChild(doc.CreateElement("comment"));
                            comment = row.SelectSingleNode("comment");
                        }
                        comment.InnerText = newField.Description;
                        XmlNode displayName = row.SelectSingleNode("displayName");
                        if (displayName == null)
                        {
                            row.AppendChild(doc.CreateElement("displayName"));
                            displayName = row.SelectSingleNode("displayName");
                        }
                        displayName.InnerText = newField.TitleAlias;
                    }
                }
            }
            return doc;
        }
        void SearchField()
        {
            if (String.IsNullOrEmpty(this.txbSearch.CurrentText))
            {
                this.UpdateList(Fields);
            }
            else
            {
                this.UpdateList(this.Fields.Where(x => x.ToString().ToLower().Contains(this.txbSearch.CurrentText.ToLower())).ToList());
            }
        }
        #endregion

        #region Конструкторы/Деструкторы
        public MainWindow()
        {
            InitializeComponent();
            this.Fields = new List<Field>();
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Save(this.Fields, this.GetSaveFilePath("JSON файл|*.json", 1, "json"));
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void загрзуитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadFile(this.GetLoadFilePath("XML файл|*.xml|JSON файл|*.json", 1, ".xml"));
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.txbTitleOld.Enabled = false;
            //this.txbTitleOld.Te
            //this.LoadFile("C:\\Users\\ivan.ivanov\\Documents\\SQL Server Management Studio\\Standart-Electric\\Standart-Electric\\DB_Standart-Electric.xml");
            //txbSearch.CurrentText = "Covers";
        }
        private void txbSearch_CurrentTextChanged(object sender, EventArgs e)
        {
            try
            {
                this.SearchField();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void txbTitle_CurrentTextChanged(object sender, EventArgs e)
        {
            try
            {
                //this.Save(this.SaveField());
                //this.SearchField();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void txbTitleOld_CurrentTextChanged(object sender, EventArgs e)
        {
            try
            {
                //this.Save(this.SaveField());
                //this.SearchField();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void txbAlias_CurrentTextChanged(object sender, EventArgs e)
        {
            try
            {
                //this.Save(this.SaveField());
                //this.SearchField();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void txbDescription_CurrentTextChanged(object sender, EventArgs e)
        {
            try
            {
                //this.Save(this.SaveField());
                //this.SearchField();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void txbTable_CurrentTextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Save(this.SaveField());
                //this.SearchField();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void chlbFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.chlbFields.SelectedItems.Count > 0 && this.chlbFields.SelectedIndex != -1)
            {
                this.SelectedField = (Field)this.chlbFields.SelectedItems[0];
                this.UpdateField(this.SelectedField);
            }
        }
        private void заменаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument doc = this.Replace(this.GetLoadFilePath("XML файл|*.xml", 1, ".xml", true, true, "Открыть файл для замены полей"), this.Fields);
                doc.Save(this.GetSaveFilePath("XML файл|*.xml", 1, ".xml", false, true, "Сохранение файла с замененными полями"));
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Save(this.SaveField());
                this.SearchField();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
        private void MainWindow_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.S && e.KeyCode == System.Windows.Forms.Keys.Control)
            {
                try
                {
                    this.Save(this.SaveField());
                    this.SearchField();
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
        }

        #endregion


    }
}
