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
            if (Start.HasValue && End.HasValue)
            {
                return End.Value - Start.Value;
            }
            else if (Start.HasValue)
            {
                return DateTime.UtcNow - Start.Value;
            }
            else 
            {
                return new TimeSpan(0);
            }
        }

        public DateTime? LocalStart
        {
            get 
            {
                if (this.Start.HasValue)
                {
                    return this.Start.Value.ToLocalTime();
                }
                return null;
            }
            set 
            {
                if (value.HasValue)
                {
                    this.Start = value.Value.ToUniversalTime();
                }
            }
        }

        public DateTime? LocalEnd
        {
            get
            {
                if (this.End.HasValue)
                {
                    return this.End.Value.ToLocalTime();
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.End = value.Value.ToUniversalTime();
                }
            }
        }
    }
}
