//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace flower
{
    using System;
    using System.Collections.Generic;
    
    public partial class Session
    {
        public int id { get; set; }
        public Nullable<int> id_user { get; set; }
        public Nullable<System.DateTime> date { get; set; }
    
        public virtual User User { get; set; }
    }
}