using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace YAYACC
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Argumento: "+args[0]);

            //PRUEBA PARSER
            Parser parser = new Parser();
            parser.Parse(args[0]);
            Console.WriteLine("Expresión OK");

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
