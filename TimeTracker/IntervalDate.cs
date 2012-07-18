using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    enum DelayType 
    {
        Day,
        Week,
        Month,
        Year,
    }
    class IntervalDate
    {
        public DateTime Date { get; set; }
        public string Label { get; set; }
        public int Delay { get; set; }
        public DelayType DelayType { get; set; }
        public string DateFormat { get; set; }

        public DateTime NextDate
        {
            get
            {
                switch (this.DelayType)
                {
                    case TimeTracker.DelayType.Year:
                        return this.Date.AddYears(this.Delay);
                    case TimeTracker.DelayType.Month:
                        return this.Date.AddMonths(this.Delay);
                    case TimeTracker.DelayType.Week:
                        return this.Date.AddDays(this.Delay * 7);
                    case TimeTracker.DelayType.Day: 
                    default:
                        return this.Date.AddDays(this.Delay);
                }
            }
        }

        public DateTime PrevDate
        {
            get
            {
                switch (this.DelayType)
                {
                    case TimeTracker.DelayType.Year:
                        return this.Date.AddYears((-1) * this.Delay);
                    case TimeTracker.DelayType.Month:
                        return this.Date.AddMonths((-1) * this.Delay);
                    case TimeTracker.DelayType.Week:
                        return this.Date.AddDays((-1) * this.Delay * 7);
                    case TimeTracker.DelayType.Day:
                    default:
                        return this.Date.AddDays((-1) * this.Delay);
                }
            }
        }

        public IntervalDate()
        {
            Delay = 1;
            DelayType = TimeTracker.DelayType.Day;
            Date = DateTime.UtcNow;
            DateFormat = "dd.MM.yyyy";
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}", this.Label, this.Date.ToLocalTime().ToString(this.DateFormat));
        }
        
    }
}
