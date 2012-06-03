using System;
using System.Collections.Generic;

namespace Birko.TimeTracker.Entities
{
    public class Tag
    {
        public Guid ID { get; set; }
        public string Name { get; set;}
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
