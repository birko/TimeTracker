using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birko.TimeTracker.DbContext
{
    public class TaskManager : EntityManagement.TaskManager
    {
        private TimeTrackerDbContext context = null;

        public TaskManager()
        {
            // TODO: Complete member initialization;
        }

        protected virtual TimeTrackerDbContext GetContext()
        {
            if (this.context == null)
            {
                this.context = new TimeTrackerDbContext();
            }
            return this.context;
        }

        public override Entities.Task NewTask()
        {
            Entities.Task task = this.GetContext().Tasks.Create();
            task.ID = Guid.NewGuid();
            return task; 
        }

        public override Entities.Task CreateTask(Entities.Task task)
        {
            Entities.Task newTask = null;
            using (TimeTrackerDbContext context = this.GetContext())
            {
                newTask = context.Tasks.Add(task);
                context.SaveChanges();
            }
            this.context = null;
            return this.GetTask(newTask.ID);
        }

        public override Entities.Task UpdateTask(Entities.Task task)
        {
            Entities.Task newTask = null;
            using (TimeTrackerDbContext context = this.GetContext())
            {
                newTask = context.Tasks.FirstOrDefault(t => t.ID == task.ID);
                if (newTask != null)
                {
                    newTask.CategoryID = task.CategoryID;
                    newTask.End = task.End;
                    newTask.Name = task.Name;
                    newTask.Description = task.Description;
                    newTask.Start = task.Start;
                    context.SaveChanges();
                }
            }
            this.context = null;
            return this.GetTask(newTask.ID);
        }

        public override Entities.Task TagTask(Entities.Task task, IEnumerable<Entities.Tag> tags)
        {
            Entities.Task newTask = null;
            using (TimeTrackerDbContext context = this.GetContext())
            {
                newTask = context.Tasks.FirstOrDefault(t => t.ID == task.ID);
                if (newTask != null)
                {
                    foreach (Entities.Tag tag in tags)
                    {
                        if (newTask.Tags.FirstOrDefault(t => t.ID == tag.ID) == null)
                        {
                            Entities.Tag addTag = context.Tags.FirstOrDefault(t => t.ID == tag.ID);
                            if(addTag != null)
                            {
                                newTask.Tags.Add(addTag);
                            }
                        }
                    }
                    foreach (Entities.Tag tag in newTask.Tags)
                    {
                        if (tags.FirstOrDefault(t => t.ID == tag.ID) == null)
                        {
                            Entities.Tag removeTag = context.Tags.FirstOrDefault(t => t.ID == tag.ID);
                            if (removeTag != null)
                            {
                                newTask.Tags.Remove(removeTag);
                            }
                        }
                    }
                    context.SaveChanges();
                }
            }
            this.context = null;
            return this.GetTask(newTask.ID);
        }

        public override Entities.Task DeleteTask(Entities.Task task)
        {
            Entities.Task newTask = null;
            using (TimeTrackerDbContext context = this.GetContext())
            {
                newTask = context.Tasks.FirstOrDefault(t => t.ID == task.ID);
                if (newTask != null)
                {
                    context.Tasks.Remove(task);
                    //tags ?
                    //Categories ?

                    context.SaveChanges();
                }
            }
            this.context = null;
            return newTask;
        }

        public override IEnumerable<Entities.Task> GetTasks()
        {
            return this.GetContext().Tasks.Include("Category").Include("Tags");
        }

        public override void Dispose()
        {
            if (this.context != null)
            {
                try
                {
                    this.context.Dispose();
                }
                catch (System.ObjectDisposedException)
                {
                }
                this.context = null;
            }
        }
    }
}
