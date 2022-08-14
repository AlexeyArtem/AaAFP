using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    public class FinanceModel : BaseModel
    {
        private decimal initialWorkingCapital;

        public FinanceModel() : base() 
        {
            EnterpriseInfo info = dbEntities.EnterprisesInfo.FirstOrDefault();
            initialWorkingCapital = info == null ? 0 : info.WorkingCapital;
        }

        public decimal WorkingCapital 
        {
            get 
            {
                var value = initialWorkingCapital + Indicators.Profit;
                return value;
            } 
        }

        // Хранятся финансовые показатели за все время
        public FinancialIndicators Indicators
        {
            get 
            {
                var value = GetFinancialIndicators();
                return value;
            }
        }

        public FinancialIndicators GetFinancialIndicators() 
        {
            // Доходы: Другие доходы и платежи по заказам
            decimal resultRevenue = dbEntities.ContractPayments.Local.Sum(p => p.Sum)
                + dbEntities.OtherRevenues.Local.Sum(r => r.Sum);

            // Расходы: Постоянные платежи, покупки материалов, другие расходы, платежи по ЗП
            decimal resultCosts = dbEntities.OtherCosts.Local.Sum(c => c.Sum)
                + dbEntities.PaymentsFixedCosts.Local.Sum(c => c.fixed_costs.Sum)
                + dbEntities.SalaryPayments.Local.Sum(s => s.Sum)
                + dbEntities.RecordsPackingList.Local.Sum(s => s.QuantityUnits * s.UnitPrice)
                + dbEntities.PaymentsSalaryPrepay.Local.Sum(s => s.Sum)
                + dbEntities.BonusSalaries.Local.Sum(s => s.Sum)
                - dbEntities.DeductionsFromSalary.Local.Sum(d => d.Sum);

            return new FinancialIndicators(resultRevenue, resultCosts);
        }

        public FinancialIndicators GetFinancialIndicators(DateTime startDate, DateTime endDate) 
        {
            if (startDate.Date > endDate.Date)
                throw new ArgumentException("Конечная дата меньше начальной");

            // Корректировка дат
            DateTime currentEndDate = endDate;
            DateTime expectedStartDate = startDate;
            if (endDate > DateTime.Today && startDate < DateTime.Today) 
            {
                currentEndDate = DateTime.Today; // Начальная дата для расчета текущих показателей
                expectedStartDate = DateTime.Today; // Конечная дата для расчета ожидаемых показателей
            }

            // Доходы: Другие доходы и платежи по заказам
            decimal resultRevenue = dbEntities.ContractPayments.Local.Where(p => p.Date >= startDate && p.Date <= currentEndDate).Sum(p => p.Sum)
                + dbEntities.OtherRevenues.Local.Where(r => r.Date >= startDate && r.Date <= currentEndDate).Sum(r => r.Sum);

            // Расходы: Постоянные платежи, покупки материалов, другие расходы, платежи по ЗП
            decimal resultCosts = dbEntities.OtherCosts.Local.Where(c => c.Date >= startDate && c.Date <= currentEndDate).Sum(c => c.Sum)
                + dbEntities.PaymentsFixedCosts.Local.Where(c => c.Date >= startDate && c.Date <= currentEndDate).Sum(c => c.fixed_costs.Sum)
                + dbEntities.RecordsPackingList.Local.Where(r => r.packing_list.Date >= startDate && r.packing_list.Date <= currentEndDate).Sum(s => s.QuantityUnits * s.UnitPrice);

            decimal salaryCosts = dbEntities.SalaryPayments.Local.Where(s => s.Date >= startDate && s.Date <= currentEndDate).Sum(s => s.Sum)
                + dbEntities.PaymentsSalaryPrepay.Local.Where(s => s.Date >= startDate && s.Date <= currentEndDate).Sum(s => s.Sum)
                - dbEntities.BonusSalaries.Local.Where(b => b.Date >= startDate && b.Date <= currentEndDate).Sum(s => s.Sum)
                + dbEntities.DeductionsFromSalary.Local.Where(d => d.Date >= startDate && d.Date <= currentEndDate).Sum(s => s.Sum);

            resultCosts += salaryCosts;

            // Расчет ожидаемых показателей
            if (endDate.Date > DateTime.Today)
            {
                // Заказы, дата завершения которых входит в промежуток времени
                var contracts = dbEntities.Contracts.Local.Where(c => c.DateSigning.AddDays(c.ProductionDays) >= expectedStartDate && c.DateSigning.AddDays(c.ProductionDays) <= endDate);

                // Доходы: остаточные выплаты по заказам
                decimal revenue = contracts.Sum(c => c.Price) - contracts.Sum(c => c.contract_payments.Sum(cp => cp.Sum));

                // Расходы: остатки ЗП, постоянные расходы, материалы (для заказов, которые должны будут завершиться в указанный промежуток)                
                TimePeriod monthPeriod = new TimePeriod(TypeTimePeriod.Month, DateTime.Today);

                // Остаточные зтраты по ЗП
                decimal value = dbEntities.Employees.Local.Sum(s => s.SumSalary) - salaryCosts;
                decimal residualSalaryCosts = value < 0 ? 0 : value;

                // Сумма постоянных затрат
                decimal fixedCosts = (from w in dbEntities.WeeklyCosts.Local.Where(w => w.DayAccrual <= (int)endDate.DayOfWeek)
                                    from m in dbEntities.MonthlyCosts.Local.Where(m => m.DayAccrual <= endDate.Day)
                                    from p in dbEntities.PaymentsFixedCosts.Local.Where(p => p.Date >= expectedStartDate)
                                    from f in dbEntities.FixedCosts.Local
                                    where (w.IdFixedCost != p.IdFixedCost || m.IdFixedCost != p.IdFixedCost) &&
                                    (f.ID == w.IdFixedCost || f.ID == m.IdFixedCost)
                                    select f.Sum).Sum();

                resultRevenue += revenue;
                resultCosts += residualSalaryCosts + fixedCosts;
            }

            return new FinancialIndicators(resultRevenue, resultCosts);
        }
    }
}
