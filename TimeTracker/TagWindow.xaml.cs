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
    /// Interaction logic for TagWindow.xaml
    /// </summary>
    public partial class TagWindow : Window
    {
        public Birko.TimeTracker.Tracker.Tracker Tracker { get; set; }

        public TagWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            if (this.Tracker != null)
            {
                this.dataGridTags.DataContext = this.Tracker.Tags.GetTags().OrderBy(t => t.Name);
            }
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(this.textBoxTag.Text.Trim())&& this.Tracker != null)
            {
                string name = this.textBoxTag.Text.Trim();
                Birko.TimeTracker.Entities.Tag tag = this.Tracker.Tags.GetByName(name);
                this.RefreshData();
                if (tag != null)
                {
                    this.dataGridTags.SelectedItem = tag;
                }
            }
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGridTags.SelectedItem != null && this.Tracker != null)
            {
                Birko.TimeTracker.Entities.Tag tag = (this.dataGridTags.SelectedItem as Birko.TimeTracker.Entities.Tag);
                if (!String.IsNullOrEmpty(this.textBoxTag.Text.Trim()) && this.Tracker != null && tag != null)
                {
                    string name = this.textBoxTag.Text.Trim();
                    Birko.TimeTracker.Entities.Tag testTag = this.Tracker.Tags.GetByName(name);
                    if (testTag == null || testTag.ID == tag.ID)
                    {
                        tag.Name = name;
                        tag = this.Tracker.Tags.SaveTag(tag);
                    }
                    else
                    {
                        IEnumerable<Birko.TimeTracker.Entities.Task> tasks = this.Tracker.Tags.GetTasks(tag);
                        this.Tracker.Tags.DeleteTag(tag);
                        foreach (Birko.TimeTracker.Entities.Task task in tasks)
                        {
                            this.Tracker.TagTask(task, new Birko.TimeTracker.Entities.Tag[] { testTag }, false);
                        }
                        tag = testTag;

                    }
                    this.RefreshData();
                    if (tag != null)
                    {
                        this.dataGridTags.SelectedItem = tag;
                    }
                }
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGridTags.SelectedItem != null && this.Tracker != null)
            {
                Birko.TimeTracker.Entities.Tag tag = (this.dataGridTags.SelectedItem as Birko.TimeTracker.Entities.Tag);
                if (tag != null)
                {
                    this.Tracker.Tags.DeleteTag(tag);
                    this.RefreshData();
                }
            }
        }

        private void dataGridTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                Birko.TimeTracker.Entities.Tag tag = (e.AddedItems[0] as Birko.TimeTracker.Entities.Tag);
                this.textBoxTag.Text = tag.Name;
            }
        }
    }
}
