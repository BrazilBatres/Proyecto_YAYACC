using System;
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
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            
        }
    }
}