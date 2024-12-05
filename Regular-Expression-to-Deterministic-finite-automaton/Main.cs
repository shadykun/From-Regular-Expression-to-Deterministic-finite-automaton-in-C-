using dfa;
using System;

namespace dfa
{
    class Program
    {
        public static void Main(string[] args)
        {
            Parser parser = new Parser("aba(aa|bb)*c(ab)*");

            Console.WriteLine(parser.PolishForm);
        }
    }
}