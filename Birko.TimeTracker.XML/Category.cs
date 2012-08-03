using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Birko.TimeTracker.XML
{
    class Category: Entities.Category
    {
        public static explicit operator Category(XElement element)
        {
            return new Category()
            {
                ID = (Guid)element.Attribute("ID"),
                Name = (string)element.Element("Name"),
                Group = (string)element.Element("Group"),
            };
        }

        public XElement ToXElement()
        {
            XElement el = new XElement("Category",
                new XAttribute("ID", this.ID),
                new XElement("Name", this.Name),
                new XElement("Group", this.Group)
            );
            return el;
        }
    }
}
