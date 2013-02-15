using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Birko.TimeTracker.XML
{
    class TagManager : EntityManagement.TagManager
    {
        public System.IO.Stream Stream { get; set; }

        public override Entities.Tag CreateTag(Entities.Tag tag)
        {
            Tag newTag = new Tag()
            {
                ID = tag.ID,
                Name = tag.Name,
            };
            XDocument doc = XDocument.Load(this.Stream);
            doc.Element("Tags").Elements("Tag").Last().AddAfterSelf(newTag.ToXElement());
            doc.Save(this.Stream);
            return newTag;
        }

        public override Entities.Tag UpdateTag(Entities.Tag tag)
        {
            XDocument doc = XDocument.Load(this.Stream);
            XElement el = doc.Element("Tags").Elements("Tag").Where(x => x.Attribute("ID").Value == tag.ID.ToString()).SingleOrDefault();
            if (el != null)
            {
                el.SetElementValue("Name", tag.Name);
                doc.Save(this.Stream);
            }
            return (Tag)el;
        }

        public override Entities.Tag DeleteTag(Entities.Tag tag)
        {
            XDocument doc = XDocument.Load(this.Stream);
            XElement el = doc.Element("Tags").Elements("Tag").Where(x => x.Attribute("ID").Value == tag.ID.ToString()).SingleOrDefault();
            if (el != null)
            {
                el.Remove();
                doc.Save(this.Stream);
            }
            return (Tag)el;
        }

        public override IEnumerable<Entities.Tag> GetTags(Func<Entities.Tag, bool> predicate)
        {
            if (this.Stream != null)
            {
                XDocument doc = XDocument.Load(this.Stream);
                IEnumerable<Entities.Tag> result = (IEnumerable<Entities.Tag>)doc.Descendants("Tags").Select(x => (Tag)x);
                if (predicate != null)
                {
                    result = result.Where(predicate);
                }
                return result;
            }
            return new List<Entities.Tag>();
        }

        public override void Dispose()
        {
        }
    }
}
