using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    class RecommendationModel : BaseModel
    {
        private SalaryModel salaryModel;
        private MaterialModel materialModel;

        public RecommendationModel(SalaryModel salaryModel, FinanceModel financeModel, MaterialModel materialModel) 
        {
            this.materialModel = materialModel;
            this.salaryModel = salaryModel;
            this.FinanceModel = financeModel;
        }

        public FinanceModel FinanceModel { get; }

        private ConditionOrdersAndEmployees GetConditionOrdersAndEmployees()
        {
            bool hasEmployeesManyPosition = false;
            if (dbEntities.Employees.Local.Where(e => e.employees_positions.Count > 1).Count() > 0) hasEmployeesManyPosition = true;

            bool hasCompletedOrdersUnpaid = false;
            foreach (var order in dbEntities.Orders.Local.Where(o => o.IdStatusOrder == (int)TypeStatusOrder.Completed))
            {
                var contract = order.contracts.FirstOrDefault();
                if (contract != null)
                {
                    decimal payments = contract.contract_payments.Sum(p => p.Sum);
                    if (payments < contract.Price)
                    {
                        hasCompletedOrdersUnpaid = true;
                        break;
                    }
                }
            }

            bool hasIncompleteOrdersProductionTimeViolations = false;
            foreach (var contract in dbEntities.Contracts.Local.Where(c => c.order.IdStatusOrder != (int)TypeStatusOrder.Completed))
            {
                DateTime endDate = contract.DateSigning.AddDays(contract.ProductionDays);
                if (endDate < DateTime.Today)
                {
                    hasIncompleteOrdersProductionTimeViolations = true;
                    break;
                }
            }

            var condition = new ConditionOrdersAndEmployees()
            {
                HasCompletedOrdersUnpaid = Convert.ToSByte(hasCompletedOrdersUnpaid),
                HasEmployeesManyPosition = Convert.ToSByte(hasEmployeesManyPosition),
                HasIncompleteOrdersProductionTimeViolations = Convert.ToSByte(hasIncompleteOrdersProductionTimeViolations),
            };

            return condition;
        }

        private ConditionManufacturing GetConditionManufacturing() 
        {
            bool productsProductionExceedsPredicted = false;
            TimePeriod currentMonth = new TimePeriod(TypeTimePeriod.Month, DateTime.Today);
            
            int productionProducts = dbEntities.Contracts.Local.Where(c => c.order.IdStatusOrder == (int)TypeStatusOrder.Production && c.DateSigning.AddDays(c.ProductionDays) <= currentMonth.End).Sum(c => c.order.products.Count);
            int completedProducts = dbEntities.Contracts.Local.Where(c => c.order.IdStatusOrder != (int)TypeStatusOrder.Production && c.DateSigning >= currentMonth.Start).Sum(c => c.order.products.Count);
            int currentProducts = productionProducts + completedProducts;

            int predictedProducts = dbEntities.TypesProducts.Local.Sum(t => t.PlannedQuantity);
            if (currentProducts > predictedProducts) productsProductionExceedsPredicted = true;

            var condition = new ConditionManufacturing()
            {
                ProductsProductionExceedsPredicted = Convert.ToSByte(productsProductionExceedsPredicted)
            };

            return condition;
        }

        private ConditionFinance GetConditionFinance() 
        {
            TimePeriod currentMonth = new TimePeriod(TypeTimePeriod.Month, DateTime.Today);
            bool isPredictedProfitNegative = FinanceModel.GetFinancialIndicators(currentMonth.Start, currentMonth.End).Profit < 0;

            bool isWorkingCapitalNotEnough = false;
            decimal salaryCosts = salaryModel.AccruedSalaries.Sum(a => a.Value);
            decimal materialsCosts = materialModel.ShortageMaterials.Sum(s => s.UnitPrice * s.QuantityUnits);
            decimal fixedCosts = dbEntities.FixedCosts.Local.Sum(f => f.Sum);
            if (FinanceModel.WorkingCapital < salaryCosts + materialsCosts + fixedCosts) isWorkingCapitalNotEnough = true;

            bool isWorkingCapitalEnoughSalary = false;
            if (FinanceModel.WorkingCapital > salaryCosts)
                isWorkingCapitalEnoughSalary = true;

            bool isWokingCapitalEnoughMaterial = false;
            if (FinanceModel.WorkingCapital > materialsCosts)
                isWokingCapitalEnoughMaterial = true;

            bool isWorkingCapitalEnoughFixedCosts = false;
            if (FinanceModel.WorkingCapital > fixedCosts)
                isWorkingCapitalEnoughFixedCosts = true;

            var condition = new ConditionFinance()
            {
                IsPredictedProfitNegative = Convert.ToSByte(isPredictedProfitNegative),
                IsWorkingCapitalNotEnough = Convert.ToSByte(isWorkingCapitalNotEnough),
                IsWorkingCapitalEnoughSalary = Convert.ToSByte(isWorkingCapitalEnoughSalary),
                IsWokingCapitalEnoughMaterial = Convert.ToSByte(isWokingCapitalEnoughMaterial),
                IsWorkingCapitalEnoughFixedCosts = Convert.ToSByte(isWorkingCapitalEnoughFixedCosts)
            };

            return condition;
        }

        public Recommendation FindRecommendation()
        {
            // Условия заказов и сотрудников
            ConditionOrdersAndEmployees conditionOrdersAndEmployees = GetConditionOrdersAndEmployees();

            // Производственные условия
            ConditionManufacturing conditionManufacturing = GetConditionManufacturing();

            // Финансовые условия
            ConditionFinance conditionFinance = GetConditionFinance();

            if (conditionOrdersAndEmployees == null || conditionManufacturing == null || conditionFinance == null)
            {
                return new Recommendation() { RecommendationText = "Рекомендаций не найдено", StateDescription = "Рекомендаций не найдено" };
            }

            Recommendation recommendation = null;
            foreach (var rec in dbEntities.Recommendations.Local)
            {
                if (rec.conditions_finace.IsPredictedProfitNegative == conditionFinance.IsPredictedProfitNegative &&
                    rec.conditions_finace.IsWokingCapitalEnoughMaterial == conditionFinance.IsWokingCapitalEnoughMaterial &&
                    rec.conditions_finace.IsWorkingCapitalEnoughFixedCosts == conditionFinance.IsWorkingCapitalEnoughFixedCosts &&
                    rec.conditions_finace.IsWorkingCapitalEnoughSalary == conditionFinance.IsWorkingCapitalEnoughSalary &&
                    rec.conditions_finace.IsWorkingCapitalNotEnough == conditionFinance.IsWorkingCapitalNotEnough &&
                    rec.conditions_manufacturing.ProductsProductionExceedsPredicted == conditionManufacturing.ProductsProductionExceedsPredicted &&
                    rec.conditions_orders_and_employees.HasCompletedOrdersUnpaid == conditionOrdersAndEmployees.HasCompletedOrdersUnpaid &&
                    rec.conditions_orders_and_employees.HasEmployeesManyPosition == conditionOrdersAndEmployees.HasEmployeesManyPosition &&
                    rec.conditions_orders_and_employees.HasIncompleteOrdersProductionTimeViolations == conditionOrdersAndEmployees.HasIncompleteOrdersProductionTimeViolations

                    )
                {
                    recommendation = rec;
                }
            }

            if (recommendation == null)
                recommendation = new Recommendation() { RecommendationText = "Рекомендаций не найдено", StateDescription = "Рекомендаций не найдено" };

            return recommendation;

        }
    }
}
