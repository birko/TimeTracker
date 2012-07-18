using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Birko.TimeTracker.Tracker
{
    public class Categories
    {
        private EntityManagement.EntityManager EntityManager { get; set; }

        public Categories(EntityManagement.EntityManager entityManager)
        {
            this.EntityManager = entityManager;
        }

        public Birko.TimeTracker.Entities.Category GetByName(string name)
        {
            Entities.Category category = null;
            using (EntityManagement.CategoryManager manager = this.EntityManager.GetCategoryManager())
            {
                category = manager.GetCategory(name);
                if (category == null)
                {
                    category = manager.NewCategory();
                    category.Name = name;
                    category = manager.CreateCategory(category);
                }
            }
            return category;
        }

        public IEnumerable<Entities.Category> GetCategories()
        {
            return this.GetCategories(null);
        }

        public IEnumerable<Entities.Category> GetCategories(System.Linq.Expressions.Expression<Func<Entities.Category, bool>> predicate)
        {
            List<Entities.Category> list = new List<Entities.Category>();
            using (EntityManagement.CategoryManager manager = this.EntityManager.GetCategoryManager())
            {
                IEnumerable<Entities.Category> loaded = manager.GetCategories(predicate);
                foreach (Entities.Category category in loaded)
                {
                    list.Add(category);
                }

            }
            return list;
        }

        public IEnumerable<Entities.Task> GetTasks(Entities.Category category)
        {
            List<Entities.Task> list = new List<Entities.Task>();
            using (EntityManagement.CategoryManager manager = this.EntityManager.GetCategoryManager())
            {
                Entities.Category loaded = manager.GetCategory(category.ID);
                list = loaded.Tasks.ToList();
            }
            return list;
        }

        public Entities.Category DeleteCategory(Entities.Category category)
        {
            Entities.Category result = null;
            using (EntityManagement.CategoryManager manager = this.EntityManager.GetCategoryManager())
            {
                result = manager.DeleteCategory(category);
            }
            return result;
        }

        public Entities.Category EditCategory(Entities.Category category)
        {
            Entities.Category result = null;
            using (EntityManagement.CategoryManager manager = this.EntityManager.GetCategoryManager())
            {
                result = manager.UpdateCategory(category);
            }
            return result;
        }
    }
}
