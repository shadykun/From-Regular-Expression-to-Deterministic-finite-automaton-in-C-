using dfa;
using System;

namespace dfa
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Parser parser = new Parser("aba(aa|bb)*c(ab)*");

            // Console.WriteLine(parser.PolishForm);

            //NFA lhs = new NFA('a', 0);

            //NFA rhs = new NFA('b', 2);

            //NFA alternated = NFA.Alternate(lhs, rhs, 4);

            //NFA miau = new NFA('c', 6);

            NFAFactory factory = new NFAFactory("aba(aa|bb)*c(ab)*");

            Console.WriteLine(factory.nfa);
        }
    }
}