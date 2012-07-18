using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for IntervalWindow.xaml
    /// </summary>
    public partial class IntervalWindow : Window
    {
        public DateTime? StartDate 
        {
            get 
            {
                return this.datePickerStart.SelectedDate;
            }
            set
            {
                if (value.HasValue)
                {
                    this.datePickerStart.SelectedDate = value.Value.ToLocalTime();
                }
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return this.datePickerEnd.SelectedDate;
            }
            set
            {
                if (value.HasValue)
                {
                    this.datePickerEnd.SelectedDate = value.Value.ToLocalTime();
                }
            }
        }

        public IntervalWindow()
        {
            InitializeComponent();
            this.datePickerStart.SelectedDate = DateTime.Now;
            this.datePickerEnd.SelectedDate = DateTime.Now;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
