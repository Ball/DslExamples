using System;
using System.Collections.ObjectModel;

namespace TaskPaperDomain.Domain
{
    public class Project
    {
        public string Notes { get; private set; }
        public string Name { get; private set; }
        public ObservableCollection<TaskItem> Tasks { get; private set; }

        public Project(string line)
        {
            Notes = string.Empty;
            Name = line.Replace(":", "");
            Tasks = new ObservableCollection<TaskItem>();
        }

        public void AddTask(TaskItem taskItem)
        {
            Tasks.Add(taskItem);
        }

        public void AddNote(string line)
        {
            if(String.IsNullOrEmpty(Notes))
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