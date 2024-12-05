using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dfa
{
    public class State
    {
        public string Name { get; set; }
        public State(string name) => Name = name;

        public override bool Equals(object? obj) => obj is State state && Name == state.Name;
        public override int GetHashCode() => Name.GetHashCode();
        public override string ToString() => Name;
    }
}
