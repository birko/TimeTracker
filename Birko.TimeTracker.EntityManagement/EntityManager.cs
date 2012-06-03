using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birko.TimeTracker.EntityManagement
{
    public abstract class EntityManager
    {
        public abstract CategoryManager GetCategoryManager();
        public abstract TagManager GetTagManager();
        public abstract TaskManager GetTaskManager();
    }
}
