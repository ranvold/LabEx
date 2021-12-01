using Antlr4.Runtime;

namespace LabEx
{
    public class Calculator
    {
        public static double Evaluate(string expression)
        {
            var lexer = new LabExLexer(new AntlrInputStream(expression));
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new ThrowExceptionErrorListener());

            var tokens = new CommonTokenStream(lexer);
            var parser = new LabExParser(tokens);

            var tree = parser.compileUnit();

            var visitor = new LabExVisitor();

            return visitor.Visit(tree);
        }
    }
}
