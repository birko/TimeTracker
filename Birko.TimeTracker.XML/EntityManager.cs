using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Birko.TimeTracker.XML
{
    class EntityManager: Birko.TimeTracker.EntityManagement.EntityManager
    {
        public override EntityManagement.CategoryManager GetCategoryManager()
        {
            throw new NotImplementedException();
        }

        public override EntityManagement.TagManager GetTagManager()
        {
            throw new NotImplementedException();
        }

        public override EntityManagement.TaskManager GetTaskManager()
        {
            throw new NotImplementedException();
        }
    }
}
