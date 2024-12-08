using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dfa
{
    internal class DFA : Automaton {
        public Dictionary<(State, char?), State> Transitions { get; set; }

        public DFA(NFA nfa)
        {
            States = new HashSet<State>();
            StartState = new State();
            FinalStates = new HashSet<State>();
            Transitions = new Dictionary<(State, char?), State>();

            if (nfa.Alphabet != null)
            {
                Alphabet = nfa.Alphabet;
            }
        }

        public override string ToString()
        {
            var transitionsString = string.Join("\n", Transitions.Select(t =>
            $"({t.Key.Item1}, {t.Key.Item2?.ToString()}) -> {{ {string.Join(", ", t.Value)}}}"));

            return base.ToString() +
                "Delta: {\n" +
                transitionsString +
                "\n}";
        }

    }
}
