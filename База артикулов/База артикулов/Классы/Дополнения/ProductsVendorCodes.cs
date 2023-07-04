using System;
namespace База_артикулов.Модели
{
    public partial class ProductsVendorCodes
    {
        public ProductsVendorCodes(int id, int idProduct, int idCode, Products products, VendorCodes vendorCodes)
        {
            this.id = id;
            this.idProduct = idProduct;
            this.idCode = idCode;
            this.Products = products;
            this.VendorCodes = vendorCodes;
        }
        public ProductsVendorCodes(Products products, VendorCodes vendorCodes)
        {
            this.Products = products ?? throw new ArgumentNullException(nameof(products));
            this.VendorCodes = vendorCodes ?? throw new ArgumentNullException(nameof(vendorCodes));
        }
        public ProductsVendorCodes() : this(-1, -1, -1, null, null) { }
        public object ToObject()
        {
            return new
            {
                id = this.id
            };
        }
    }
}
