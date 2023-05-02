using System;
using System.Collections.Generic;

namespace Calculatrice_Ezo
{
    public class FormulaExpression
    {
        private List<Func<decimal, decimal>> Terms = new List<Func<decimal, decimal>>();

        public string Evaluate()
        {
            var value = 0m;

            foreach (var term in Terms)
            {
                value = term(value);
            }

            return value.ToString().Replace(',', '.');

            /*
                **Tentative d'ordre d'opération**

                var orderedOperations = new List<Func<decimal[], decimal>>();
                var value = 0m;

                while(Operations.Count > 0)
                {
                    var test = Operations.IndexOf("+");
                    var currentOperation = OperationsDict[Operations[test]];

                    Operations.RemoveAt(test);

                    orderedOperations.Add(currentOperation(Tokens[test], Tokens[test + 1]));
                
                }
             */
        }

        public void AddTerm(Func<decimal, decimal> term)
        {
            Terms.Add(term);
        }

    }
}