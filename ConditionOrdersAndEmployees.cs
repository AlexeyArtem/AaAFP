//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AaAFP2
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConditionOrdersAndEmployees
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConditionOrdersAndEmployees()
        {
            this.recommendations = new HashSet<Recommendation>();
        }
    
        public int ID { get; set; }
        public Nullable<sbyte> HasEmployeesManyPosition { get; set; }
        public Nullable<sbyte> HasCompletedOrdersUnpaid { get; set; }
        public Nullable<sbyte> HasIncompleteOrdersProductionTimeViolations { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recommendation> recommendations { get; set; }
    }
}
