using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birko.TimeTracker
{
    public class Tracker
    {
        public TimeTracker.Categories Categories { get; protected set; }
        public TimeTracker.Tags Tags { get; protected set; }
        public TimeTracker.Tasks Tasks { get; protected set; }
        public Birko.TimeTracker.Entities.Task ActiveTask { get; protected set; }

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
                this.EndTask(this.ActiveTask);
            }
            this.StartTask(task);
        }

        private void StartTask(Entities.Task task)
        {
            task.Start = DateTime.UtcNow;
            Entities.Task newTask = this.Tasks.SaveTask(task);
            this.ActiveTask = newTask;
        }

        public void EndTask(Entities.Task task)
        {
            task.End = DateTime.UtcNow;
            this.Tasks.SaveTask(task);
        }

        public void TagTask(Entities.Task task, IEnumerable<Entities.Tag> tags)
        {
            this.Tasks.Tag(task, tags);
        }
    }
}
