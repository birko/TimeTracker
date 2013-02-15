using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Birko.TimeTracker.XML
{
    class TaskManager : EntityManagement.TaskManager
    {
        public System.IO.Stream Stream { get; set; }
        public System.IO.Stream TagStream { get; set; }

        public override Entities.Task CreateTask(Entities.Task task)
        {
            Task newTask = new Task()
            {
                ID = task.ID,
                Name = task.Name,
                Description = task.Description,
                Start = task.Start,
                End= task.End,
                CategoryID = task.CategoryID,
                Category = task.Category,
            };
            XDocument doc = XDocument.Load(this.Stream);
            doc.Element("Task").Elements("Task").Last().AddAfterSelf(newTask.ToXElement());
            doc.Save(this.Stream);
            return newTask;
        }

        public override Entities.Task UpdateTask(Entities.Task task)
        {
            XDocument doc = XDocument.Load(this.Stream);
            XElement el = doc.Element("Task").Elements("Task").Where(x => x.Attribute("ID").Value == task.ID.ToString()).SingleOrDefault();
            if (el != null)
            {
                el.SetElementValue("Name", task.Name);
                el.SetElementValue("Category", task.CategoryID);
                el.SetElementValue("Description", task.Description);
                el.SetElementValue("Start", task.Start);
                el.SetElementValue("End", task.End);
                doc.Save(this.Stream);
            }
            ((Task)el).Category = task.Category;
            return (Task)el;
        }

        public override Entities.Task DeleteTask(Entities.Task task)
        {
            XDocument doc = XDocument.Load(this.Stream);
            XElement el = doc.Element("Task").Elements("Task").Where(x => x.Attribute("ID").Value == task.ID.ToString()).SingleOrDefault();
            if (el != null)
            {
                el.Remove();
                doc.Save(this.Stream);
            }
            ((Task)el).Category = task.Category;
            return (Task)el;
        }

        public override IEnumerable<Entities.Task> GetTasks(Func<Entities.Task, bool> predicate)
        {
            if (this.Stream != null)
            {
                XDocument doc = XDocument.Load(this.Stream);
                IEnumerable<Entities.Task> result = (IEnumerable<Entities.Task>)doc.Descendants("Tasks").Select(x => (Task)x);
                if (predicate != null)
                {
                    result = result.Where(predicate);
                }
                return result;
            }
            return new List<Entities.Task>();
        }

        public override Entities.Task TagTask(Entities.Task task, IEnumerable<Entities.Tag> tags)
        {
            List<Entities.Tag> result = new List<Entities.Tag>();
            if (this.Stream != null)
            {
                using (TagManager tm = new TagManager())
                {
                    tm.Stream = this.TagStream;
                    XDocument doc = XDocument.Load(this.Stream);
                    Task el = (Task)doc.Element("Task").Elements("Task").Where(x => x.Attribute("ID").Value == task.ID.ToString()).SingleOrDefault();
                    foreach (Entities.Tag tag in tags)
                    {
                        if (!el.TagIDs.Contains(tag.ID))
                        {
                            el.TagIDs.Add(tag.ID);
                        }
                    }
                     List<Guid> removeTags = new List<Guid>();
                    foreach (Guid tagid in el.TagIDs)
                    {
                        if (tags.FirstOrDefault(t => t.ID == tagid) == null)
                        {
                            removeTags.Add(tagid);
                        }
                    }
                    if (removeTags.Count > 0)
                    {
                        foreach (Guid tag in removeTags)
                        {
                           el.TagIDs.Remove(tag);
                        }
                    }
                    XElement el2 = doc.Element("Task").Elements("Task").Where(x => x.Attribute("ID").Value == task.ID.ToString()).SingleOrDefault();
                    if (el2 != null)
                    {
                        el2.SetElementValue("Tags", el.TagIDs);
                        doc.Save(this.Stream);
                    }
                    return (Task)el2;
                }
            }
            return task;
        }

        public override IEnumerable<Entities.Tag> GetTaskTags(Entities.Task task)
        {
            List<Entities.Tag> result = new List<Entities.Tag>();
            if (this.Stream != null)
            {
                using (TagManager tm = new TagManager())
                {
                    tm.Stream = this.TagStream;
                    IEnumerable<Entities.Tag> tags = tm.GetTags();
                    XDocument doc = XDocument.Load(this.Stream);
                    Task el = (Task)doc.Element("Task").Elements("Task").Where(x => x.Attribute("ID").Value == task.ID.ToString()).SingleOrDefault();
                    foreach (Guid id in el.TagIDs)
                    {
                        result.AddRange(tm.GetTags(t=>t.ID == id));
                    }
                }
            }
            return result;
        }

        public override void Dispose()
        {
        }
    }
}
