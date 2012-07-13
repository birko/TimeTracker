using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birko.TimeTracker.EntityManagement
{
    public abstract class TaskManager : IDisposable
    {
        public virtual Entities.Task NewTask()
        {
            return new Entities.Task() { ID = Guid.NewGuid(), };
        }

        public abstract Entities.Task CreateTask(Entities.Task task);

        public abstract Entities.Task UpdateTask(Entities.Task task);

        public abstract Entities.Task DeleteTask(Entities.Task task);

        public abstract IEnumerable<Entities.Task> GetTasks();

        public virtual Entities.Task GetTask(Guid id)
        {
            return this.GetTasks().FirstOrDefault(t => t.ID == id);
        }

        public virtual IEnumerable<Entities.Task> GetCategoryTasks(Guid categoryId)
        {
            return this.GetTasks().Where(t => t.CategoryID == categoryId);
        }

        public virtual IEnumerable<Entities.Task> GetCategoryTasks(Entities.Category category)
        {
            return this.GetCategoryTasks(category.ID);
        }

        public abstract Entities.Task TagTask(Entities.Task task, IEnumerable<Entities.Tag> tags);

        public abstract IEnumerable<Entities.Tag> GetTaskTags(Entities.Task task);

        public abstract void Dispose();
    }
}
