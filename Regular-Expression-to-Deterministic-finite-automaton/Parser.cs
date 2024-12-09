using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace dfa {
    internal class Parser {
        public string PolishForm { get; private set; }
        public string Expression;

        public Parser(string expression) {
            PolishForm = CreatePolishPostfix(AddConcatenations(expression));
            Expression = expression;
        }

        static public bool isOperand(char ch) {
            return Char.IsLetter(ch);
        }

        static public bool isOperator(char ch) {
            switch (ch) {
                case '|':
                    return true;
                case '.':
                    return true;
                case '*':
                    return true;
                default:
                    return false;
            }
        }

        static public uint getArity(char ch) {
            switch (ch) {
                case '|':
                    return 2;
                case '.':
                    return 2;
                case '*':
                    return 1;
                default: 
                    throw new NotImplementedException();
            }
        }

        static public bool isLeftPharanthesis(char ch) {
            if (ch == '(') {
                return true;
            }

            return false;
        }

        static public bool isRightPharanthesis(char ch) {
            if (ch == ')') {
                return true;
            }

            return false;
        }


        private string AddConcatenations(string expr) {
            string result = new string("");

            for (int i = 0; i < expr.Length - 1; i++) {
                result += expr[i];

                if (InsertConcatenation(expr[i], expr[i + 1])) {
                    result += '.';
                }
            }

            return result + expr.Last();
        }

        private bool InsertConcatenation(char lhs, char rhs) {
            if (isOperand(lhs) && isOperand(rhs)) {
                return true;
            }

            if (isOperand(lhs) && isLeftPharanthesis(rhs)) {
                return true;
            }

            if (lhs == '*' && isOperand(rhs)) {
                return true;
            }

            if (isRightPharanthesis(lhs) && isRightPharanthesis(rhs)) {
                return true;
            }

            if (isRightPharanthesis(lhs) && isOperand(rhs)) {
                return true;
            }

            if (lhs == '*' && isLeftPharanthesis(rhs)) {
                return true;
            }

            return false;
        }

        private string CreatePolishPostfix(string expression) {
            Stack<char> stack = new Stack<char>();
            string polish = new string("");

            foreach (char ch in expression) {
                if (isOperand(ch)) {
                    polish += ch;
                }
                else if (isLeftPharanthesis(ch)) {
                    stack.Push(ch);
                }
                else if (isRightPharanthesis(ch)) {
                    while (stack.Peek() != '(') {
                        polish += stack.Pop();
                    }
                    stack.Pop();
                }
                else {
                    if (stack.Count() == 0) {
                        stack.Push(ch);
                        continue;
                    }

                    if (getPriority(ch) < getPriority(stack.Peek())) {
                        while (stack.Count() != 0 && stack.Peek() != '(') {
                            polish += stack.Pop();
                        }
                        stack.Push(ch);
                    }
                    else if (getPriority(ch) == getPriority(stack.Peek())) {
                        polish += ch;
                    }
                    else {
                        stack.Push(ch);
                    }
                }
            }

            while (stack.Count() != 0) {
                polish += stack.Pop();
            }

            return polish;
        }

        private int getPriority(char ch) {
            switch (ch) {
                case '|':
                    return 1;
                case '.':
                    return 2;
                case '*':
                    return 3;
                default:
                    return 0;
            }
        }

        public static bool RegexCheck(string expressionFile)
        {
            try
            {
                StreamReader f = new StreamReader(expressionFile);
                string? expression;
                expression = f.ReadLine();

                if(expression == null) 
                    return false;
                
                if (string.IsNullOrWhiteSpace(expression))
                    return false;

                expression.Replace(".", "");

                Regex regex = new Regex(expression);
                try
                {
                    new Regex(expression);
                    return true;
                }
                catch (ArgumentException)
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
