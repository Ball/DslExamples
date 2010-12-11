using System;
using Irony.Ast;
using Irony.Parsing;

namespace IronyBased
{
    [Language("Taskpaper", "1.0", "Task Expression System")]
    public class ProjectsGrammar : Grammar
    {
        public ProjectsGrammar()
        {
            // Todo 
            // Fix Note to work actually right, just take the rest of the text

            // Define Terminals
            var taskName = new TaskTerminal("TaskName");
            var projectName = new ProjectTerminal("ProjectName");
            var noteText = new NoteTerminal("NoteText");
            
            // Define Non-Terminals
            var notes = new NonTerminal("Notes", typeof(StatementListNode));
            var project = new NonTerminal("Project", typeof (StatementListNode));
            var task = new NonTerminal("Task", typeof (StatementListNode));
            var tasks = new NonTerminal("Tasks", typeof (StatementListNode));

            var program = new NonTerminal("ProgramLine", typeof(StatementListNode));


            // Define Rules
            notes.Rule = MakeStarRule(notes, noteText);
            task.Rule = taskName + notes;
            tasks.Rule = MakeStarRule(tasks, task);
            project.Rule = projectName +notes + tasks;
            program.Rule = MakeStarRule(program, project);
            
            Root = program;
            LanguageFlags = LanguageFlags.CreateAst 
                            | LanguageFlags.CanRunSample;
        }
    }
}