using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace База_артикулов.Модели
{
    public partial class ProductsViewLite
    {
        public ProductsViewLite() { }
        public ProductsViewLite(GetFilteredProductsLite_Result product)
        {
            ID_продукта = product.ID_продукта;
            Артикул = product.Артикул;
            Наименование_продукта = product.Наименование_продукта;
            ID_подгруппы = product.ID_подгруппы;
            Наименование_подгруппы = product.Наименование_подгруппы;
            ID_группы = product.ID_группы;
            Наименование_группы = product.Наименование_группы;
            ID_класса = product.ID_класса;
            Наименование_класса = product.Наименование_класса;
            Вес = product.Вес;
        }
    }
}
