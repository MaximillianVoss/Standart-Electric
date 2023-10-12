using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace База_артикулов.Модели
{
    public partial class Manufacturers
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
