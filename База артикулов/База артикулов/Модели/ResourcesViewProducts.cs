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
    
    public partial class ResourcesViewProducts
    {
        public int ID_ресурса { get; set; }
        public string URL_ресурса { get; set; }
        public string Наименование_объекта { get; set; }
        public Nullable<int> ID_дескриптора_объекта { get; set; }
        public Nullable<int> ID_дескриптора_ресурса { get; set; }
        public Nullable<int> ID_типа_ресурса { get; set; }
        public string Наименование_типа_ресурса { get; set; }
        public string Расширение_ресурса { get; set; }
        public int ID_продукта { get; set; }
        public int ID_дескриптора_продукта { get; set; }
        public Nullable<int> ID_стандарта_продукта { get; set; }
        public Nullable<int> ID_подгруппы_продукта { get; set; }
        public Nullable<int> ID_покрытия_продукта { get; set; }
        public Nullable<int> ID_материала_продукта { get; set; }
        public Nullable<int> ID_перфорации_продукта { get; set; }
        public Nullable<int> ID_упаковки_продукта { get; set; }
        public bool На_складе_продукт { get; set; }
    }
}
