using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birko.TimeTracker.DbContext
{
    public class TagManager : EntityManagement.TagManager
    {
        private TimeTrackerDbContext context = null;

        public TagManager()
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

        public override Entities.Tag NewTag()
        {
            Entities.Tag tag = this.GetContext().Tags.Create();
            tag.ID = Guid.NewGuid();
            return tag;
        }  

        public override Entities.Tag CreateTag(Entities.Tag tag)
        {
            Entities.Tag newTag = null;
            using (TimeTrackerDbContext context = this.GetContext())
            {
                newTag = context.Tags.Add(tag);
                context.SaveChanges();
            }
            this.context = null;
            return newTag;
        }

        public override Entities.Tag UpdateTag(Entities.Tag tag)
        {
            Entities.Tag newTag = null;
            using (TimeTrackerDbContext context = this.GetContext())
            {
                newTag = context.Tags.FirstOrDefault(t => t.ID == tag.ID);
                if (newTag != null)
                {
                    newTag.Name = tag.Name;
                    //tasks?

                    context.SaveChanges();
                }
            }
            this.context = null;
            return newTag;
        }

        public override Entities.Tag DeleteTag(Entities.Tag tag)
        {
            Entities.Tag newTag = null;
            using (TimeTrackerDbContext context = this.GetContext())
            {
                newTag = context.Tags.FirstOrDefault(t => t.ID == tag.ID);
                if (newTag != null)
                {
                    context.Tags.Remove(newTag);
                    //tasks ?

                    context.SaveChanges();
                }
            }
            this.context = null;
            return newTag;
        }

        public override IEnumerable<Entities.Tag> GetTags()
        {
            return this.GetContext().Tags;
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
