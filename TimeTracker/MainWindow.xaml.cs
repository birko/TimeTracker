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
        public MainWindow()
        {
            InitializeComponent();
            Birko.TimeTracker.DbContext.EntityManager manager  = new Birko.TimeTracker.DbContext.EntityManager();
            this.tracker = new Birko.TimeTracker.Tracker(manager);
            
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
                this.RefreshTaskList();
            }
        }

        private void RefreshTaskList()
        {
            IEnumerable<Birko.TimeTracker.Entities.Task> task = tracker.Tasks.GetTasks();
        }

        private void buttonStopTracking_Click(object sender, RoutedEventArgs e)
        {
            tracker.EndTask(tracker.ActiveTask);
            this.RefreshTaskList();
        }

    }
}
