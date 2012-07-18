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
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {
        public Birko.TimeTracker.Tracker.Tracker Tracker { get; set; }

        public CategoryWindow()
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
                this.dataGridCategories.DataContext = this.Tracker.Categories.GetCategories().OrderBy(t => t.Name);
            }
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(this.textBoxCategory.Text.Trim()) && this.Tracker != null)
            {
                string name = this.textBoxCategory.Text.Trim();
                Birko.TimeTracker.Entities.Category category = this.Tracker.Categories.GetByName(name);
                string group = this.textBoxGroup.Text.Trim();
                if (!String.IsNullOrEmpty(group))
                {
                    category.Group = group;
                }
                this.Tracker.Categories.EditCategory(category);
                this.RefreshData();
                if (category != null)
                {
                    this.dataGridCategories.SelectedItem = category;
                }
            }
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGridCategories.SelectedItem != null && this.Tracker != null)
            {
                Birko.TimeTracker.Entities.Category category = (this.dataGridCategories.SelectedItem as Birko.TimeTracker.Entities.Category);
                if (!String.IsNullOrEmpty(this.textBoxCategory.Text.Trim()) && this.Tracker != null && category != null)
                {
                    string name = this.textBoxCategory.Text.Trim();
                    Birko.TimeTracker.Entities.Category testCategory = this.Tracker.Categories.GetByName(name);
                    if (testCategory == null || testCategory.ID == category.ID)
                    {
                        string group = this.textBoxGroup.Text.Trim();
                        if (!String.IsNullOrEmpty(group))
                        {
                            category.Group = group;
                        }
                        this.Tracker.Categories.EditCategory(category);
                    }
                    else
                    {
                        IEnumerable<Birko.TimeTracker.Entities.Task> tasks = this.Tracker.Categories.GetTasks(category);
                        this.Tracker.Categories.DeleteCategory(category);
                        string group = this.textBoxGroup.Text.Trim();
                        if (!String.IsNullOrEmpty(group))
                        {
                            testCategory.Group = group;
                        }
                        foreach (Birko.TimeTracker.Entities.Task task in tasks)
                        {
                            task.CategoryID = testCategory.ID;
                            this.Tracker.Tasks.EditTask(task);
                        }
                        category = testCategory;
                    }
                    this.RefreshData();
                    if (category != null)
                    {
                        this.dataGridCategories.SelectedItem = category;
                    }
                }
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGridCategories.SelectedItem != null && this.Tracker !=  null)
            {
                Birko.TimeTracker.Entities.Category category = (this.dataGridCategories.SelectedItem as Birko.TimeTracker.Entities.Category);
                if (category != null)
                {
                    IEnumerable<Birko.TimeTracker.Entities.Task> tasks = this.Tracker.Categories.GetTasks(category);
                    if (tasks.Count() > 0)
                    {
                        Birko.TimeTracker.Entities.Category none = this.Tracker.Categories.GetByName("None");
                        foreach (Birko.TimeTracker.Entities.Task task in tasks)
                        {
                            task.CategoryID = none.ID;
                            this.Tracker.Tasks.EditTask(task);
                        }
                    }
                    this.Tracker.Categories.DeleteCategory(category);
                    this.RefreshData();
                }
            }
        }

        private void dataGridCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                Birko.TimeTracker.Entities.Category category = (e.AddedItems[0] as Birko.TimeTracker.Entities.Category);
                this.textBoxCategory.Text = category.Name;
                this.textBoxGroup.Text = category.Group;
            }

        }
    }
}
