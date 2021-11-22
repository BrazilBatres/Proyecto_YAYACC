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
            //try
            //{
                parser.Parse("Gramática_2.y");
                Console.WriteLine("Expresión OK");
                parser.grammar.Print();

                //List<char> toReturn = new List<char>();
                //parser.grammar.FirstVariable("Var_list", toReturn);

                parser.grammar.BuildParser();
                                
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    Console.ReadLine();
            //}
        }
    }
}