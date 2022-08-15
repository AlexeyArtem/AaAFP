using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AaAFP2
{
    class ICollectionEmployeesPositionsToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ICollection<EmployeePosition> collection = value as ICollection<EmployeePosition>;
            if (collection == null)
                throw new ArgumentException("Передаваемый объект не является коллекцией объектов EmployeePosition");

            if (targetType != typeof(string))
                throw new InvalidOperationException("Целью преобразования должна быть строка");

            List<string> titles = new List<string>();
            foreach (var item in collection)
            {
                titles.Add(item.position.Title);
            }
            

            return String.Join(", ", titles.ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
