using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birko.TimeTracker.DbContext
{
    public class EntityManager: EntityManagement.EntityManager
    {

        public EntityManager()
        {

            using (TimeTrackerDbContext contex = new TimeTrackerDbContext())
            {
                Console.WriteLine(contex.Database.Connection);
                if (!contex.Database.Exists())
                {
                    contex.Database.CreateIfNotExists();
                }
            }
        }

        public override EntityManagement.CategoryManager GetCategoryManager()
        {
            return new CategoryManager();
        }

        public override EntityManagement.TagManager GetTagManager()
        {
            return new TagManager();
        }

        public override EntityManagement.TaskManager GetTaskManager()
        {
            return new TaskManager();
        }
    }
}
