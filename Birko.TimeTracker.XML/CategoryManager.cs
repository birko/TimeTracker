using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Birko.TimeTracker.XML
{
    class CategoryManager : EntityManagement.CategoryManager
    {
        private string file = "Category.xml";
        protected string Path {get; set;}
        protected string FilePath
        {
            get
            {
                return string.Format("{0}\\{1}", this.Path, this.file);
            }
        }

        public CategoryManager(string path)
        {
            this.Path = path;
        }
        public override Entities.Category CreateCategory(Entities.Category category)
        {
            throw new NotImplementedException();
        }

        public override Entities.Category UpdateCategory(Entities.Category category)
        {
            throw new NotImplementedException();
        }

        public override Entities.Category DeleteCategory(Entities.Category category)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Entities.Category> GetCategories(System.Linq.Expressions.Expression<Func<Entities.Category, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
