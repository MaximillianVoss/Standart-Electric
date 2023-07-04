using System;

namespace Менеджер_полей
{
    internal class Field
    {
        public Field() : this(-1, "", "", "", "", "")
        {

        }
        public Field(int id, string titleOld, string title, string titleAlias, string description, string table)
        {
            this.id = id;
            this.TitleOld = titleOld ?? throw new ArgumentNullException(nameof(titleOld));
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
            this.TitleAlias = titleAlias ?? throw new ArgumentNullException(nameof(titleAlias));
            this.Description = description ?? throw new ArgumentNullException(nameof(description));
            this.Table = table ?? throw new ArgumentNullException(nameof(table));
        }

        #region Поля

        #endregion

        #region Свойства
        /// <summary>
        /// ID внутри коллекции полей
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// Старое название в БД
        /// </summary>
        public string TitleOld { set; get; }
        /// <summary>
        /// Текущее название
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string TitleAlias { set; get; }
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// Таблица, где поле находится
        /// </summary>
        public string Table { set; get; }
        #endregion

        #region Методы
        public override string ToString()
        {
            return String.Format("Поле:'{1}({2})' Отображаемое имя:'{0}' Описание:'{3}' Таблица:'{4}'",
                String.IsNullOrEmpty(this.TitleAlias) ? "Не указано" : this.TitleAlias,
                String.IsNullOrEmpty(this.Title) ? "Не указано" : this.Title,
                String.IsNullOrEmpty(this.TitleOld) ? "Не указано" : this.TitleOld,
                String.IsNullOrEmpty(this.Description) ? "Не указано" : this.Description,
                String.IsNullOrEmpty(this.Table) ? "Не указано" : this.Table
                );
        }
        #endregion

        #region Конструкторы/Деструкторы

        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion


    }
}
