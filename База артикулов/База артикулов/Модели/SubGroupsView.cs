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
    
    public partial class SubGroupsView
    {
        public int ID_подгруппы { get; set; }
        public string Наименование_подгруппы { get; set; }
        public string Сокращенное_наименование_подгруппы { get; set; }
        public string Описание_подгруппы { get; set; }
        public string Код_подгруппы { get; set; }
        public string URL_изображения_подгруппы { get; set; }
        public Nullable<int> ID_группы { get; set; }
        public Nullable<int> ID_схемы_нагрузок { get; set; }
        public Nullable<int> ID_дескриптора_схемы_нагрузок { get; set; }
        public string Наименование_схемы_нагрузок { get; set; }
        public string Сокращенное_наименование_схемы_нагрузок { get; set; }
        public string Отображаемое_название_схемы_нагрузок { get; set; }
        public string Код_схемы_нагрузок { get; set; }
        public string Описание_схемы_нагрузок { get; set; }
    }
}
