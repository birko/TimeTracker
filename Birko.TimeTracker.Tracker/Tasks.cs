using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Birko.TimeTracker.Tracker
{
    public class Tasks
    {
        private EntityManagement.EntityManager EntityManager { get; set; }

        public Tasks(EntityManagement.EntityManager entityManager)
        {
            this.EntityManager = entityManager;
        }

        public Birko.TimeTracker.Entities.Task NewTask()
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

        public IEnumerable<Entities.Task> GetTasks(System.Linq.Expressions.Expression<Func<Entities.Task, bool>> predicate)
        {
            List<Entities.Task> tasks = new List<Entities.Task>();
            using (EntityManagement.TaskManager manager = this.EntityManager.GetTaskManager())
            {
                IEnumerable<Entities.Task> loadedTasks = manager.GetTasks(predicate);
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
                }
                else
                {
                    newTask = manager.UpdateTask(task);
                }

                TimeSpan minTime = new TimeSpan(0, 1, 0);//TODO: settings
                if (newTask.End.HasValue)
                {
                    // update end for tasks that have ended after Task start and before Task end and started before Task start
                    IEnumerable<Entities.Task> changeTasks = manager.GetTasks().Where(
                        t => t.End.HasValue &&
                        t.Start <= newTask.Start &&
                        t.End >= newTask.Start &&
                        t.End <= newTask.End &&
                        t.ID != newTask.ID
                    ).ToList();
                    foreach (Entities.Task chtask in changeTasks)
                    {
                        chtask.End = newTask.Start;
                        if (chtask.Duration <= minTime)
                        {
                            manager.DeleteTask(chtask);
                        }
                        else
                        {
                            manager.UpdateTask(chtask);
                        }
                    }

                    // update Start for tasks that have ended after Task End and started after before Task start
                    changeTasks = manager.GetTasks().Where(
                        t => t.End.HasValue &&
                        t.Start >= newTask.Start &&
                        t.End >= newTask.End &&
                        t.Start <= newTask.End &&
                        t.ID != newTask.ID
                    ).ToList();
                    foreach (Entities.Task chtask in changeTasks)
                    {
                        chtask.Start = newTask.End;
                        if (chtask.Duration <= minTime)
                        {
                            manager.DeleteTask(chtask);
                        }
                        else
                        {
                            manager.UpdateTask(chtask);
                        }
                    }

                    // divide tasks that have started befor Task and Edned After Task
                    changeTasks = manager.GetTasks().Where(
                        t => t.End.HasValue &&
                        t.Start <= newTask.Start &&
                        t.End >= newTask.Start &&
                        t.End >= newTask.End &&
                        t.ID != newTask.ID
                    ).ToList();
                    foreach (Entities.Task chtask in changeTasks)
                    {
                        Birko.TimeTracker.Entities.Task newtask = this.NewTask();
                        newtask.Name = chtask.Name;
                        newtask.Start = newTask.End;
                        newtask.CategoryID = chtask.CategoryID;
                        newtask.End = chtask.End;

                        chtask.End = newTask.Start;
                        if (chtask.Duration <= minTime)
                        {
                            manager.DeleteTask(chtask);
                        }
                        else
                        {
                            manager.UpdateTask(chtask);
                        }

                        if (newtask.Duration >= minTime)
                        {
                            List<Birko.TimeTracker.Entities.Tag> tags = new List<Birko.TimeTracker.Entities.Tag>();
                            IEnumerable<Birko.TimeTracker.Entities.Tag> taskTags = this.GetTags(chtask);
                            foreach (Birko.TimeTracker.Entities.Tag tag in taskTags)
                            {
                                tags.Add(tag);
                            }
                            this.SaveTask(newtask);
                            this.Tag(newtask, tags);
                        }
                    }

                    //delete tasks that have started and ended between Task start and Task end
                    changeTasks = manager.GetTasks().Where(
                        t => t.End.HasValue &&
                        t.Start >= newTask.Start &&
                        t.End <= newTask.End &&
                        t.ID != newTask.ID
                    ).ToList();

                    foreach (Entities.Task chtask in changeTasks)
                    {
                        manager.DeleteTask(chtask);
                    }
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

        public IEnumerable<Entities.Tag> GetTags(Entities.Task task)
        {
            IEnumerable<Entities.Tag> result = new List<Entities.Tag>();
            using (EntityManagement.TaskManager manager = this.EntityManager.GetTaskManager())
            {
                result = manager.GetTaskTags(task);
            }
            return result;
        }

        internal Entities.Task DeleteTask(Entities.Task task)
        {
            Entities.Task result = null;
            using (EntityManagement.TaskManager manager = this.EntityManager.GetTaskManager())
            {
                result = manager.DeleteTask(task);
            }
            return result;
        }

        public Entities.Task GetFirstTask() 
        {
            Entities.Task result = null;
            using (EntityManagement.TaskManager manager = this.EntityManager.GetTaskManager())
            {
                result = manager.GetTasks().OrderBy(t => t.Start).FirstOrDefault();
            }
            return result;
        }

        public Entities.Task GetLastTask()
        {
            Entities.Task result = null;
            using (EntityManagement.TaskManager manager = this.EntityManager.GetTaskManager())
            {
                result = manager.GetTasks().OrderByDescending(t => t.Start).FirstOrDefault();
            }
            return result;
        }
    }
}
