using System;

namespace База_артикулов.Модели
{
    public partial class Groups : IToObject
    {
        public Groups(Descriptors descriptor, Classes @class)
        {
            this.Descriptors = descriptor;
            this.idClass = @class.id;
            this.Classes = @class;
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
