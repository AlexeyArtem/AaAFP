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
    
    public partial class UnitMeasure
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnitMeasure()
        {
            this.types_materials = new HashSet<TypeMaterial>();
        }
    
        public int ID { get; set; }
        public string ShortTitle { get; set; }
        public string FullTitle { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TypeMaterial> types_materials { get; set; }
    }
}
