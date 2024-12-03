using dfa;
using System;

namespace HelloWorld
{
    class HelloWorld
    {
        public static void Main(string[] args)
        {
            Operator op = new Operator("|");

            Console.WriteLine(op.Execute("miau", "hau"));
        }
    }
}