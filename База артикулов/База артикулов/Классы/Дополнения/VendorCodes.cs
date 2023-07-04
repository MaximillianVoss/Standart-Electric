using System;
namespace База_артикулов.Модели
{
    public partial class VendorCodes
    {
        public VendorCodes(Descriptors descriptors)
        {
            this.Descriptors = descriptors ?? throw new ArgumentNullException(nameof(descriptors));
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
