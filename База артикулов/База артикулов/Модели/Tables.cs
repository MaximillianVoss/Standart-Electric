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
    
    public partial class Tables
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tables()
        {
            this.ViewsTables = new HashSet<ViewsTables>();
        }
    
        public int id { get; set; }
        public Nullable<int> idDescriptor { get; set; }
    
        public virtual Descriptors Descriptors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ViewsTables> ViewsTables { get; set; }
    }
}
