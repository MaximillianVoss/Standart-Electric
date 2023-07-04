using System;

namespace База_артикулов.Модели
{
    public partial class UnitsProducts
    {
        public UnitsProducts(int idProduct, int? idUnit, int? idType, double value)
        {
            this.idProduct = idProduct;
            this.idUnit = idUnit;
            this.idType = idType;
            this.value = value;
        }
        public UnitsProducts()
        {

        }

        public object ToObject()
        {
            return new
            {
                id = this.id,
                title = this.UnitsTypes.title,
                idType = this.idType,
                titleShort = String.Empty,
                description = String.Empty,
                code = String.Empty,
                value = this.value
            };
        }
    }
}
