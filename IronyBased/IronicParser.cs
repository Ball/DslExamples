using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using TaskPaperDomain;
using TaskPaperDomain.Domain;

namespace IronyBased
{
    public class IronicParser : IInterpret
    {
        public string Name
        {
            get { return "Ironic Parser"; }
        }

        public IEnumerable<Project> Interpret(string dslText)
        {
            List<Project> projects = new List<Project>();
            var grammar = new ProjectsGrammar();
            var interpriter = new ScriptInterpreter(grammar);
            interpriter.Evaluate(dslText);
            if (interpriter.Status != InterpreterStatus.Ready)
            {
                throw new InvalidOperationException("Your syntax errored!?");
            }

            var program = interpriter.ParsedScript.Root.AstNode as StatementListNode;
            foreach (var node in program.ChildNodes)
            {
                if (node.Term.Name == "Project")
                {
                    var projectName = (LiteralValueNode) node.ChildNodes[0];
                    var project = new Project(projectName.Value.ToString().Trim());
                    projects.Add(project);
                    var notes = (StatementListNode) node.ChildNodes[1];
                    foreach(var note in notes.ChildNodes)
                    {
                        project.AddNote(((LiteralValueNode) note).Value.ToString().Trim());
                    }

                    var tasks = (StatementListNode)node.ChildNodes[2];
                    foreach (var child in tasks.ChildNodes)
                    {
                        var taskName = (LiteralValueNode)child.ChildNodes[0];
                        var task = new TaskItem(taskName.Value.ToString());
                        projects.Last().AddTask(task);
                        var taskNotes = (StatementListNode)child.ChildNodes[1];
                        foreach (var note in taskNotes.ChildNodes)
                        {
                            task.AddNote(((LiteralValueNode)note).Value.ToString().Trim());
                        }
                    }
                }
            }
            return projects;
        }
    }
}
