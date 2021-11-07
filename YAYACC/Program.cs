using System;
namespace YAYACC
{
    class Program
    {
        static void Main(string[] args)
        {            
            Parser parser = new Parser();
            try
            {
                parser.Parse(args[0]);
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