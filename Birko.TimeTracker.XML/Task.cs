using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Birko.TimeTracker.XML
{
    class Task : Entities.Task
    {
        internal IList<Guid> TagIDs { get; set; }

        public static explicit operator Task(XElement element)
        {
            return new Task()
            {
                ID = (Guid)element.Attribute("ID"),
                Name = (string)element.Element("Name"),
                CategoryID = (Guid)element.Element("Category"),
                Description = (string)element.Element("Description"),
                Start = (DateTime)element.Element("Start"),
                End = (DateTime)element.Element("End"),
                TagIDs = new List<Guid>((IEnumerable<Guid>)element.Element("Tags"))
            };
        }

        public XElement ToXElement()
        {
            XElement el = new XElement("Tag",
                new XAttribute("ID", this.ID),
                new XElement("Name", this.Name),
                new XElement("Category", this.CategoryID),
                new XElement("Description", this.Description),
                new XElement("Start", this.Start),
                new XElement("End", this.End),
                new XElement("Tags", this.Tags.Select(t=>t.ID))
            );
            return el;
        }
    }
}
