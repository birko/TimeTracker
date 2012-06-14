using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birko.TimeTracker.Entities
{
    public class Task
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public Guid? CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        public TimeSpan Duration 
        {
            get 
            {
                return this.GetDuration();
            }
        }

        public string FullName
        {
            get
            {
                string cat = (this.Category != null) ? this.Category.Name : "";
                return (string.IsNullOrEmpty(cat)) ? this.Name : this.Name + "@" + cat;
            }
        }

        private TimeSpan GetDuration()
        {
            if (Start != null && End != null)
            {
                return End.Value - Start.Value;
            }
            else if (Start != null && End == null)
            {
                return DateTime.UtcNow - Start.Value;
            }
            else 
            {
                return new TimeSpan(0);
            }
        }
    }
}
