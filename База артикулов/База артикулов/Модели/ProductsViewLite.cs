//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace База_артикулов.Модели
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductsViewLite
    {
        public int ID_продукта { get; set; }
        public string Артикул { get; set; }
        public string Наименование_продукта { get; set; }
        public Nullable<double> Вес { get; set; }
        public Nullable<int> ID_типа_измерения { get; set; }
        public string Наименование_типа_единицы_измерения { get; set; }
        public Nullable<int> ID_единицы_измерения { get; set; }
        public string Наименование_единицы_измерения { get; set; }
        public Nullable<int> ID_подгруппы { get; set; }
        public string Наименование_подгруппы { get; set; }
        public Nullable<int> ID_группы { get; set; }
        public string Наименование_группы { get; set; }
        public Nullable<int> ID_класса { get; set; }
        public string Наименование_класса { get; set; }
    }
}
