using dfa;
using System;
using System.ComponentModel;

namespace dfa
{
    class Program
    {

        public static void Main(string[] args)
        {


            // Console.WriteLine(parser.PolishForm);

            //NFA lhs = new NFA('a', 0);

            //NFA rhs = new NFA('b', 2);

            //NFA alternated = NFA.Alternate(lhs, rhs, 4);




            //NFA miau = new NFA('c', 6);
            //Console.WriteLine(factory.nfa);

            string expr = "(a|b)c*d*";

            Regex regex = new Regex(expr);

            while (true) {
                string line = Console.ReadLine();
                Console.WriteLine(regex.regexMatch(line));
            }

            //Parser parser = new Parser(expr);

            //Console.WriteLine(parser.PolishForm);

            //DFAFactory factory_dfa = new DFAFactory(expr);

            //Console.WriteLine(factory_dfa.dfa);
        }
    }
}