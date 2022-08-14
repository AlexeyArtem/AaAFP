using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    public struct TimePeriod
    {
        private TypeTimePeriod type;
        private DateTime currentDate;

        public TimePeriod(TypeTimePeriod typeTimePeriod, DateTime currentDate) 
        {
            type = typeTimePeriod;
            this.currentDate = currentDate;
            DateTime start = currentDate;
            DateTime end = currentDate;
            switch (typeTimePeriod)
            {
                case TypeTimePeriod.Week:
                    int startDay = currentDate.Day - (int)currentDate.DayOfWeek;
                    start = new DateTime(currentDate.Year, currentDate.Month, startDay == 0 ? 1 : startDay);

                    int endDay = currentDate.Day + (DayOfWeek.Saturday + 1 - currentDate.DayOfWeek);
                    end = new DateTime(currentDate.Year, currentDate.Month, endDay);
                    break;
                case TypeTimePeriod.Month:
                    start = new DateTime(currentDate.Year, currentDate.Month, 1);
                    end = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                    break;
            }
            Start = start;
            End = end;
        }

        public TimePeriod(DateTime dateStart, DateTime dateEnd)
        {
            if (dateStart > dateEnd)
                throw new ArgumentException("Конечная дата меньше начальной");

            type = TypeTimePeriod.Month;
            currentDate = DateTime.Today;
            Start = dateStart;
            End = dateEnd;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
    }
}
