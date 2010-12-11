using System.Text.RegularExpressions;
using Irony.Parsing;

namespace IronyBased
{
    public class TaskTerminal : Terminal
    {
        private readonly Regex _expression;

        public TaskTerminal(string name):base(name)
        {
            SetFlag(TermFlags.IsLiteral);
            _expression = new Regex("- ([^\r\n\v]+)[\r\n\v]*");
        }
        public override Token TryMatch(ParsingContext context, ISourceStream source)
        {
            var match = _expression.Match(source.Text, source.PreviewPosition);
            if(!(match.Success && (match.Index == source.PreviewPosition)))
            {
                return null;
            }
            source.PreviewPosition += match.Length;
            return source.CreateToken(OutputTerminal, match.Groups[1] );
        }
    }
}