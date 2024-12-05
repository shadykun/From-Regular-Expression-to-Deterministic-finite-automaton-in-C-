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
        public HashSet<char>    Alphabet { get; set; } = new HashSet<char>();
        public State?           StartState { get; set; }
        public HashSet<State>   FinalStates { get; set; } = new HashSet<State>();
    }
}
