namespace База_артикулов.Модели
{
    public partial class SubGroups
    {
        public object ToObject()
        {
            return new
            {
                id = this.id,
                title = this.Descriptors.title,
                titleShort = this.Descriptors.titleShort,
                description = this.Descriptors.description,
                code = this.Descriptors.code
            };
        }
    }
}
