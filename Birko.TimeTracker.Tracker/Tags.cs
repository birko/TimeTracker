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

        public IEnumerable<Entities.Tag> GetTags()
        {
            return this.GetTags(null);
        }

        public IEnumerable<Entities.Tag> GetTags(System.Linq.Expressions.Expression<Func<Entities.Tag, bool>> predicate)
        {
            List<Entities.Tag> tags = new List<Entities.Tag>();
            using (EntityManagement.TagManager manager = this.EntityManager.GetTagManager())
            {
                IEnumerable<Entities.Tag> loadedTasks = manager.GetTags(predicate);
                foreach (Entities.Tag tag in loadedTasks)
                {
                    tags.Add(tag);
                }

            }
            return tags;
        }

        public Entities.Tag SaveTag(Entities.Tag tag)
        {
            Entities.Tag result = null;
            using (EntityManagement.TagManager manager = this.EntityManager.GetTagManager())
            {
                Entities.Tag testtag = manager.GetTag(tag.ID);
                if (testtag == null)
                {
                    manager.UpdateTag(tag);
                }
                else
                {
                    if (tag.ID == null || tag.ID == Guid.Empty)
                    {
                        tag.ID = Guid.NewGuid();
                    }
                    manager.CreateTag(tag);
                }
            }
            result = this.GetByName(tag.Name);
            return result;
        }

        public Entities.Tag DeleteTag(Entities.Tag tag)
        {
            Entities.Tag result = null;
            using (EntityManagement.TagManager manager = this.EntityManager.GetTagManager())
            {
                result = manager.DeleteTag(tag);
            }
            return result;
        }

        public IEnumerable<Entities.Task> GetTasks(Entities.Tag tag)
        {
            IEnumerable<Entities.Task> result = new List<Entities.Task>();
            using (EntityManagement.TagManager manager = this.EntityManager.GetTagManager())
            {
                result = manager.GetTag(tag.ID).Tasks.ToList();
            }
            return result;
        }
    }
}

