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
    
    public partial class TablesView
    {
        public int ID { get; set; }
        public int ID_дескриптора { get; set; }
        public string Наименование { get; set; }
        public string Сокращенное_наименование { get; set; }
        public string Отображаемое_название { get; set; }
        public string Код { get; set; }
        public string Описание { get; set; }
        public Nullable<System.DateTime> Дата_создания { get; set; }
    }
}
