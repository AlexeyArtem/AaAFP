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
    
    public partial class Recommendation
    {
        public int ID { get; set; }
        public int IdConditionManufacturing { get; set; }
        public int IdConditionFinance { get; set; }
        public int IdConditionOrdersAndEmployees { get; set; }
        public string StateDescription { get; set; }
        public string RecommendationText { get; set; }
    
        public virtual ConditionFinance conditions_finace { get; set; }
        public virtual ConditionManufacturing conditions_manufacturing { get; set; }
        public virtual ConditionOrdersAndEmployees conditions_orders_and_employees { get; set; }
    }
}
