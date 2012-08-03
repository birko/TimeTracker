using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Birko.TimeTracker.Entities
{
    public class Category
    {
        public virtual Guid ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Group { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
