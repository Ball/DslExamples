using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TaskPaperDomain;
using TaskPaperDomain.Domain;

namespace DelimiterDirected
{
    public class DelimiterParser : IInterpret
    {
        public string Name
        {
            get { return "Delimiter Parser"; }
        }

        public IEnumerable<Project> Interpret(string dslText)
        {
            var reader = new StringReader(dslText);
            Project project = null;
            foreach (var line in reader.ReadLines().Select( s => s.Replace("\n", "")))
            {
                if(line.EndsWith(":"))
                {
                    if(project != null)
                    {
                        yield return project;
                    }
                    project = new Project(line);
                }
                else if(project != null && Regex.Match(line, @"^-\s*").Success)
                {
                    project.AddTask(new TaskItem(line));
                }
                else if(project != null)
                {
                    if (project.Tasks.Count == 0)
                        project.AddNote(line);
                    else
                        project.Tasks.Last().AddNote(line);
                }
            }
            yield return project;
        }
    }
}