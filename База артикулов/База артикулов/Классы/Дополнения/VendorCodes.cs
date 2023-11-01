using System;
namespace База_артикулов.Модели
{
    public partial class VendorCodes
    {
        public VendorCodes(string code, string accountantCode, int manufacturerId, bool isActual, bool isPublic, bool isSale) : this()
        {
            this.isActual = isActual;
            this.isPublic = isPublic; // Если значение isPublic NULL, присваиваем false
            this.isSale = isSale;
            this.codeAccountant = accountantCode;
            this.idManufacturer = manufacturerId;

            // Создание дескриптора
            this.Descriptors = new Descriptors() { title = code };
        }
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
