﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlocakGoldData
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Blackgold_PRODEntities : DbContext
    {
        public Blackgold_PRODEntities()
            : base("name=Blackgold_PRODEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Well> Wells { get; set; }
        public virtual DbSet<WellDrillBitPeriod> WellDrillBitPeriods { get; set; }
        public virtual DbSet<WellDrillPeriod> WellDrillPeriods { get; set; }
        public virtual DbSet<WellDrillPeriodRemark> WellDrillPeriodRemarks { get; set; }
        public virtual DbSet<WellConnectTime> WellConnectTimes { get; set; }
        public virtual DbSet<WellTrippingSpeed> WellTrippingSpeeds { get; set; }
    }
}