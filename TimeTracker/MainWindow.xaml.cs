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
        private Birko.TimeTracker.Tracker.Tracker tracker = null;
        private DateTime startTime = DateTime.UtcNow.Date;

        public MainWindow()
        {
            InitializeComponent();
            Birko.TimeTracker.DbContext.EntityManager manager  = new Birko.TimeTracker.DbContext.EntityManager("Data Source=timetracker.sdf", "System.Data.SqlServerCe.4.0");
            this.tracker = new Birko.TimeTracker.Tracker.Tracker(manager);
            this.tracker.OnTaskStarted += tracker_OnTaskStarted;
            this.tracker.OnTaskEnded += tracker_OnTaskEnded;
            this.tracker.OnTaskDeleted += tracker_OnTaskDeleted;
        }

        void tracker_OnTaskDeleted(Birko.TimeTracker.Entities.Task task)
        {
            this.RefreshTaskList();
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
            this.textBoxTags.Text = string.Empty;
            this.textBoxTask.Text = string.Empty;
        }

        private void RefreshTaskList()
        {
            IEnumerable<Birko.TimeTracker.Entities.Task> tasks = tracker.Tasks.GetTasks(t=>t.Start >= this.startTime).OrderBy(t=> t.Start);
            this.dataGridTasks.DataContext = tasks;
            if (tasks != null && tasks.Count() > 0)
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

        private void overviewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OverviewWindow window = new OverviewWindow();
            window.Tracker = this.tracker;
            window.Owner = this;
            window.Show();
        }

        private void newTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow window = new TaskWindow();
            window.Tracker = this.tracker;
            window.Owner = this;
            window.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.dataGridTasks.SelectedItem != null)
            {
                Birko.TimeTracker.Entities.Task task = (this.dataGridTasks.SelectedItem as Birko.TimeTracker.Entities.Task);
                TaskWindow window = new TaskWindow();
                window.Task = task;
                if (this.tracker.ActiveTask != null)
                {
                    window.Tags = this.tracker.Tasks.GetTags(task);
                }
                window.Tracker = this.tracker;
                window.Owner = this;
                window.Show();
            }
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGridTasks.SelectedItem != null)
            {
                Birko.TimeTracker.Entities.Task task = (this.dataGridTasks.SelectedItem as Birko.TimeTracker.Entities.Task);
                this.tracker.RemoveTask(task);
            }
        }

        private void dataGridTasks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.dataGridTasks.SelectedItem != null)
            {
                Birko.TimeTracker.Entities.Task selectedTask = (this.dataGridTasks.SelectedItem as Birko.TimeTracker.Entities.Task);
                Birko.TimeTracker.Entities.Task task = this.tracker.Tasks.NewTask();
                task.Name = selectedTask.Name;
                task.Start = DateTime.UtcNow;
                task.CategoryID = selectedTask.CategoryID;
                List<Birko.TimeTracker.Entities.Tag> tags = new List<Birko.TimeTracker.Entities.Tag>();
                IEnumerable <Birko.TimeTracker.Entities.Tag> taskTags =  this.tracker.Tasks.GetTags(selectedTask);
                foreach (Birko.TimeTracker.Entities.Tag tag in taskTags)
                {
                    tags.Add(tag);
                }
                tracker.SwitchTask(task);
                tracker.TagTask(task, tags);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.RefreshTaskList();
        }
    }
}
