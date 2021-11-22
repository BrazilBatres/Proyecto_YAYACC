using System;
using System.Collections.Generic;

namespace YAYACC
{
    class Program
    {
        static void Main(string[] args)
        {            
            //¡TODO OK!
            Parser parser = new Parser();
            try
            {
                parser.Parse(args[0]);
                Console.WriteLine("Expresión OK");
                parser.grammar.Print();

                //List<char> toReturn = new List<char>();
                //parser.grammar.FirstVariable("_Declaration", toReturn);

                parser.grammar.BuildParser();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}