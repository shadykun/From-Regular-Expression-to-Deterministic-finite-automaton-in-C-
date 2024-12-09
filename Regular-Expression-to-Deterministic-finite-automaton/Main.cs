using dfa;
using System;
using System.ComponentModel;
using System.IO;

namespace dfa
{
    class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine("Works, don't ask");
            Console.ReadKey();
            Console.WriteLine("Fine... I'll show. Where do you want to get regex from? keyb or file?0 or 1?");
            int check = Convert.ToInt32(Console.ReadLine());
            if (check == 0)
            {
                Console.WriteLine("give that regex: ");
                string? expression = Console.ReadLine();
                if (!Parser.RegexCheck(expression))
                {
                    Console.WriteLine("invalid regex");
                    return;
                }
                while (expression == null)
                {
                    Console.WriteLine("Nice try pal, give something or i'll give this message until you actually type something\nExpression: ");
                    expression = Console.ReadLine();
                }
                DFAFactory factory = new DFAFactory(expression);
                Console.WriteLine(factory.dfa);
            }
            else
            {
                Console.WriteLine("give the file name where i can get the regex from: ");
                string? expressionLocation = Console.ReadLine();
                string? expression = Parser.RegexCheckFromFile(expressionLocation);
                if (expression == null)
                {
                    Console.WriteLine("invalid regex");
                    return;
                }
                else
                {
                    if (!Parser.RegexCheck(expression))
                    {
                        Console.WriteLine("invalid regex");
                        return;
                    }
                    while (expression == null)
                    {
                        Console.WriteLine("Nice try pal, now you have to type one or i'll give this message until you actually type something\nExpression: ");
                        expression = Console.ReadLine();
                    }
                    DFAFactory factory = new DFAFactory(expression);
                    Console.WriteLine(Parser.VerifyAutomaton(factory.dfa));
                    Console.WriteLine(factory.dfa);
                }
                //Console.WriteLine("i'm too lazy rn");
            }
        }
    }
}