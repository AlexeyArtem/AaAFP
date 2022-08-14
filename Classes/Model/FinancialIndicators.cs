using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    public struct FinancialIndicators
    {
        public FinancialIndicators(decimal revenue, decimal costs)
        {
            Revenue = revenue;
            Costs = costs;
        }

        public decimal Revenue { get; private set; }
        public decimal Costs { get; private set; }
        public decimal Profit { get => Revenue - Costs; }

        public void Add(decimal revenue, decimal costs) 
        {
            Revenue += revenue;
            Costs += costs;
        }
    }
}
