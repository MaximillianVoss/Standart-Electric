namespace База_артикулов.Модели
{
    public partial class UnitsTypes
    {
        public object ToObject()
        {
            return new
            {
                id = this.id,
                title = this.title
            };
        }
    }
}
