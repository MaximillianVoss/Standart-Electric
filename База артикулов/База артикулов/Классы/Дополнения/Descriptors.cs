namespace База_артикулов.Модели
{
    public partial class Descriptors
    {
        /// <summary>
        /// Конструктор для инициализации объекта Descriptors.
        /// </summary>
        /// <param name="code">Код дескриптора. По умолчанию null.</param>
        /// <param name="title">Название дескриптора. По умолчанию null.</param>
        /// <param name="titleShort">Краткое название дескриптора. По умолчанию null.</param>
        /// <param name="titleDisplay">Отображаемое название дескриптора. По умолчанию null.</param>
        /// <param name="description">Описание дескриптора. По умолчанию null.</param>
        public Descriptors(string code = null, string title = null, string titleShort = null, string titleDisplay = null, string description = null)
        {
            this.code = code;
            this.title = title;
            this.titleShort = titleShort;
            this.titleDisplay = titleDisplay;
            this.description = description;
        }



        public object ToObject()
        {
            return new
            {
                id = this.id
            };
        }
    }
}
