using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dfa { 
    internal class NFAFactory {

        public NFA nfa { get; private set; }
        public NFAFactory(string regular_expr) {
            nfa = Build(regular_expr);
        }
        private NFA Build(string regular_expr) {
            Parser parser = new Parser(regular_expr);

            Stack<NFA> stack = new Stack<NFA>();

            int counter = 0;
            foreach (char ch in parser.PolishForm) {
                if (Parser.isOperand(ch)) {
                    stack.Push(new NFA(ch, counter));
                    counter += 2;
                    continue;
                }
                
                if (Parser.getArity(ch) == 1) {
                    NFA automate = ExecuteOperation(stack.Pop(), ch, ref counter);
                    stack.Push(automate);
                }
                else if(Parser.getArity(ch) == 2) {
                    NFA rhs = stack.Pop();
                    NFA lhs = stack.Pop();

                    NFA automate = ExecuteOperation(lhs, rhs, ch, ref counter);
                    stack.Push(automate);
                }
            }

            NFA nfa = stack.Pop();
            nfa.Alphabet = setAlphabet(regular_expr);
            
            return nfa;
        }

        private HashSet<char> setAlphabet(string regular_expr) {
            return new HashSet<char>(
                regular_expr.Where(char.IsLetter)
                );
        }

        private NFA ExecuteOperation(NFA lhs, char op, ref int counter) {
            switch (op) {
                case '*':
                    return NFA.Stellation(lhs, ref counter);
                default:
                    throw new NotImplementedException();
            }
        }

        private NFA ExecuteOperation(NFA lhs, NFA rhs, char op, ref int counter) {
            switch (op) {
                case '|':
                    return NFA.Alternate(lhs, rhs, ref counter);
                case '.':
                    return NFA.Concatenate(lhs, rhs);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
