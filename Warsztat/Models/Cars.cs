//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Warsztat.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cars()
        {
            this.Repairs = new HashSet<Repairs>();
        }
    
        public long ID_car { get; set; }
        public Nullable<long> ID_user { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNo { get; set; }
        public Nullable<System.DateTime> ProductionDate { get; set; }
        public int Mileage { get; set; }
        public Nullable<bool> InService { get; set; }
    
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Repairs> Repairs { get; set; }
    }
}
