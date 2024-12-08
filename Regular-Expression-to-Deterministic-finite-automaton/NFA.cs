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
            State startState,
            HashSet<State> finalStates,
            Dictionary<(State, char?), HashSet<State>> transitions,
            HashSet<char>? alphabet = null)
        {
            States = states;
            StartState = startState;
            FinalStates = finalStates;
            Transitions = transitions;

            if (alphabet != null) {
                Alphabet = alphabet;
            }
        }

        public NFA(char ch, int count)
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

        static public NFA Alternate(NFA lhs, NFA rhs, ref int count)
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

            count += 2;

            return new NFA(
                    res_states,
                    res_start_state,
                    new HashSet<State>{ res_final_state },
                    res_transitions
                );
        }

        static public NFA Concatenate(NFA lhs, NFA rhs) {
            State res_start_state = new State(lhs.StartState.Name);
            State res_final_state = new State(rhs.FinalStates.First().Name);
            State concat_state = lhs.FinalStates.First() + rhs.StartState;

            HashSet<State> res_states = new HashSet<State>(lhs.States.Concat(rhs.States));
            res_states.Remove(lhs.FinalStates.First());
            res_states.Remove(rhs.StartState);
            res_states.Add(concat_state);

            Dictionary<(State, char?), HashSet<State>> res_transitions = new Dictionary<(State, char?), HashSet<State>>();

            foreach (var (key, to_states) in lhs.Transitions.Concat(rhs.Transitions)) {
                var (from_state, symbol) = key;

                var res_from_state = key.Item1;
                var res_to_states = to_states;

                if (res_from_state == rhs.StartState) {
                    res_from_state = new State(concat_state.Name);
                }

                if (res_to_states.Contains(lhs.FinalStates.First())) {
                    res_to_states.Remove(lhs.FinalStates.First());
                    res_to_states.Add(new State(concat_state.Name));
                }

                res_transitions.Add((res_from_state, symbol), new HashSet<State>(res_to_states));
            }

            return new NFA(
                    res_states,
                    res_start_state,
                    new HashSet<State> { res_final_state },
                    res_transitions
                );
        }

        static public NFA Stellation(NFA lhs, ref int count) {
            HashSet<State> res_states = new HashSet<State>(lhs.States); //mutlimea starilor la noul automaton

            State res_start_state = new State($"q{count}");                 //start la noul automaton
            State res_final_state = new State($"q{count + 1}");             //final la noul automaton

            res_states.Add(res_start_state);
            res_states.Add(res_final_state);

            Dictionary<(State, char?), HashSet<State>> res_transitions =
               new Dictionary<(State, char?), HashSet<State>>(lhs.Transitions);

            res_transitions.Add((res_start_state, null), new HashSet<State> { lhs.StartState, res_final_state });
            res_transitions.Add((lhs.FinalStates.First(), null), new HashSet<State> { lhs.StartState, res_final_state});

            count += 2;

            return new NFA(
                   res_states,
                   res_start_state,
                   new HashSet<State> { res_final_state },
                   res_transitions
               );
        }

        public override string ToString() {
            var transitionsString = string.Join("\n", Transitions.Select(t =>
                $"({t.Key.Item1}, {t.Key.Item2?.ToString() ?? "ε"}) -> {{ {string.Join(", ", t.Value)} }}"));

            return base.ToString() +
                "Delta: {\n" +
                transitionsString +
                "\n}";
        }
    }
}

