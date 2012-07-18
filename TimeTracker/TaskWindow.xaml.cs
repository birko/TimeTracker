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
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private Birko.TimeTracker.Entities.Task task = null;
        private IEnumerable<Birko.TimeTracker.Entities.Tag> tags = null;

        public Birko.TimeTracker.Tracker.Tracker Tracker { get; set; }

        public IEnumerable<Birko.TimeTracker.Entities.Tag> Tags
        {
            get { return tags; }
            set 
            { 
                tags = value;
                if (tags != null && tags.Count() > 0)
                {
                    bool start = true;
                    foreach (Birko.TimeTracker.Entities.Tag tag in tags)
                    {
                        if (!start)
                        {
                            this.textBoxTags.Text += ", ";
                        }
                        else
                        {
                            start = false;
                        }
                        this.textBoxTags.Text += tag.Name;
                    }
                }
            }
        }

        public Birko.TimeTracker.Entities.Task Task
        {
            get { return task; }
            set 
            { 
                task = value;
                if (this.task != null)
                {
                    this.textBoxTask.Text = this.task.FullName;
                    if (this.Task.Description != null)
                    {
                        this.textBoxDescription.Text = this.Task.Description.Trim();
                    }
                    this.datePickerStart.SelectedDate = this.task.Start.Value.ToLocalTime().Date;
                    this.comboBoxHourStart.SelectedItem = this.task.Start.Value.ToLocalTime().Hour;
                    this.comboBoxMinuteStart.SelectedItem = this.task.Start.Value.ToLocalTime().Minute;
                    if (this.task.End.HasValue)
                    {
                        this.checkBoxSetEnd.IsChecked = true;
                        this.datePickerEnd.SelectedDate = this.task.End.Value.ToLocalTime().Date;
                        this.comboBoxHourEnd.SelectedItem = this.task.End.Value.ToLocalTime().Hour;
                        this.comboBoxMinuteEnd.SelectedItem = this.task.End.Value.ToLocalTime().Minute;
                    }
                }
            }

        }
        public TaskWindow()
        {
            InitializeComponent();
            this.datePickerStart.SelectedDate = DateTime.Now.Date;
            this.comboBoxHourStart.SelectedItem = DateTime.Now.Hour;
            this.comboBoxMinuteStart.SelectedItem = DateTime.Now.Minute;
            if (this.checkBoxSetEnd.IsChecked == true)
            {
                this.datePickerEnd.SelectedDate = DateTime.Now.Date;
                this.comboBoxHourEnd.SelectedItem = DateTime.Now.Hour;
                this.comboBoxMinuteEnd.SelectedItem = DateTime.Now.Minute;
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (this.Tracker != null)
            {
                if (this.task == null)
                {
                    this.task = this.Tracker.Tasks.NewTask();
                    this.task.ID = Guid.NewGuid();
                }
                string[] names = this.textBoxTask.Text.Trim().Split('@');
                string categoryName = (names.Length > 1) ? names[1] : "None";
                string[] tagNames = this.textBoxTags.Text.Trim().Split(',');

                Birko.TimeTracker.Entities.Category category = Tracker.Categories.GetByName(categoryName);
                this.task.Name = names[0];
                if (this.datePickerStart.SelectedDate.HasValue )
                {
                    this.task.Start = new DateTime(this.datePickerStart.SelectedDate.Value.Year, this.datePickerStart.SelectedDate.Value.Month, this.datePickerStart.SelectedDate.Value.Day, (int)this.comboBoxHourStart.SelectedItem, (int)this.comboBoxMinuteStart.SelectedItem, 0).ToUniversalTime();
                }
                if (this.checkBoxSetEnd.IsChecked == true)
                {
                    if (this.datePickerEnd.SelectedDate.HasValue)
                    {
                        this.task.End = new DateTime(this.datePickerEnd.SelectedDate.Value.Year, this.datePickerEnd.SelectedDate.Value.Month, this.datePickerEnd.SelectedDate.Value.Day, (int)this.comboBoxHourEnd.SelectedItem, (int)this.comboBoxMinuteEnd.SelectedItem, 0).ToUniversalTime();
                    }
                }

                this.task.CategoryID = category.ID;
                List<Birko.TimeTracker.Entities.Tag> tags = new List<Birko.TimeTracker.Entities.Tag>();
                foreach (string tagName in tagNames)
                {
                    string tagNameTrim = tagName.Trim();
                    if (!string.IsNullOrEmpty(tagNameTrim))
                    {
                        Birko.TimeTracker.Entities.Tag tag = this.Tracker.Tags.GetByName(tagNameTrim);
                        tags.Add(tag);
                    }
                }
                this.task.Description = this.textBoxDescription.Text.Trim();
                this.tags = tags;
                this.Tracker.SwitchTask(this.task);
                this.Tracker.TagTask(task, tags);
                this.Close();
            }
        }

        private void checkBoxSetEnd_Checked(object sender, RoutedEventArgs e)
        {
            if (this.checkBoxSetEnd.IsChecked == true)
            {
                this.datePickerEnd.SelectedDate = DateTime.Now.Date;
                this.comboBoxHourEnd.SelectedItem = DateTime.Now.Hour;
                this.comboBoxMinuteEnd.SelectedItem = DateTime.Now.Minute;
            }
        }
    }
}
