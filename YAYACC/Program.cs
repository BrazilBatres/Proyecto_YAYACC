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
                Console.WriteLine("Grammar OK!");
                parser.grammar.Print();
                parser.grammar.BuildParser();

                bool _continue = true;
                while (_continue)
                {
                    Console.Write("Insert a word to analize: ");
                    string _word = Console.ReadLine();

                    try
                    {
                        parser.grammar.ParserGrammar(_word);
                        Console.WriteLine("ACCEPT");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    Console.WriteLine("If you want to continue, please press C key");
                    string option = Console.ReadLine();
                    if (option.ToUpper() != "C")
                    {
                        _continue = false;
                    }
                }
                Console.WriteLine("Press any key to finish...");
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