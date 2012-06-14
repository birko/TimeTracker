using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birko.TimeTracker.DbContext
{
    public class CategoryManager : EntityManagement.CategoryManager
    {
        private TimeTrackerDbContext context = null;

        public CategoryManager()
        {
            // TODO: Complete member initialization
        }

        protected virtual TimeTrackerDbContext GetContext()
        {
            if (this.context == null) 
            {
                this.context = new TimeTrackerDbContext();
            }
            return this.context;
        }

        public override Entities.Category NewCategory()
        {
            Entities.Category category = this.GetContext().Categories.Create();
            category.ID = Guid.NewGuid();
            return category;
        }  

        public override Entities.Category CreateCategory(Entities.Category category)
        {
            Entities.Category newCategory = null;
            using (TimeTrackerDbContext context = this.GetContext())
            {
                newCategory = context.Categories.Add(category);
                context.SaveChanges();
            }
            this.context = null;
            return newCategory;
        }


        public override Entities.Category UpdateCategory(Entities.Category category)
        {
            Entities.Category newCategory = null;
            using (TimeTrackerDbContext context = this.GetContext())
            {
                newCategory = context.Categories.FirstOrDefault(c => c.ID == category.ID);
                if (newCategory != null)
                {
                    newCategory.Group = category.Group;
                    newCategory.Name = category.Name;
                    //tasks?

                    context.SaveChanges();
                }
            }
            this.context = null;
            return newCategory;
        }

        public override Entities.Category DeleteCategory(Entities.Category category)
        {
            Entities.Category newCategory = null;
            using (TimeTrackerDbContext context = this.GetContext())
            {
                newCategory = context.Categories.FirstOrDefault(c => c.ID == category.ID);
                if (newCategory != null)
                {
                    newCategory = context.Categories.Remove(newCategory);
                    // tasks?

                    context.SaveChanges();
                }
            }
            this.context = null;
            return newCategory;
        }

        public override IEnumerable<Entities.Category> GetCategories()
        {
            return this.GetContext().Categories;
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
