using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    class ReportsModel : BaseModel
    {
        private ProductModel productModel;
        private SalaryModel salaryModel;
        private FinanceModel financeModel;

        public ReportsModel(ProductModel productModel, SalaryModel salaryModel, FinanceModel financeModel)
        {
            this.productModel = productModel;
            this.salaryModel = salaryModel;
            this.financeModel = financeModel;
        }

        private DataTable CreateDataTable(Dictionary<string, Type> columsInfo) 
        {
            DataTable table = new DataTable();

            foreach (var item in columsInfo)
            {
                DataColumn column = new DataColumn(item.Key, item.Value) { ReadOnly = true };
                table.Columns.Add(column);
            }

            return table;
        }

        public DataTable GetSalaryReport()
        {
            TimePeriod period = new TimePeriod(TypeTimePeriod.Month, DateTime.Today);
            Dictionary<string, Type> columsInfo = new Dictionary<string, Type>
            {
                {"Фамилия", typeof(string)},
                {"Оклад", typeof(decimal)},
                {"Премии", typeof(decimal) },
                {"Штрафы", typeof(decimal) },
                {"По КТУ", typeof(decimal) },
                {"Начислено", typeof(decimal)},
                {"Сумма авансов", typeof(decimal) },
                {"Итого к оплате", typeof(decimal) }

            };
            DataTable table = CreateDataTable(columsInfo);

            foreach (Employee emp in dbEntities.Employees.Local)
            {
                decimal salary = emp.SumSalary;
                decimal accrued = salaryModel.AccruedSalaries[emp];
                decimal rwp = salary * (decimal)salaryModel.GetRwp(emp, period) - salary;
                decimal prepayments = emp.payments_salary_prepay.Where(p => p.Date >= period.Start && p.Date <= period.End).Sum(p => p.Sum);
                decimal deductions = dbEntities.DeductionsFromSalary.Local.Where(d => d.IdEmployee == emp.ID && d.Date >= period.Start && d.Date <= period.End).Sum(d => d.Sum);
                decimal bonuses = emp.bonus_salaries.Where(b => b.Date >= period.Start && b.Date <= period.End).Sum(b => b.Sum);
                decimal paidSum = emp.salary_payments.Where(p => p.Date >= period.Start && p.Date <= period.End).Sum(s => s.Sum);

                decimal sumPayments = accrued - prepayments - paidSum;

                object[] itemArray = new object[]
                {
                    emp.Surname,
                    salary,
                    bonuses,
                    deductions,
                    rwp,
                    accrued,
                    prepayments,
                    sumPayments
                };

                DataRow row = table.NewRow();
                row.ItemArray = itemArray;
                table.Rows.Add(row);
            }
            return table;
        }

        public DataTable GetReportOnOrders(DateTime startDate, DateTime endDate) 
        {
            if (startDate > endDate)
                throw new ArgumentException("Конечная дата меньше начальной");             

            Dictionary<string, Type> columsInfo = new Dictionary<string, Type>
            {
                {"Информация о заказе", typeof(string)},
                {"Стоимость заказа (руб.)", typeof(decimal) },
                {"Затраты на материалы (руб.)", typeof(decimal) },
                {"Прочие затраты (руб.)", typeof(decimal) },
                {"Себестоимость (руб.)", typeof(decimal) },
                {"Прибыль (руб.)", typeof(decimal) },
                {"Маржинальность (%)", typeof(decimal) },
                {"Рентабельность (%)", typeof(decimal) },

            };
            DataTable table = CreateDataTable(columsInfo);

            var orders = from order in dbEntities.Orders.Local
                         from contract in dbEntities.Contracts.Local.Where(c => c.IdOrder == order.ID)
                         where (TypeStatusOrder)order.IdStatusOrder == TypeStatusOrder.Completed && contract.DateSigning >= startDate && contract.DateSigning <= endDate
                         select new 
                         {
                             Order = order,
                             Contract = contract
                         };

            foreach (var o in orders)
            {
                decimal costsMaterials = 0;
                decimal primeCost = 0;
                decimal price = o.Contract.Price;

                var products = o.Order.products;
                string nameProducts = "";
                foreach (var product in products)
                {
                    primeCost += productModel.GetPrimeCost(product);
                    costsMaterials += dbEntities.ProductsMaterials.Local.Where(pm => pm.IdProduct == product.ID).Sum(pm => pm.QuantityMaterial * pm.UnitPrice);
                    nameProducts += product.types_products.Title + ", ";
                }

                decimal otherCost = primeCost - costsMaterials;
                decimal profit = price - primeCost;

                // Маржинальность = прибыль / стоимость заказа * 100
                decimal marginality = profit / price * 100;

                // Рентабельность = прибыль / себестоимость * 100
                decimal profitability = profit / primeCost * 100;

                string orderInfo = o.Contract.ProjectTitle + "\n" +
                                   o.Order.client.Surname + " " + o.Order.client.Name + " " + o.Order.client.Patronymic + "\n" +
                                   o.Order.client.PhoneNumber;

                DataRow row = table.NewRow();
                object[] itemArray = new object[]
                { 
                    orderInfo,
                    price,
                    costsMaterials,
                    otherCost,
                    primeCost,
                    profit,
                    marginality,
                    profitability
                };
                row.ItemArray = itemArray;
                table.Rows.Add(row);
            }

            return table;
        }

        public DataTable GetReportOnEmployees(DateTime startDate, DateTime endDate) 
        {
            if (startDate > endDate)
                throw new ArgumentException("Конечная дата меньше начальной");

            Dictionary<string, Type> columsInfo = new Dictionary<string, Type>
            {
                {"ФИО", typeof(string)},
                {"Должность", typeof(string)},
                {"Выплачено (руб.)", typeof(decimal) },
                {"Авансы (руб.)", typeof(decimal) },
                {"Штрафы (руб.)", typeof(decimal) },
                {"Премии (руб.)", typeof(decimal) },
                {"Коэффициент трудового участия", typeof(double) },

            };
            DataTable table = CreateDataTable(columsInfo);

            foreach (Employee e in dbEntities.Employees.Local)
            {
                decimal salary = e.SumSalary;

                string fullName = e.Surname + " " + e.Name + " " + e.Patronymic;
                string position = "";
                foreach (var pos in e.employees_positions)
                    position += pos.position.Title + " ";
                decimal paidSum = e.salary_payments.Where(sp => sp.Date >= startDate && sp.Date <= endDate).Sum(sp => sp.Sum);
                decimal prepayments = e.payments_salary_prepay.Where(p => p.Date >= startDate && p.Date <= endDate).Sum(p => p.Sum);
                decimal deductions = dbEntities.DeductionsFromSalary.Local.Where(d => d.IdEmployee == e.ID && d.Date >= startDate && d.Date <= endDate).Sum(d => d.Sum);
                decimal bonuses = e.bonus_salaries.Where(b => b.Date >= startDate && b.Date <= endDate).Sum(b => b.Sum);
                double rwp = salaryModel.GetRwp(e, new TimePeriod(startDate, endDate));

                object[] itemArray = new object[] 
                {
                    fullName,
                    position,
                    paidSum,
                    prepayments,
                    deductions,
                    bonuses,
                    rwp
                };
                DataRow row = table.NewRow();
                row.ItemArray = itemArray;
                table.Rows.Add(row);
            }

            return table;
        }

        public DataTable GetReportOnClients(DateTime startDate, DateTime endDate) 
        {
            if (startDate > endDate)
                throw new ArgumentException("Конечная дата меньше начальной");

            Dictionary<string, Type> columsInfo = new Dictionary<string, Type>
            {
                {"ФИО", typeof(string)},
                {"Номер телефона", typeof(decimal)},
                {"Количество заказов", typeof(int) },
                {"Стоимость заказов (руб.)", typeof(decimal) },
                {"Себестоимость заказов (руб.)", typeof(decimal) },
                {"Прибыль (руб.)", typeof(decimal) },
                {"Маржинальность (%)", typeof(decimal) },
                {"Рентабельность (%)", typeof(decimal) },

            };
            DataTable table = CreateDataTable(columsInfo);

            foreach (Client client in dbEntities.Clients.Local)
            {
                string fullName = client.Surname + " " + client.Name + " " + client.Patronymic;
                decimal phoneNumber = client.PhoneNumber;
                int amountOrders = client.orders.Count;

                var ordersInfo = from order in client.orders
                                 from contract in order.contracts
                                 where contract.DateSigning >= startDate &&
                                       contract.DateSigning <= endDate &&
                                       (TypeStatusOrder)order.IdStatusOrder == TypeStatusOrder.Completed
                                 select new
                                 {
                                     Products = order.products,
                                     Price = order.contracts.Sum(c => c.Price),
                                 };

                decimal ordersPrice = 0;
                decimal ordersPrimeCost = 0;
                foreach (var orderInfo in ordersInfo)
                {
                    ordersPrice += orderInfo.Price;
                    ordersPrimeCost += orderInfo.Products.Sum(p => productModel.GetPrimeCost(p));
                }

                decimal ordersProfit = ordersPrice - ordersPrimeCost;
                decimal marginality = ordersProfit / (ordersPrice == 0 ? 1 : ordersPrice) * 100;
                decimal profitability = ordersProfit / (ordersPrimeCost == 0 ? 1 : ordersPrimeCost) * 100;

                object[] itemArray = new object[]
                {
                    fullName,
                    phoneNumber,
                    amountOrders,
                    ordersPrice,
                    ordersPrimeCost,
                    ordersProfit,
                    marginality,
                    profitability
                };

                DataRow row = table.NewRow();
                row.ItemArray = itemArray;
                table.Rows.Add(row);
            }

            return table;
        }

        public DataTable GetReportOnFinance(DateTime startDate, DateTime endDate) 
        {
            if (startDate > endDate)
                throw new ArgumentException("Конечная дата меньше начальной");

            Dictionary<string, Type> columsInfo = new Dictionary<string, Type>
            {
                {"Затраты на зарплату (руб.)", typeof(decimal)},
                {"Затраты на материалы (руб.)", typeof(decimal)},
                {"Постоянные расходы (руб.)", typeof(decimal)},
                {"Прочие расходы (руб.)", typeof(decimal)},
                {"Общие затраты (руб.)", typeof(decimal) },
                {"Доход (руб.)", typeof(decimal) },
                {"Прибыль (руб.)", typeof(decimal) },
                {"Рентабельность (%)", typeof(decimal) },
            };
            DataTable table = CreateDataTable(columsInfo);

            decimal salaryCosts = dbEntities.SalaryPayments.Local.Where(s => s.Date >= startDate && s.Date <= endDate).Sum(s => s.Sum)
                + dbEntities.PaymentsSalaryPrepay.Local.Where(s => s.Date >= startDate && s.Date <= endDate).Sum(s => s.Sum)
                + dbEntities.BonusSalaries.Local.Where(b => b.Date >= startDate && b.Date <= endDate).Sum(s => s.Sum)
                - dbEntities.DeductionsFromSalary.Local.Where(d => d.Date >= startDate && d.Date <= endDate).Sum(s => s.Sum);
            decimal costsMaterials = dbEntities.PackingLists.Local.Where(p => p.Date >= startDate && p.Date <= endDate).Sum(p => p.records_packing_list.Sum(r => r.QuantityUnits * r.UnitPrice));
            decimal fixedCosts = dbEntities.PaymentsFixedCosts.Local.Where(f => f.Date >= startDate && f.Date <= endDate).Sum(f => f.fixed_costs.Sum);
            decimal otherCosts = dbEntities.OtherCosts.Local.Where(o => o.Date >= startDate && o.Date <= endDate).Sum(o => o.Sum);

            FinancialIndicators indicators = financeModel.GetFinancialIndicators(startDate, endDate);
            decimal profitability = indicators.Profit / (indicators.Costs == 0 ? 1 : indicators.Costs) * 100;

            Dictionary<string, decimal> rowsInfo = new Dictionary<string, decimal>()
            {
                { "Затраты на зарплату (руб.)", salaryCosts},
                { "Затраты на материалы (руб.)", costsMaterials},
                { "Постоянные расходы (руб.)", fixedCosts},
                { "Прочие расходы (руб.)", otherCosts},
                { "Общие затраты (руб.)", indicators.Costs },
                { "Доход (руб.)", indicators.Revenue },
                { "Прибыль (руб.)", indicators.Profit },
                { "Рентабельность (%)", profitability }
            };

            object[] itemArray = new object[]
            {
                    salaryCosts,
                    costsMaterials,
                    fixedCosts,
                    otherCosts,
                    indicators.Costs,
                    indicators.Revenue,
                    indicators.Profit,
                    profitability
            };

            DataRow row = table.NewRow();
            row.ItemArray = itemArray;
            table.Rows.Add(row);

            return table;
        }
    }
}
