using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TaskPaperDomain.Domain;

namespace ParserTests.cs
{
    public abstract class BaseParserTest
    {
        protected TaskPaperDomain.IInterpret _parser;

        [Test]
        public void ShouldFindAProject()
        {
            var projects = Build(@"Project:");
           Assert.That(projects.First().Name, Is.EqualTo("Project"));
        }
        
        [Test]
        public void ShouldFindTwoProjects()
        {
            var projects = Build(
@"Project:
Another:").ToArray();
            Assert.That(projects.Length, Is.EqualTo(2));
            Assert.That(projects[0].Name, Is.EqualTo("Project"));
            Assert.That(projects[1].Name, Is.EqualTo("Another"));
        }

        [Test]
        public void ShouldAddANote()
        {
            var projects = Build(
                @"Project:
Some text that needs to be
long enough to wrap");
            Assert.That(projects.First().Notes, Is.EqualTo("Some text that needs to be\nlong enough to wrap"));
        }

        [Test]
        public void ShouldContainTasks()
        {
            var project = Build(@"Project:
some note
- task one
- task two").First();

            Assert.That(project.Tasks.Count, Is.EqualTo(2));
            Assert.That(project.Tasks[0].TaskName, Is.EqualTo("task one"));
            Assert.That(project.Tasks[1].TaskName, Is.EqualTo("task two"));
        }

        [Test]
        public void ShouldAllowForTaskNotes()
        {
            var project = Build(@"Project:
- task one
a note for task one").First();
            Assert.That(project.Tasks[0].Notes, Is.EqualTo("a note for task one"));
        }
        
        [Test]
        public void ShouldHandleExampleText()
        {
            var project = Build(@"Project:
Some notes
- A Task
- Another Task
Write a DSL:
It's a thing
- A Task").ToArray();
            Assert.That(project[0].Name, Is.EqualTo("Project"));
            Assert.That(project[0].Notes, Is.EqualTo("Some notes"));
            Assert.That(project[0].Tasks[0].TaskName, Is.EqualTo("A Task"));
            Assert.That(project[0].Tasks[1].TaskName, Is.EqualTo("Another Task"));
            Assert.That(project[1].Name, Is.EqualTo("Write a DSL"));
            Assert.That(project[1].Notes, Is.EqualTo("It's a thing"));
            Assert.That(project[1].Tasks[0].TaskName, Is.EqualTo("A Task"));
        }

        protected IEnumerable<Project> Build(string projectFile)
        {
            return _parser.Interpret(projectFile);
        }
    }
}
