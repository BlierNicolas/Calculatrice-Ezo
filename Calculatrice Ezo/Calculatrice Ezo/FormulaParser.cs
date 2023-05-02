using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calculatrice_Ezo
{
    public static class FormulaParser
    {
        const char OPEN_PARENTHESIS = '(';
        const char CLOSE_PARENTHESIS = ')';
        const string OPERATORS = "+-*/^";
        const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";
        const string NUMBERS = "0123456789,.";
        private static readonly Regex Space = new Regex("\\s", RegexOptions.Compiled);

        public static FormulaExpression Parse(string formula)
        {
            var index = 0;
            var currentNumber = "";
            char? currentOperator = null;
            var currentFunction = "";
            var expression = new FormulaExpression();

            while (index < formula.Length)
            {
                var character = formula[index];

                if (OPERATORS.Contains(character))
                {
                    // Flush current function & operation
                    FlushFunction(ref currentFunction, ref currentNumber, expression);
                    FlushOperation(ref currentOperator, ref currentNumber, expression);

                    // Set operation
                    if (currentNumber.Length == 0 && character == '-')
                    {
                        AccumulateNumber(character, ref currentNumber);
                    }
                    else
                    {
                        currentOperator = character;
                    }
                }
                else if (ALPHABET.Contains(character))
                {
                    // Accumulate named formula
                    AccumulateFunction(character, ref currentFunction);
                }
                else if (NUMBERS.Contains(character))
                {
                    // Accumulate number
                    AccumulateNumber(character, ref currentNumber);
                }
                else if (character == OPEN_PARENTHESIS)
                {
                    // Increase depth
                    // Not implemented
                }
                else if (character == CLOSE_PARENTHESIS)
                {
                    // Decrease depth
                    // Not implemented
                }
                else if (Space.IsMatch(character.ToString()))
                {
                    // Flush current function & operation
                    FlushFunction(ref currentFunction, ref currentNumber, expression);
                    FlushOperation(ref currentOperator, ref currentNumber, expression);

                    // Ignore spaces
                }
                else
                {
                    throw new ArithmeticException("Erreur caractère invalide");
                }

                index++;
            }

            // Flush current function & operation
            FlushFunction(ref currentFunction, ref currentNumber, expression);
            FlushOperation(ref currentOperator, ref currentNumber, expression);

            return expression;
        }

        private static void FlushOperation(ref char? currentOperator, ref string currentNumber, FormulaExpression expression)
        {
            if (decimal.TryParse(currentNumber, out var value))
            {
                if (currentOperator == null) currentOperator = '+';

                switch (currentOperator)
                {
                    case '+':
                        expression.AddTerm(new Func<decimal, decimal>(x => x + value));
                        break;
                    case '-':
                        expression.AddTerm(new Func<decimal, decimal>(x => x - value));
                        break;
                    case '*':
                        expression.AddTerm(new Func<decimal, decimal>(x => x * value));
                        break;
                    case '/':
                        if (value == 0) throw new ArithmeticException ("Erreur, division par zero");
                        expression.AddTerm(new Func<decimal, decimal>(x => x / value));
                        break;
                    case '^':
                        expression.AddTerm(new Func<decimal, decimal>(x => (decimal)Math.Pow((double)x, (double)value)));
                        break;
                    default:
                        return;
                }

                currentOperator = null;
                currentNumber = "";
            }
        }

        private static void FlushFunction(ref string currentFunction, ref string currentNumber, FormulaExpression expression)
        {
            if (decimal.TryParse(currentNumber, out var value))
            {
                switch (currentFunction)
                {
                    case "sqrt":
                        expression.AddTerm(new Func<decimal, decimal>(x => x + (decimal)Math.Sqrt((double)value)));
                        break;
                    default:
                        return;
                }

                currentFunction = "";
                currentNumber = "";
            }
        }

        public static void AccumulateNumber(char value, ref string number)
        {
            number += value == '.' ? ',' : value;
        }

        private static void AccumulateFunction(char character, ref string currentFunction)
        {
            currentFunction += character;
        }
    }
}
