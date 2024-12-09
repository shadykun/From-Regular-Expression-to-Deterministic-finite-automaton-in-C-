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
            string path = "input.txt";
            string content;
            if (File.Exists(path)) {
                content = File.ReadAllText(path);
                Console.WriteLine(content);
            }
            else {
                throw new Exception("File not found");
            }

            Regex regex = new Regex(content);
            Console.WriteLine($"Expresia regulata: {content}");

            while (true) {
                Console.WriteLine("Introdu test");
                string line = Console.ReadLine();
                Console.WriteLine($"regex match: {regex.regexMatch(line)}\n");
            }
        }

        
    }
}