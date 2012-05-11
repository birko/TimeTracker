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
        [Key]
        [DatabaseGeneratedAttribute(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        [ForeignKey("Category")]
        public Guid CategoryID { get; set; }
        public Category Category { get; set; }
        public ICollection<Tag> Tags { get; set; }
        [NotMappedAttribute]
        public TimeSpan Duration 
        {
            get 
            {
                return this.GetDuration();
            }
        }

        private TimeSpan GetDuration()
        {
            if (Start != null && End != null)
            {
                return End - Start;
            }
            else if (Start != null && End == null)
            {
                return DateTime.Now - Start;
            }
            else 
            {
                return new TimeSpan(0);
            }
        }
    }
}
