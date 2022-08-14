using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    class ProductModel : BaseModel
    {
        public ProductModel() { }

        public TimeSpan GetProductionTime(Product product)
        {
            var operations = dbEntities.OperationsProducts.Local.Where(op => op.IdProduct == product.ID);
            int quantityProducts = product.Quantity ?? 1;

            TimeSpan prodTime = new TimeSpan();
            foreach (var o in operations)
            {
                for (int i = 0; i < o.Quantity; i++)
                {
                    var time = TimeSpan.FromTicks(o.manufacturing_operations.ExecutionTime.Value.Ticks * quantityProducts);
                    prodTime.Add(time);
                }
            }

            return prodTime;
        }

        public decimal GetPrimeCost(Product product)
        {
            int plannedQuantity = dbEntities.TypesProducts.Local.Sum(t => t.PlannedQuantity);
            int quantityProducts = product.Quantity ?? 1;

            decimal sumSalaries = dbEntities.Employees.Local.Sum(s => s.SumSalary);
            decimal sumFixedCosts = dbEntities.FixedCosts.Local.Sum(f => f.Sum);
            decimal sumAmortization = dbEntities.Equipments.Local.Where(e => e.AmortizationSumInMonth != null).Sum(e => e.AmortizationSumInMonth) ?? 0;
            decimal costMaterials = dbEntities.ProductsMaterials.Local.Where(pm => pm.IdProduct == product.ID).Sum(pm => pm.QuantityMaterial * pm.UnitPrice);

            decimal primeCost = ((sumSalaries + sumFixedCosts + sumAmortization) / plannedQuantity) + costMaterials * quantityProducts;
            return primeCost;
        }

        public decimal GetPrimeCostOrder(Order order) 
        {
            if (order == null) return 0;

            var products = dbEntities.Products.Local.Where(p => p.IdOrder == order.ID);
            decimal? sum = products?.Sum(p => GetPrimeCost(p));
            
            return sum ?? 0;
        }

        public decimal GetPrimeCostOrder(int idOrder)
        {
            var products = dbEntities.Products.Local.Where(p => p.IdOrder == idOrder);
            decimal? sum = products?.Sum(p => GetPrimeCost(p));
            return sum ?? 0;
        }

        public void AddProduct(Product product, List<ProductMaterial> materials, List<OperationProduct> operations)
        {
            if (dbEntities.Set<Product>().Local.Contains(product))
                return;

            dbEntities.AddEntity(product);
            dbEntities.SaveChanges();

            foreach (var material in materials)
            {
                material.IdProduct = product.ID;
                dbEntities.AddEntity(material);
            }

            foreach (var operation in operations)
            {

                operation.IdProduct = product.ID;
                dbEntities.AddEntity(operation);
            }
        }
    }
}
