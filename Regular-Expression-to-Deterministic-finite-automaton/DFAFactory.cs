using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Tracing;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
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
        public DFA dfa {  get; private set; }
        private NFA nfa { get; set; }

        public DFAFactory(NFA nfa) {
            this.nfa = nfa;
            dfa = Build();
        }

        private DFA Build(){

            DFA rez = new DFA(nfa);
            Stack<State> states = new Stack<State>();
            Dictionary<State, HashSet<State>> newStates = new Dictionary<State, HashSet<State>>();
            Dictionary<HashSet<State>, State> newStatesRev = new Dictionary<HashSet<State>, State>();

            int counter = 0;
            states.Push(new State($"q{counter}"));
            counter++;
            HashSet<State> work_states = new HashSet<State>(); 
            foreach(State s in bfs(nfa.StartState))
                work_states.Add(s);
            rez.StartState = states.Peek();
            rez.States.Add(states.Peek());
            newStates.Add(rez.StartState, work_states);

            while (states.Count > 0)
            {
                State start_state = states.Pop();

                HashSet<State> check = new HashSet<State>(newStates[start_state]);
                work_states = new HashSet<State>();
                foreach (State s in check)
                    work_states.UnionWith(bfs(s));

                foreach (char c in nfa.Alphabet)
                {
                    foreach (State state in work_states)
                    {
                        HashSet<State> state_set;
                        if(nfa.Transitions.ContainsKey((state, c)))
                            state_set = new HashSet<State>(nfa.Transitions[(state, c)]);
                        else
                            state_set = new HashSet<State>();
                        if (state_set.Count > 0)
                        {
                            State end_state = new State("");
                            bool checker = true;
                            foreach (HashSet<State> s in newStates.Values)
                            {
                                if(s.SetEquals(state_set))
                                    checker = false;
                            }
                            if (checker){
                                end_state = new State($"q{counter}");
                                counter++;
                                states.Push(end_state);
                                newStates.Add(end_state, state_set);
                            }
                            else{
                                foreach(State s in newStates.Keys)
                                {
                                    if (newStates[s].SetEquals(state_set))
                                        end_state = s;
                                }
                            }
                            if (!rez.Transitions.TryAdd((start_state, c), end_state))
                                return rez;
                            rez.States.Add(end_state);
                            HashSet<State> possibleEnd = new HashSet<State>();
                            foreach (State s in state_set)
                                possibleEnd.UnionWith(bfs(s));
                            foreach (State s in possibleEnd)
                                if (nfa.FinalStates.Contains(s))
                                    rez.FinalStates.Add(end_state);
                        }
                    }
                }
            }
            return rez;
        }

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

            if (nfa.Transitions.ContainsKey((curr, null)))
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
