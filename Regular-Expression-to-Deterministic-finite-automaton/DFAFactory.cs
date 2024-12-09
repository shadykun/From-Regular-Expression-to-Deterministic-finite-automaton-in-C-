using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace dfa
{

    class HashSetComparer<T> : IEqualityComparer<HashSet<T>>
    {
        public bool Equals(HashSet<T> x, HashSet<T> y)
        {
            return x.SetEquals(y);
        }

        public int GetHashCode(HashSet<T> obj)
        {
            return obj.Aggregate(0, (hash, item) => hash ^ item.GetHashCode());
        }
    }


    internal class DFAFactory
    {
        private NFA nfa { get; set; }
        private int state_counter { get; set; }
        public DFA dfa {  get; private set; }

        public DFAFactory(string regular_expr) {
            state_counter = 0;

            NFAFactory nfa_factory =  new NFAFactory(regular_expr);
            this.nfa = nfa_factory.nfa;
            Console.WriteLine(nfa);

            dfa = new DFA(nfa);
            Build2();
        }

        private void Build2() {
            Dictionary<string, State> visited_states = new Dictionary<string, State>();
            HashSet<State> initial_nfa_states = bfs(nfa.StartState);

            State initial_dfa_state = new State(state_counter++);
            dfa.StartState = new State(initial_dfa_state.Name);
            dfa.States.Add(new State(initial_dfa_state.Name));

            Queue<(State, HashSet<State>)> queue = new Queue<(State, HashSet<State>)>();
            queue.Enqueue((initial_dfa_state, initial_nfa_states));

            while (queue.Count > 0) {
                var state_to_check = queue.Dequeue();

                var new_states = tryAlphabetTransitions(state_to_check, visited_states);
                
                foreach (var new_state in new_states) {
                    queue.Enqueue(new_state);
                }
            }

        }

        private List<(State, HashSet<State>)> tryAlphabetTransitions((State, HashSet<State>) state_to_check, Dictionary<string, State> visited) {
            List<(State, HashSet<State>)> new_states = new List<(State, HashSet<State>)>();

            foreach (char ch in nfa.Alphabet) {
                var new_nfa_states = seachForNewStates(state_to_check.Item2, ch);

                if (new_nfa_states == null) {
                    continue;
                }

                string new_states_str = string.Join("", new_nfa_states.Select(val => val.Name));

                if (visited.ContainsKey(new_states_str)) {
                    dfa.Transitions.Add((state_to_check.Item1, ch), visited[new_states_str]);
                }
                else {
                    State new_dfa_state = new State(state_counter++);

                    visited.Add(new_states_str, new_dfa_state);

                    dfa.States.Add(new_dfa_state);
                    dfa.Transitions.Add((state_to_check.Item1, ch), new_dfa_state);

                    new_states.Add((new_dfa_state, new_nfa_states));

                    if (new_nfa_states.Overlaps(nfa.FinalStates)) {
                        dfa.FinalStates.Add(new_dfa_state);
                    }
                }
            }

            return new_states;
        }

        private HashSet<State>? seachForNewStates (HashSet<State> states_to_check, char ch) {
            HashSet<State> new_states = new HashSet<State>();

            foreach (State state in states_to_check) {
                if (nfa.Transitions.ContainsKey((state, ch))) {
                    State start_state = nfa.Transitions[(state, ch)].First();

                    new_states.UnionWith(bfs(start_state));
                }
            }

            return (new_states.Count() > 0) ? new_states : null;
        }


        //private DFA Build() {

        //    DFA rez = new DFA(nfa);
        //    Queue<State> states = new Queue<State>();
        //    Dictionary<State, HashSet<State>> newStates = new Dictionary<State, HashSet<State>>();
        //    Dictionary<HashSet<State>, State> newStatesRev = new Dictionary<HashSet<State>, State>();

        //    int counter = 0;
        //    states.Enqueue(new State($"q{counter}"));
        //    counter++;
        //    HashSet<State> work_states = new HashSet<State>();
        //    foreach (State s in bfs(nfa.StartState))
        //        work_states.Add(s);
        //    rez.StartState = states.Peek();
        //    rez.States.Add(states.Peek());
        //    newStates.Add(rez.StartState, work_states);

        //    while (states.Count > 0) {
        //        State start_state = states.Dequeue();

        //        HashSet<State> check = new HashSet<State>(newStates[start_state]);
        //        work_states = new HashSet<State>();
        //        foreach (State s in check)
        //            work_states.UnionWith(bfs(s));

        //        foreach (char c in nfa.Alphabet) {
        //            foreach (State state in work_states) {
        //                HashSet<State> state_set;
        //                if (nfa.Transitions.ContainsKey((state, c)))
        //                    state_set = new HashSet<State>(nfa.Transitions[(state, c)]);
        //                else
        //                    state_set = new HashSet<State>();
        //                if (state_set.Count > 0) {
        //                    State end_state = new State("");
        //                    bool checker = true;
        //                    foreach (HashSet<State> s in newStates.Values) {
        //                        if (s.SetEquals(state_set))
        //                            checker = false;
        //                    }
        //                    if (checker) {
        //                        end_state = new State($"q{counter}");
        //                        counter++;
        //                        states.Enqueue(end_state);
        //                        newStates.Add(end_state, state_set);
        //                    }
        //                    else {
        //                        foreach (State s in newStates.Keys) {
        //                            if (newStates[s].SetEquals(state_set))
        //                                end_state = s;
        //                        }
        //                    }
        //                    if (!rez.Transitions.TryAdd((start_state, c), end_state))
        //                        return rez;
        //                    rez.States.Add(end_state);
        //                    HashSet<State> possibleEnd = new HashSet<State>();
        //                    foreach (State s in state_set)
        //                        possibleEnd.UnionWith(bfs(s));
        //                    foreach (State s in possibleEnd)
        //                        if (nfa.FinalStates.Contains(s))
        //                            rez.FinalStates.Add(end_state);
        //                }
        //            }
        //        }
        //    }
        //    //foreach (State s in newStates.Keys)
        //    //{
        //    //    Console.WriteLine(s);
        //    //    foreach (State q in newStates[s])
        //    //        Console.WriteLine("    " + q);
        //    //}
        //    return rez;
        //}

        public HashSet<State> bfs(State curr, HashSet<State> visited = null)
        {
            if (visited == null)
            {
                visited = new HashSet<State>();
            }

            if (visited.Contains(curr))
            {
                return new HashSet<State>();
            }

            HashSet<State> states = new HashSet<State> { curr };

            visited.Add(curr);

            if (nfa.Transitions.ContainsKey((curr, null)) == true)
            {
                foreach (State state in nfa.Transitions[(curr, null)])
                {
                    states.UnionWith(bfs(state, visited));
                }
            }

            return states;
        }
    }
}
