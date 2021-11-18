using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabEx
{
    class LabExVisitor : LabExBaseVisitor<double>
    {

        public override double VisitCompileUnit(LabExParser.CompileUnitContext context)
        {
            return Visit(context.expression());
        }

        public override double VisitNumberExpr(LabExParser.NumberExprContext context)
        {
            var result = double.Parse(context.GetText());
            Debug.WriteLine(result);

            return result;
        }

        public override double VisitNmaxNminExpr(LabExParser.NmaxNminExprContext context)
        {
            var expressions = context.expression();
            double max_min = Visit(context.expression(0));
            if (context.operatorToken.Type == LabExLexer.NMAX)
            {
                foreach (var expr in expressions)
                {
                    if (Visit(expr) >= max_min)
                    {
                        max_min = Visit(expr);
                    }
                }
            }
            else if (context.operatorToken.Type == LabExLexer.NMIN)
            {
                foreach (var expr in expressions)
                {
                    if (Visit(expr) <= max_min)
                    {
                        max_min = Visit(expr);
                    }
                }
            }
            return max_min;
        }

        public override double VisitIncDecExpr([NotNull] LabExParser.IncDecExprContext context)
        {
            var number = WalkLeft(context);
            if (context.operatorToken.Type == LabExLexer.INC)
            {
                Debug.WriteLine("inc( {0} )", number);
                return number + 1;
            }
            else
            {
                Debug.WriteLine("dec( {0} )", number);
                return number - 1;
            }
        }
        //IdentifierExpr
        public override double VisitIdentifierExpr(LabExParser.IdentifierExprContext context)
        {
            var result = context.GetText();
            //видобути значення змінної з таблиці
            if (Table.Database.TryGetValue(result.ToString(), out Cell cell))
            {
                cell.DependentCells.Add(Table.CurrCell());
                Table.CurrCell().CellDepends.Add(cell);
                return cell.CellValue;
            }
            else
            {
                return 0.0;
            }
        }

        public override double VisitParenthesizedExpr(LabExParser.ParenthesizedExprContext context)
        {
            return Visit(context.expression());
        }

        public override double VisitExponentialExpr(LabExParser.ExponentialExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            Debug.WriteLine("{0} ^ {1}", left, right);
            return System.Math.Pow(left, right);
        }

        public override double VisitAdditiveExpr(LabExParser.AdditiveExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            if (context.operatorToken.Type == LabExLexer.ADD)
            {
                Debug.WriteLine("{0} + {1}", left, right);
                return left + right;
            }
            else //LabExLexer.SUBTRACT
            {
                Debug.WriteLine("{0} - {1}", left, right);
                return left - right;
            }
        }

        public override double VisitMultiplicativeExpr(LabExParser.MultiplicativeExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            if (context.operatorToken.Type == LabExLexer.MULTIPLY)
            {
                Debug.WriteLine("{0} * {1}", left, right);
                return left * right;
            }
            else //LabExLexer.DIVIDE
            {
                Debug.WriteLine("{0} / {1}", left, right);
                return left / right;
            }
        }

        private double WalkLeft(LabExParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<LabExParser.ExpressionContext>(0));
        }

        private double WalkRight(LabExParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<LabExParser.ExpressionContext>(1));
        }
    }
}

