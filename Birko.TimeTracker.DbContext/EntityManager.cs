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
        internal string ConnnectionString { get; set; }
        internal string Provider { get; set; }

        public EntityManager(string nameOrConnectionString, string provider)
        {
            // TODO: Complete member initialization
            this.ConnnectionString = nameOrConnectionString;
            this.Provider = provider;
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
