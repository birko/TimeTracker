using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Birko.TimeTracker.EntityManagement
{
    public abstract class CategoryManager : IDisposable
    {
        public virtual Entities.Category NewCategory()
        {
            return new Entities.Category() { ID = Guid.NewGuid(), };
        }

        public abstract Entities.Category CreateCategory(Entities.Category category);

        public abstract Entities.Category UpdateCategory(Entities.Category category);

        public abstract Entities.Category DeleteCategory(Entities.Category category);

        public virtual IEnumerable<Entities.Category> GetCategories()
        {
            return this.GetCategories(null);
        }

        public abstract IEnumerable<Entities.Category> GetCategories(Func<Entities.Category, bool> predicate);

        public virtual Entities.Category GetCategory(Guid id)
        {
            return this.GetCategories().FirstOrDefault(c => c.ID == id);
        }

        public virtual Entities.Category GetCategory(string name)
        {
            return this.GetCategories().FirstOrDefault(c => c.Name == name);
        }

        public virtual IEnumerable<Entities.Category> GetGroupCategories(string group)
        {
            return this.GetCategories().Where(c => c.Group == group);
        }

        public virtual IEnumerable<string> GetGroups()
        {
            return this.GetCategories().Select(c => c.Group);
        }


        public abstract void Dispose();
    }
}
