﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birko.TimeTracker
{
    public delegate void TaskStarted(Entities.Task task);
    public delegate void TaskEnded(Entities.Task task);
    public delegate void TaskDeleted(Entities.Task task);

    public class Tracker
    {
        public TimeTracker.Categories Categories { get; protected set; }
        public TimeTracker.Tags Tags { get; protected set; }
        public TimeTracker.Tasks Tasks { get; protected set; }
        public Birko.TimeTracker.Entities.Task ActiveTask { get; protected set; }
        public event TaskStarted OnTaskStarted = null;
        public event TaskEnded OnTaskEnded = null;
        public event TaskDeleted OnTaskDeleted = null;

        protected EntityManagement.EntityManager EntityManager { get; set; }

        public Tracker(EntityManagement.EntityManager entityManager)
        {
            this.EntityManager = entityManager;
            this.Tags = new TimeTracker.Tags(this.EntityManager);
            this.Tasks = new TimeTracker.Tasks(this.EntityManager);
            this.Categories = new TimeTracker.Categories(this.EntityManager);
        }

        public void SwitchTask(Entities.Task task)
        {
            if (this.ActiveTask != null)
            {
                if (this.ActiveTask.ID != task.ID)
                {
                    this.EndTask(this.ActiveTask);
                }
            }
            this.StartTask(task);
        }

        private void StartTask(Entities.Task task)
        {
            if (task.Start == null)
            {
                task.Start = DateTime.UtcNow;
            }
            Entities.Task newTask = this.Tasks.SaveTask(task);
            this.ActiveTask = newTask;
            if(OnTaskStarted != null)
            {
                this.OnTaskStarted(this.ActiveTask);
            }
        }

        public void EndTask(Entities.Task task)
        {
            task.End = DateTime.UtcNow;
            this.Tasks.SaveTask(task);
            if (task.ID == this.ActiveTask.ID)
            {
                this.ActiveTask = null;
            }
            if (OnTaskEnded != null)
            {
                this.OnTaskEnded(task);
            }
        }

        public void TagTask(Entities.Task task, IEnumerable<Entities.Tag> tags)
        {
            this.Tasks.Tag(task, tags);
            if (OnTaskStarted != null)
            {
                this.OnTaskStarted(this.ActiveTask);
            }
        }

        public void RemoveTask(Entities.Task task)
        {
            if (this.ActiveTask!= null && this.ActiveTask.ID == task.ID)
            {
                this.EndTask(task);
            }
            this.Tasks.DeleteTask(task);
            if (this.OnTaskDeleted != null)
            {
                this.OnTaskDeleted(task);
            }
        }
    }
}
