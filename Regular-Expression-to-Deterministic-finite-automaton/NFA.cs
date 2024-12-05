using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dfa
{
    internal class NFA : Automaton
    {
        public Dictionary<(State, char?), HashSet<State>> Transitions { get; set; }

        public NFA(
            HashSet<State> states,
            State? startState,
            HashSet<State> finalStates,
            Dictionary<(State, char?), HashSet<State>> transitions,
            HashSet<char> alphabet = null)
        {
            alphabet = alphabet ?? new HashSet<char> { 'a', 'b', 'c' };

            States = new HashSet<State>(states);
            Alphabet = new HashSet<char>(alphabet);
            StartState = startState;
            FinalStates = new HashSet<State>(finalStates);
            Transitions = new Dictionary<(State, char?), HashSet<State>>(transitions);
        }

        public NFA(char ch, uint count)
        {
            State start_state = new State($"q{count}");                
            State final_state = new State($"q{count + 1}");

            States = new HashSet<State> { start_state, final_state };
            StartState = start_state;
            FinalStates = new HashSet<State> { final_state };
            Transitions = new Dictionary<(State, char?), HashSet<State>> 
            {
                 {(start_state, ch), new HashSet<State> { final_state }}
            };
        }

        static public NFA GetAlternationNFA(NFA lhs, NFA rhs, uint count)
        {
            HashSet<State> res_states = new HashSet<State>(lhs.States.Concat(rhs.States)); //mutlimea starilor la noul automaton

            State res_start_state = new State($"q{count}");                 //start la noul automaton
            State res_final_state = new State($"q{count + 1}");             //final la noul automaton

            res_states.Add(res_start_state);                                
            res_states.Add(res_final_state);

            Dictionary<(State, char?), HashSet<State>> res_transitions =
                new Dictionary<(State, char?), HashSet<State>>(lhs.Transitions.Concat(rhs.Transitions));

            res_transitions.Add((res_start_state, null), new HashSet<State>());
            res_transitions[(res_start_state, null)].Add(lhs.StartState);
            res_transitions[(res_start_state, null)].Add(rhs.StartState);

            res_transitions.Add((lhs.FinalStates.First(), null), new HashSet<State>());
            res_transitions.Add((rhs.FinalStates.First(), null), new HashSet<State>());

            res_transitions[(lhs.FinalStates.First(), null)].Add(res_final_state);
            res_transitions[(rhs.FinalStates.First(), null)].Add(res_final_state);

            return new NFA(
                    res_states,
                    res_start_state,
                    new HashSet<State>{res_final_state },
                    res_transitions
                );
        }
    }
}

