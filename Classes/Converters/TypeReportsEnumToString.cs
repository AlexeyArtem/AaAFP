using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AaAFP2
{
    class TypeReportsEnumToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("Целью преобразования должна быть строка");
            
            TypeReport type = (TypeReport)value;
            string desc = "";
            switch (type) 
            {
                case TypeReport.OnClients:
                    desc = "Отчет по клиентам";
                    break;
                case TypeReport.OnEmployees:
                    desc = "Отчет по сотрудникам";
                    break;
                case TypeReport.OnFinance:
                    desc = "Отчет по финансам";
                    break;
                case TypeReport.OnOrders:
                    desc = "Отчет по заказам";
                    break;
                case TypeReport.OnSalaries:
                    desc = "Отчет по зарплатам";
                    break;
            }

            return desc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
