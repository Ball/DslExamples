using System.Collections.Generic;
using TaskPaperDomain.Domain;

namespace TaskPaperDomain
{
    public interface IInterpret
    {
        string Name { get; }
        IEnumerable<Project> Interpret(string dslText);
    }
}