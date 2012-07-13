using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birko.TimeTracker
{
    public class Tasks
    {
        private EntityManagement.EntityManager EntityManager { get; set; }

        public Tasks(EntityManagement.EntityManager entityManager)
        {
            this.EntityManager = entityManager;
        }

        public Entities.Task NewTask()
        {
            Entities.Task task = null;
            using (EntityManagement.TaskManager manager = this.EntityManager.GetTaskManager())
            {
                task = manager.NewTask();
            }
            return task;
        }

        public IEnumerable<Entities.Task> GetTasks()
        {
            return this.GetTasks(null);
        }

        public IEnumerable<Entities.Task> GetTasks(System.Linq.Expressions.Expression<Func<Entities.Task,bool>> predicate)
        {
            List<Entities.Task> tasks = new List<Entities.Task>();
            using (EntityManagement.TaskManager manager = this.EntityManager.GetTaskManager())
            {
                IEnumerable<Entities.Task> loadedTasks = manager.GetTasks();
                if (predicate != null)
                {
                    loadedTasks = loadedTasks.AsQueryable().Where(predicate);
                }
                foreach (Entities.Task task in loadedTasks)
                {
                    tasks.Add(task);
                }

            }
            return tasks;
        }

        internal Entities.Task SaveTask(Entities.Task task)
        {
            Entities.Task newTask = null;
            using (EntityManagement.TaskManager manager = this.EntityManager.GetTaskManager())
            {
                Entities.Task testTask = manager.GetTask(task.ID);
                if (testTask == null)
                {
                    newTask = manager.CreateTask(task);
                    manager.TagTask(newTask, task.Tags);
                }
                else
                {
                    newTask = manager.UpdateTask(task);
                    manager.TagTask(newTask, task.Tags);
                }
            }
            return newTask;
        }

        internal void Tag(Entities.Task task, IEnumerable<Entities.Tag> tags)
        {
            using (EntityManagement.TaskManager manager = this.EntityManager.GetTaskManager())
            {
                manager.TagTask(task, tags);
            }
        }
    }
}
