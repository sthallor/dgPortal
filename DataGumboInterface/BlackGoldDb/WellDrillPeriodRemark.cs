//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlackGoldData
{
    using System;
    using System.Collections.Generic;
    
    public partial class WellDrillPeriodRemark
    {
        public int WellDrillPeriodRemarkID { get; set; }
        public Nullable<bool> ActiveInd { get; set; }
        public string Source { get; set; }
        public Nullable<int> Tour_WellDrillPeriodID { get; set; }
        public Nullable<int> RemarkSeqNo { get; set; }
        public Nullable<int> RemarkType_rRemarkTypeID { get; set; }
        public Nullable<System.DateTimeOffset> EffectiveDate { get; set; }
        public Nullable<System.DateTimeOffset> ExpiryDate { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> RowChangedDate { get; set; }
        public Nullable<System.DateTime> RowCreatedDate { get; set; }
        public string ActivityCodeDescriptionOverride { get; set; }
    
        public virtual WellDrillPeriod WellDrillPeriod { get; set; }
    }
}
