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
    
    public partial class PerforationsView
    {
        public int ID_перфорации { get; set; }
        public string Наименование_перфорации { get; set; }
        public string Сокращенное_наименование_перфорации { get; set; }
        public string Описание_перфорации { get; set; }
        public Nullable<double> Шаг { get; set; }
        public Nullable<double> Ширина { get; set; }
        public Nullable<double> Длина { get; set; }
        public Nullable<double> Шаг_перфорации__мм { get; set; }
        public Nullable<double> Ширина_перфорации__мм { get; set; }
        public Nullable<double> Длина_перфорации__мм { get; set; }
    }
}
