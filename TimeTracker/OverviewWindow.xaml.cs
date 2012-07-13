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
    /// Interaction logic for OverviewWindow.xaml
    /// </summary>
    public partial class OverviewWindow : Window
    {
        public Birko.TimeTracker.Tracker Tracker { get; set; }
        private DateTime startTime = DateTime.UtcNow.Date;
        private int dayDelay = 1;

        public OverviewWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.RefreshData();
        }

        private void RefreshData()
        {
            if (this.Tracker != null)
            {
                DateTime endate = this.startTime.AddDays(this.dayDelay);
                IEnumerable<Birko.TimeTracker.Entities.Task> tasks = Tracker.Tasks.GetTasks(
                    t => t.Start >= this.startTime  &&
                        (!t.End.HasValue|| (t.End.HasValue && t.End.Value < endate))
                    );

                decimal sumtotal = (decimal)tasks.Sum(t => t.Duration.TotalHours);
                IQueryable<ChartItem> dataTask =
                        from task in tasks.AsQueryable()
                        group task by new
                        {
                            task.Name
                        }
                        into groupedTask
                        select new ChartItem
                        {
                            Text = groupedTask.Key.Name,
                            Value = (decimal)groupedTask.Sum(t=> t.Duration.TotalHours),
                            MaxValue = sumtotal,
                        };
                this.dataGridTasks.DataContext = dataTask.OrderBy(ch => ch.Text); ;
                IQueryable<ChartItem> dataCategory =
                        from task in tasks.AsQueryable()
                        group task by new
                        {
                            task.CategoryID,
                            task.Category,
                        }
                        into groupedTask
                        select new ChartItem
                        {
                                Text = groupedTask.Key.Category.Name,
                                Value = (decimal)groupedTask.Sum(t => t.Duration.TotalHours),
                                MaxValue = sumtotal,
                        };
                this.dataGridCategory.DataContext = dataCategory.OrderBy(ch=>ch.Text);
            }
        }
    }
}
