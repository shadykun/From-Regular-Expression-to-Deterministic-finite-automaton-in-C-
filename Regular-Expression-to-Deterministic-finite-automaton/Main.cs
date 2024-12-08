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

            NFAFactory factory = new NFAFactory("(aa|b)*bb");//(aa|b)*bb

            Console.WriteLine(factory.nfa);

            DFAFactory dfa = new DFAFactory(factory.nfa);

            Console.WriteLine(dfa.dfa);
            //DFAFactory factory1 = new DFAFactory(factory.nfa);
            //Console.WriteLine(factory1.dfa);
        }
    }
}