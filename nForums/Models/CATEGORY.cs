//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace nForums.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CATEGORY
    {
        public CATEGORY()
        {
            this.THREADs = new HashSet<THREAD>();
        }
    
        public int ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<System.DateTime> CREATED { get; set; }
    
        public virtual ICollection<THREAD> THREADs { get; set; }
    }
}
