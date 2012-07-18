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
        public Birko.TimeTracker.Tracker.Tracker Tracker { get; set; }
        private DateTime startTime = DateTime.UtcNow.Date;

        public OverviewWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.RefreshInterval();
            this.comboBoxInterval.SelectedIndex = 0;
        }


        private void RefreshData()
        {
            if (this.Tracker != null && this.comboBoxInterval.SelectedItem != null)
            {

                IntervalDate interval = (this.comboBoxInterval.SelectedItem as IntervalDate);
                IEnumerable<Birko.TimeTracker.Entities.Task> tasks = Tracker.Tasks.GetTasks(
                    t => t.Start >= interval.Date.Date &&
                    (!t.End.HasValue|| (t.End.HasValue && t.End.Value < interval.NextDate.Date))
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
                this.dataGridTasks.DataContext = dataTask.OrderBy(ch => ch.Text);
                /*IQueryable<ChartItem> dataTag =
                       from tag in tasks.SelectMany(t=>t.Tags).AsQueryable()
                       group tag by new
                       {
                           tag.ID,
                           tag.Name
                       }
                           into groupedTask
                           select new ChartItem
                           {
                               Text = groupedTask.Key.Name,
                               Value = (decimal)groupedTask.Sum(t => t.Tasks.Sum(i=>i.Duration.TotalHours)),
                               MaxValue = sumtotal,
                           };
                this.dataGridTags.DataContext = dataTag.OrderBy(ch => ch.Text); */
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
                this.labelTotal.Content = sumtotal.ToString();
            }
        }

        private void RefreshInterval()
        {
            int index = this.comboBoxInterval.SelectedIndex;
            IntervalDate custom = null;
            if (this.comboBoxInterval.Items.Count > 4)
            {
                IntervalDate tmp = (this.comboBoxInterval.Items[4] as IntervalDate);
                custom = new IntervalDate() 
                {
                    Label = "Custom", 
                    Date = this.startTime,
                    DateFormat = tmp.DateFormat,
                    DelayType = tmp.DelayType,
                    Delay = tmp.Delay 
                };
            }
            this.comboBoxInterval.Items.Clear();
            this.comboBoxInterval.Items.Add(new IntervalDate() { 
                 Label = "Day",
                 Date = this.startTime.Date,
                 Delay = 1
            });

            IntervalDate week = new IntervalDate() { Label = "Week", DelayType = DelayType.Week };
            switch(this.startTime.DayOfWeek)
            {
                case DayOfWeek.Monday: 
                    week.Date = this.startTime;
                    break;
                case DayOfWeek.Tuesday:
                     week.Date = this.startTime.AddDays(-1);
                    break;
                case DayOfWeek.Wednesday: 
                     week.Date = this.startTime.AddDays(-2);
                    break;
                case DayOfWeek.Thursday: 
                     week.Date =  this.startTime.AddDays(-3);
                    break;
                 case DayOfWeek.Friday:
                    week.Date = this.startTime.AddDays(-4);
                    break;
                 case DayOfWeek.Saturday: 
                     week.Date = this.startTime.AddDays(-5);
                    break;
                case DayOfWeek.Sunday: 
                     week.Date = this.startTime.AddDays(-6);
                    break;

            }
            this.comboBoxInterval.Items.Add(week);
            IntervalDate month = new IntervalDate() { Label = "Month", Date = new DateTime(this.startTime.Year, this.startTime.Month, 1), DateFormat = "MM.yyyy", DelayType = DelayType.Month};
            this.comboBoxInterval.Items.Add(month);
            IntervalDate year = new IntervalDate() { Label = "Year", Date = new DateTime(this.startTime.Year, this.startTime.Month, 1), DateFormat = "yyyy", DelayType = DelayType.Year };
            this.comboBoxInterval.Items.Add(year);
            if (custom != null)
            {
                this.comboBoxInterval.Items.Add(custom);
            }
            this.comboBoxInterval.SelectedIndex = index;
        }

        private void buttonPrev_Click(object sender, RoutedEventArgs e)
        {
            if(this.comboBoxInterval.SelectedItem != null)
            {
                this.startTime = (this.comboBoxInterval.SelectedItem as IntervalDate).PrevDate; 
            }
            this.RefreshInterval();
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (this.comboBoxInterval.SelectedItem != null)
            {
                this.startTime = (this.comboBoxInterval.SelectedItem as IntervalDate).NextDate;
            }
            this.RefreshInterval();
        }

        private void buttonFirst_Click(object sender, RoutedEventArgs e)
        {
            if(this.Tracker != null)
            {
                Birko.TimeTracker.Entities.Task task = this.Tracker.Tasks.GetFirstTask();
                if(task != null)
                {
                    this.startTime = task.Start.Value.Date;
                    this.RefreshInterval();
                }
            }
        }

        private void buttonLast_Click(object sender, RoutedEventArgs e)
        {
            if (this.Tracker != null)
            {
                Birko.TimeTracker.Entities.Task task = this.Tracker.Tasks.GetLastTask();
                if (task != null)
                {
                    if (task.End.HasValue)
                    {
                        this.startTime = task.End.Value.Date;
                    }
                    else
                    {
                        this.startTime = DateTime.UtcNow.Date;
                    }
                    this.RefreshInterval();
                }
            }
        }

        private void comboBoxInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.comboBoxInterval.SelectedIndex;
            if (index >= 0)
            {
                this.RefreshData();
            }
        }

        private void buttonHome_Click(object sender, RoutedEventArgs e)
        {

            this.startTime = DateTime.UtcNow.Date;
            this.RefreshInterval();
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            IntervalWindow window = new IntervalWindow();
            window.Owner = this;
            window.StartDate = this.startTime;
            if (window.ShowDialog() == true)
            {
                int delay = (int)(window.EndDate.Value.AddDays(1).Date - window.StartDate.Value.Date).TotalDays;
                IntervalDate custom = new IntervalDate() { Label = "Custom", Date = window.StartDate.Value.ToUniversalTime().Date, DateFormat = "dd.MM.yyyy", DelayType = DelayType.Day, Delay= delay };
                this.comboBoxInterval.Items.Add(custom);
                this.comboBoxInterval.SelectedIndex = this.comboBoxInterval.Items.Count - 1;
            }
        }
    }
}
