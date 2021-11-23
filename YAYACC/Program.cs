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
                parser.Parse("Gramatica_1.y");
                Console.WriteLine("Expresión OK");
                parser.grammar.Print();

                parser.grammar.BuildParser();
                parser.grammar.ParserGrammar("float\'");
                Console.WriteLine("ACCEPT");
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