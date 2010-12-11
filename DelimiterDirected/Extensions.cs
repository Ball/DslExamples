using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using TaskPaperDomain.Domain;

namespace DelimiterDirected
{
    public static class Extensions
    {
        public static void ClearAndAddRange(this ObservableCollection<Project> target, IEnumerable<Project> thing)
        {
            target.Clear();
            foreach (var project in thing)
            {
                target.Add(project);
            }
        }

        public static IEnumerable<string> ReadLines(this TextReader self)
        {
            while(self.Peek() != -1)
            {
                yield return self.ReadLine();
            }
        }
    }
}