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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Sessions = new HashSet<Session>();
            this.orders = new HashSet<order>();
        }
    
        public int id { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string patrynumic { get; set; }
        public string passport { get; set; }
        public Nullable<int> id_division { get; set; }
        public Nullable<int> id_role { get; set; }
        public string image { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    
        public virtual division division { get; set; }
        public virtual role role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Session> Sessions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order> orders { get; set; }
    }
}
