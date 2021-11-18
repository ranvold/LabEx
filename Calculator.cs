﻿using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabEx
{
    class Calculator
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
