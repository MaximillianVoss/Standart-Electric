namespace База_артикулов.Модели
{
    public partial class Descriptors
    {
        public Descriptors(string code, string title, string titleShort, string description)
        {
            this.code = code;
            this.title = title;
            this.titleShort = titleShort;
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
