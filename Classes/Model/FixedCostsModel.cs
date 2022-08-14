using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AaAFP2
{
    class FixedCostsModel : BaseModel
    {
        private Timer timer;

        public FixedCostsModel() 
        {
            CheckAndPay(null, null);

            timer = new Timer(86400000); // Интервал - раз в сутки (86400000 млс)
            timer.Elapsed += CheckAndPay;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
        }

        private void CheckAndPay(Object source, ElapsedEventArgs e)
        {
            foreach (var cost in dbEntities.FixedCosts.Local)
            {
                DateTime today = DateTime.Today;

                PaymentFixedCost oldPayment = dbEntities.PaymentsFixedCosts.Local.Where(p => p.IdFixedCost == cost.ID && p.Date == today).FirstOrDefault();
                if (oldPayment != null)
                    continue;
                
                WeeklyCost weeklyCost = dbEntities.WeeklyCosts.Local.Where(w => w.IdFixedCost == cost.ID).FirstOrDefault();
                MonthlyCost montlyCost = dbEntities.MonthlyCosts.Local.Where(m => m.IdFixedCost == cost.ID).FirstOrDefault();
                
                if (weeklyCost != null && (int)today.DayOfWeek + 1 == weeklyCost.DayAccrual)
                {
                    var payment = new PaymentFixedCost() { IdFixedCost = weeklyCost.IdFixedCost, Date = today };
                    dbEntities.AddEntity(payment);
                }
                else if (montlyCost != null && today.Day == montlyCost.DayAccrual) 
                {
                    var payment = new PaymentFixedCost() { IdFixedCost = montlyCost.IdFixedCost, Date = today };
                    dbEntities.AddEntity(payment);
                }
                dbEntities.SaveChanges();
            }
        }

        public void AddCost(FixedCost fixedCost, TypeTimePeriod typePeriod, int dayAccrual) 
        {
            if (typePeriod == TypeTimePeriod.Week && (dayAccrual <= 0 || dayAccrual > 7))
                throw new ArgumentException("Не верно указан день начисления. Значение выходит за рамки допустимого.");
            else if (dayAccrual <= 0 || dayAccrual > 30)
                throw new ArgumentException("Не верно указан день начисления. Значение выходит за рамки допустимого.");

            if (dbEntities.WeeklyCosts.Local.Where(c => c.IdFixedCost == fixedCost.ID).Count() != 0 ||
                dbEntities.MonthlyCosts.Local.Where(c => c.IdFixedCost == fixedCost.ID).Count() != 0) 
            {
                throw new ArgumentException("Данный регулярный расход уже добавлен");
            }
            
            if (!dbEntities.FixedCosts.Local.Contains(fixedCost)) 
            {
                dbEntities.AddEntity(fixedCost);
                dbEntities.SaveChanges();
            }

            if (typePeriod == TypeTimePeriod.Week)
            {
                WeeklyCost weeklyCost = new WeeklyCost()
                {
                    IdFixedCost = fixedCost.ID,
                    DayAccrual = dayAccrual
                };
                dbEntities.AddEntity(weeklyCost);
            }
            else 
            {
                MonthlyCost monthlyCost = new MonthlyCost()
                {
                    IdFixedCost = fixedCost.ID,
                    DayAccrual = dayAccrual
                };
                dbEntities.AddEntity(monthlyCost);
            }
            dbEntities.SaveChanges();
        }
    }
}
