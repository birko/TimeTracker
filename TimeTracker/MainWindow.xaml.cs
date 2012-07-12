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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Birko.TimeTracker.Tracker tracker = null;
        private DateTime startTime = DateTime.UtcNow.Date;

        public MainWindow()
        {
            InitializeComponent();
            Birko.TimeTracker.DbContext.EntityManager manager  = new Birko.TimeTracker.DbContext.EntityManager("Data Source=timetracker.sdf", "System.Data.SqlServerCe.4.0");
            this.tracker = new Birko.TimeTracker.Tracker(manager);
            this.tracker.OnTaskStarted += tracker_OnTaskStarted;
            this.tracker.OnTaskEnded += tracker_OnTaskEnded;
        }

        void tracker_OnTaskEnded(Birko.TimeTracker.Entities.Task task)
        {
            this.labelTaskName.Content = string.Empty;
            this.labelTaskDuration.Content = string.Empty;
            this.buttonStopTracking.IsEnabled = false;
            this.RefreshTaskList();
        }

        private void tracker_OnTaskStarted(Birko.TimeTracker.Entities.Task task)
        {
            if (task != null)
            {
                this.labelTaskName.Content = task.FullName;
                this.labelTaskDuration.Content = task.Duration.ToString("c");
                this.buttonStopTracking.IsEnabled = true;
            }
            this.RefreshTaskList();
        }

        private void buttonSwitchTask_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(this.textBoxTask.Text.Trim()))
            {
                string[] names = this.textBoxTask.Text.Trim().Split('@');
                string categoryName = (names.Length > 1) ? names[1] : "None";
                string[] tagNames = this.textBoxTags.Text.Trim().Split(',');

                Birko.TimeTracker.Entities.Category category = tracker.Categories.GetByName(categoryName);
                Birko.TimeTracker.Entities.Task task = tracker.Tasks.NewTask();
                task.Name = names[0];
                task.Start = DateTime.UtcNow;
                task.CategoryID = category.ID;
                List<Birko.TimeTracker.Entities.Tag> tags = new List<Birko.TimeTracker.Entities.Tag>();
                foreach (string tagName in tagNames)
                {
                    string tagNameTrim = tagName.Trim();
                    if (!string.IsNullOrEmpty(tagNameTrim))
                    {
                        Birko.TimeTracker.Entities.Tag tag = tracker.Tags.GetByName(tagNameTrim);
                        tags.Add(tag);
                    }
                }

                tracker.SwitchTask(task);
                tracker.TagTask(task, tags);
            }
        }

        private void RefreshTaskList()
        {
            IEnumerable<Birko.TimeTracker.Entities.Task> tasks = tracker.Tasks.GetTasks(t=>t.Start >= this.startTime).OrderBy(t=> t.Start);
            this.dataGridTasks.DataContext = tasks;
            if (tasks != null && tasks.Count() > 1)
            {
                this.labelTotalTime.Content = new TimeSpan(tasks.Sum(t => t.Duration.Ticks)).ToString("c");
            }
            else 
            {
                this.labelTotalTime.Content = 0;
            }
        }

        private void buttonStopTracking_Click(object sender, RoutedEventArgs e)
        {
            tracker.EndTask(tracker.ActiveTask);
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.InitTaskList();
            this.RefreshTaskList();
        }

        private void InitTaskList()
        {
            Birko.TimeTracker.Entities.Task task = this.tracker.Tasks.GetTasks(t => t.End == null).FirstOrDefault();
            if (task != null)
            {
                this.tracker.SwitchTask(task);
                if (task.Start.Value.Date < this.startTime.Date)
                {
                    this.startTime = task.Start.Value;
                }
            }
        }
    }
}
