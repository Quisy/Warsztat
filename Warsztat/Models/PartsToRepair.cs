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
    
    public partial class PartsToRepair
    {
        public long ID_partsToRepair { get; set; }
        public Nullable<long> ID_repair { get; set; }
        public Nullable<long> ID_part { get; set; }
        public Nullable<int> quantity { get; set; }
    
        public virtual Parts Parts { get; set; }
        public virtual Repairs Repairs { get; set; }
    }
}
