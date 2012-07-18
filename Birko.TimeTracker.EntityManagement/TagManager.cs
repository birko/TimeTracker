using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Birko.TimeTracker.EntityManagement
{
    public abstract class TagManager : IDisposable
    {
        public virtual Entities.Tag NewTag()
        {
            return new Entities.Tag() { ID = Guid.NewGuid(), };
        }

        public abstract Entities.Tag CreateTag(Entities.Tag tag);

        public abstract Entities.Tag UpdateTag(Entities.Tag tag);

        public abstract Entities.Tag DeleteTag(Entities.Tag tag);

        public virtual IEnumerable<Entities.Tag> GetTags()
        {
            return this.GetTags(null);
        }

        public abstract IEnumerable<Entities.Tag> GetTags(System.Linq.Expressions.Expression<Func<Entities.Tag, bool>> predicate);


        public virtual Entities.Tag GetTag(Guid id)
        {
            return this.GetTags().FirstOrDefault(c => c.ID == id);
        }

        public virtual Entities.Tag GetTag(string name)
        {
            return this.GetTags().FirstOrDefault(c => c.Name == name);
        }

        public abstract void Dispose();
    }
}
