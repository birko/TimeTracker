using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Birko.TimeTracker.Tracker
{
    public class Tags
    {
        private EntityManagement.EntityManager EntityManager { get; set; }

        public Tags(EntityManagement.EntityManager entityManager)
        {
            this.EntityManager = entityManager;
        }

        public Entities.Tag GetByName(string name)
        {
            Entities.Tag tag = null;
            using (EntityManagement.TagManager manager = this.EntityManager.GetTagManager())
            {
                tag = manager.GetTag(name);
                if (tag == null)
                {
                    tag = manager.NewTag();
                    tag.Name = name;
                    manager.CreateTag(tag);
                }
            }
            return tag;
        }
    }
}

