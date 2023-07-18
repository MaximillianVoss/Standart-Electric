using System;

namespace База_артикулов.Модели
{
    public partial class Applications : IToObject
    {
        public Applications(Descriptors descriptor)
        {
            this.Descriptors = descriptor;
        }
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
