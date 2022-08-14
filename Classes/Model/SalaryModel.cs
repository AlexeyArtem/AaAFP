using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    class SalaryModel : BaseModel
    {
        private Dictionary<Employee, decimal> accruedSalaries;

        public SalaryModel()
        {
            accruedSalaries = new Dictionary<Employee, decimal>();
        }

        public IReadOnlyDictionary<Employee, decimal> AccruedSalaries
        {
            get
            {
                TimePeriod period = new TimePeriod(TypeTimePeriod.Month, DateTime.Today);

                accruedSalaries.Clear();
                foreach (Employee e in dbEntities.Employees.Local)
                {
                    // Оклад * кту + бонусы - штрафы
                    decimal salary = e.SumSalary * (decimal)GetRwp(e, period)
                        + dbEntities.BonusSalaries.Local.Where(b => b.IdEmployee == e.ID && b.Date >= period.Start).Sum(s => s.Sum)
                        - dbEntities.DeductionsFromSalary.Local.Where(d => d.IdEmployee == e.ID && d.Date >= period.Start).Sum(s => s.Sum);

                    accruedSalaries.Add(e, salary);
                }
                return accruedSalaries;
            }
        }

        public double GetRwp(Employee employee, TimePeriod timePeriod)
        {
            // Начислено очков КТУ за текущий период
            double sumPointsEmployee = dbEntities.AccrualsPointsRwp.Local.Where(p => p.IdEmployee == employee.ID && (p.Date >= timePeriod.Start || p.Date <= timePeriod.End)).Sum(p => p.Sum);

            // Сумма начисленных очков для всех остальных сотрудников
            double sumPointsEmployees = dbEntities.AccrualsPointsRwp.Local.Where(p => p.IdEmployee != employee.ID && (p.Date >= timePeriod.Start || p.Date <= timePeriod.End)).Sum(p => p.Sum);

            int countEmployees = dbEntities.Employees.Local.Count;

            if (sumPointsEmployee == 0 || sumPointsEmployees == 0 || countEmployees == 0)
            {
                return 1;
            }

            double rate = countEmployees * sumPointsEmployee / sumPointsEmployees;

            return rate;
        }

        public void PaySalaries()
        {
            TimePeriod period = new TimePeriod(TypeTimePeriod.Month, DateTime.Today);
            foreach (var accruedSalary in AccruedSalaries)
            {
                decimal paidSalary = accruedSalary.Key.salary_payments.Where(sp => sp.IdEmployee == accruedSalary.Key.ID && sp.Date >= period.Start).Sum(sp => sp.Sum);
                decimal salaryPrepay = accruedSalary.Key.payments_salary_prepay.Where(p => p.IdEmployee == accruedSalary.Key.ID && p.Date >= period.Start).Sum(p => p.Sum);
                decimal sumSalary = accruedSalary.Value - paidSalary - salaryPrepay;
                if (sumSalary <= 0) continue;

                SalaryPayment salaryPayment = new SalaryPayment() { Date = DateTime.Today, IdEmployee = accruedSalary.Key.ID, Sum = sumSalary };
                dbEntities.AddEntity(salaryPayment);
                dbEntities.SaveChanges();
            }
        }
    }
}
