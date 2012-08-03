using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Birko.TimeTracker.XML
{
    class CategoryManager : EntityManagement.CategoryManager
    {
        public System.IO.Stream Stream { get; set; }

        public override Entities.Category CreateCategory(Entities.Category category)
        {
            Category newCategory = new Category()
            {
                ID = category.ID,
                Name = category.Name,
                Group = category.Group,
            };
            XDocument doc = XDocument.Load(this.Stream);
            doc.Element("Categories").Elements("Category").Last().AddAfterSelf(newCategory.ToXElement());
            doc.Save(this.Stream);
            return newCategory;
        }

        public override Entities.Category UpdateCategory(Entities.Category category)
        {
            XDocument doc = XDocument.Load(this.Stream);
            XElement el = doc.Element("Categories").Elements("Category").Where(x => x.Attribute("ID").Value == category.ID.ToString()).SingleOrDefault();
            if (el != null)
            {
                el.SetElementValue("Name", category.Name);
                el.SetElementValue("Group", category.Group);
                doc.Save(this.Stream); 
            }
            return (Category)el;
        }

        public override Entities.Category DeleteCategory(Entities.Category category)
        {
            XDocument doc = XDocument.Load(this.Stream);
            XElement el = doc.Element("Categories").Elements("Category").Where(x => x.Attribute("ID").Value == category.ID.ToString()).SingleOrDefault();
            if (el != null)
            {
                el.Remove();
                doc.Save(this.Stream);
            }
            return (Category)el;
        }

        public override IEnumerable<Entities.Category> GetCategories(Func<Entities.Category, bool> predicate)
        {
            if(this.Stream != null)
            {
                XDocument doc = XDocument.Load(this.Stream);
                IEnumerable<Entities.Category> result = (IEnumerable<Entities.Category>)doc.Descendants("Categories").Select(x => (Category)x);
                if (predicate != null)
                {
                    result = result.Where(predicate);
                }
                return result;
            }
            return new List<Entities.Category>();
        }

        public override void Dispose()
        {
        }

    }
}
