namespace База_артикулов.Модели
{
    public partial class ProductsViewLite
    {
        public ProductsViewLite() { }
        public ProductsViewLite(GetFilteredProductsLite_Result product)
        {
            this.ID_продукта = product.ID_продукта;
            this.Артикул = product.Артикул;
            this.Наименование_продукта = product.Наименование_продукта;
            this.ID_подгруппы = product.ID_подгруппы;
            this.Наименование_подгруппы = product.Наименование_подгруппы;
            this.ID_группы = product.ID_группы;
            this.Наименование_группы = product.Наименование_группы;
            this.ID_класса = product.ID_класса;
            this.Наименование_класса = product.Наименование_класса;
            //Наименование_единицы_измерения = product.Наименование_единицы_измерения;
            this.Вес = product.Вес;

        }
    }
}
