using System;

namespace База_артикулов.Модели
{
    public partial class Perforations
    {
        public object ToObject()
        {
            return new
            {
                id = this.id,
                title = String.Format("{0} {1}", this.Descriptors.code, this.Descriptors.title),
                titleShort = this.Descriptors.titleShort,
                description = this.Descriptors.description,
                code = this.Descriptors.code
            };
        }
    }
}
