using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    class RecordMaterial
    {
        public RecordMaterial(Material material, int quantityUnits, decimal unitPrice)
        {
            Material = material;
            QuantityUnits = quantityUnits;
            UnitPrice = unitPrice;
        }

        public RecordMaterial(Material material, int quantityUnits)
        {
            Material = material;
            QuantityUnits = quantityUnits;
            UnitPrice = 0;
        }

        public Material Material { get; }
        public int QuantityUnits { get; }
        public decimal UnitPrice { get; }
    }
}
