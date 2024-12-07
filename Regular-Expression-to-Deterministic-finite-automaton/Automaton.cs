using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dfa
{
    internal enum Operation
    {
        None,
        Alternation,
        Concatenation,
        Stelation
    }

    internal abstract class Automaton
    {
        public HashSet<State>   States { get; set; } = new HashSet<State>();
        public HashSet<char>    Alphabet { get; set; } = new HashSet<char> { 'a', 'b', 'c' };
        public State            StartState { get; set; } = new State();
        public HashSet<State>   FinalStates { get; set; } = new HashSet<State>();

        public override string ToString() {
            // Convert collections to strings for easier representation
            var statesString = string.Join(", ", States);
            var alphabetString = string.Join(", ", Alphabet);
            var finalStatesString = string.Join(", ", FinalStates);

            return $"{this.GetType().Name}:\n" + // Dynamically includes the class name
                   $"States: {{ {statesString} }}\n" +
                   $"Alphabet: {{ {alphabetString} }}\n" +
                   $"StartState: {StartState}\n" +
                   $"FinalStates: {{ {finalStatesString} }}\n";
        }
    }
}
