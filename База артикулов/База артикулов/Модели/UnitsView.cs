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
    
    public partial class UnitsView
    {
        public int ID_единицы_измерения { get; set; }
        public string Наименование_единицы_измерения { get; set; }
        public string Сокращенное_наименование_единицы_измерения { get; set; }
        public string Описание_единицы_измерения { get; set; }
        public Nullable<int> Общероссийский_классификатор_единиц_измерения { get; set; }
    }
}
