using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    internal class ChartItem
    {
        public string Text { get; set; }
        public decimal Value { get; set; }
        public decimal MaxValue { get; set; }

        public decimal Percentage
        {
            get 
            {
                return Value / MaxValue * 100;
            }
        }
    }
}
