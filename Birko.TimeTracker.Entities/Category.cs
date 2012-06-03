using System;
using System.Collections.Generic;

namespace Birko.TimeTracker.Entities
{
    public class Category
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
