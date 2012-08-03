using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Birko.TimeTracker.XML
{
    class Tag : Entities.Tag
    {
        public static explicit operator Tag(XElement element)
        {
            return new Tag()
            {
                ID = (Guid)element.Attribute("ID"),
                Name = (string)element.Element("Name"),
            };
        }

        public XElement ToXElement()
        {
            XElement el = new XElement("Tag",
                new XAttribute("ID", this.ID),
                new XElement("Name", this.Name)
            );
            return el;
        }
    }
}
