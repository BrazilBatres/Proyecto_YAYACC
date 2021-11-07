using System;
namespace YAYACC
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Argumento: "+args[0]);

            //PRUEBA PARSER
            Parser parser = new Parser();
            try
            {
                parser.Parse("Gramática_2.y");
                Console.WriteLine("Expresión OK");
                parser.grammar.Imprimir_Gramatica();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //PRUEBA SCANNER
            //Scanner scanner = new Scanner(args[0]);
            //Token newtoken;
            //try
            //{
            //    while (!scanner.IsEmpty())
            //    {
            //        newtoken = scanner.GetToken();
            //        string toprint = newtoken.Tag.ToString();
            //        if (newtoken.Value[0] != (char)0 )
            //        {
            //            toprint += " valor -> " + newtoken.Value;
            //        }
            //        Console.WriteLine(toprint);
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }
    }
}
