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
    
    public partial class THREAD
    {
        public THREAD()
        {
            this.POSTs = new HashSet<POST>();
        }
    
        public int ID { get; set; }
        public string SUBJECT { get; set; }
        public Nullable<System.DateTime> CREATED { get; set; }
        public Nullable<int> USER_ID { get; set; }
        public Nullable<int> STATUS { get; set; }
        public Nullable<int> CATEGORY_ID { get; set; }
    
        public virtual CATEGORY CATEGORY { get; set; }
        public virtual ICollection<POST> POSTs { get; set; }
        public virtual USER USER { get; set; }
    }
}
