using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dfa {
    internal class Regex {
        private DFA dfa;

        public Regex(string regular_expr) {
            DFAFactory dfa_factory = new DFAFactory(regular_expr);

            dfa = dfa_factory.dfa;
            Console.WriteLine(dfa);
        }

        

        public bool CheckWord(string regular_expr) {
            State curr_state = dfa.StartState;

            foreach (char ch in regular_expr) {
                if (dfa.Transitions.ContainsKey((curr_state, ch))) {
                    curr_state = dfa.Transitions[(curr_state, ch)];
                }
                else {
                    return false;
                }
            }

            return dfa.FinalStates.Contains(curr_state);
        }
    }
}
