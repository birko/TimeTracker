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
            IEnumerable<Entities.Task> tasks = null;
            using (EntityManagement.TaskManager manager = this.EntityManager.GetTaskManager())
            {
                tasks = manager.GetTasks();
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
                }
                else
                {
                    newTask = manager.UpdateTask(task);
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
