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
    
    public partial class GroupsApplicationsView
    {
        public int ID_применения_подгруппы { get; set; }
        public Nullable<int> ID_применения { get; set; }
        public string Наименование_применения { get; set; }
        public Nullable<int> ID_направления { get; set; }
        public string Наименование_направления { get; set; }
        public Nullable<int> ID_Файла_BIM_библиотеки { get; set; }
        public Nullable<int> ID_Файла_блоков_чертежей { get; set; }
        public Nullable<int> ID_Типового_альбома { get; set; }
        public Nullable<int> ID_подгруппы { get; set; }
        public string Наименование_подгруппы { get; set; }
        public string Сокращенное_наименование_подгруппы { get; set; }
        public string Код_подгруппы { get; set; }
        public string URL_изображения_подгруппы { get; set; }
        public Nullable<int> ID_группы { get; set; }
        public Nullable<int> ID_схемы_нагрузок { get; set; }
        public Nullable<int> ID_дескриптора { get; set; }
        public string Наименование_схемы_нагрузок { get; set; }
        public string Сокращенное_наименование_схемы_нагрузок { get; set; }
        public string Отображаемое_название_схемы_нагрузок { get; set; }
        public string Код_схемы_нагрузок { get; set; }
        public string Описание_схемы_нагрузок { get; set; }
    }
}
