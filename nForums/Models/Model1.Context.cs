﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NFORUMSEntities : DbContext
    {
        public NFORUMSEntities()
            : base("name=NFORUMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CATEGORY> CATEGORies { get; set; }
        public virtual DbSet<POST> POSTs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<THREAD> THREADs { get; set; }
        public virtual DbSet<USER> USERs { get; set; }
        public virtual DbSet<COMMENT> COMMENTs { get; set; }
        public virtual DbSet<REPLY_COMMENT> REPLY_COMMENT { get; set; }
        public virtual DbSet<LIKE_POST> LIKE_POST { get; set; }
        public virtual DbSet<REPORT_POST> REPORT_POST { get; set; }
    }
}
