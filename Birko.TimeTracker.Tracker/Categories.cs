using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birko.TimeTracker
{
    public class Categories
    {
        private EntityManagement.EntityManager EntityManager { get; set;  }

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
    }
}
