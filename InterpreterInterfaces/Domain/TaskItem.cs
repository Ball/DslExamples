using System;
using System.Text.RegularExpressions;

namespace TaskPaperDomain.Domain
{
    public class TaskItem
    {
        public string TaskName { get; private set; }
        public bool IsDone { get; private set; }
        public string Notes { get; private set; }

        public TaskItem(string line)
        {
            TaskName = Regex.Replace(line, @"^-\s*", "");
            IsDone = TaskName.Contains("@done");
            Notes = string.Empty;
        }

        public void AddNote(string line)
        {
            if (String.IsNullOrEmpty(Notes))
            {
                Notes = line;
            }
            else
            {
                Notes = Notes + "\n" + line;
            }
        }
    }
}