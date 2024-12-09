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
        public State() { Name = "";  }
        public State(string name) => Name = name;

        public State(int label) => Name = $"q{label}";
        public override bool Equals(object? obj) => obj is State state && Name == state.Name;
        public override int GetHashCode() => Name.GetHashCode();
        public static State operator+(State lhs, State rhs)
        {
            return new State(lhs.Name + rhs.Name);
        }
        public static bool operator==(State lhs, State rhs)
        {
            return lhs.Name == rhs.Name;
        }
        public static bool operator !=(State lhs, State rhs) {
            return lhs.Name != rhs.Name;
        }
        public override string ToString() => Name;
    }
}
