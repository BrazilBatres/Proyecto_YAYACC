using System;
using System.Collections.Generic;
namespace YAYACC
{
    public class Grammar
    {
        public List<Variable> Variables { get; set; }

        public void Imprimir_Gramatica()
        {
            Console.WriteLine("-------------------------------------------");
            for (int i = 0; i < Variables.Count; i++)
            {                
                Console.WriteLine("Variable " + Variables[i].Name + ":");
                for (int j = 0; j < Variables[i].Rules.Count; j++)
                {
                    Console.Write("Regla " + (j + 1) + ": ");
                    for (int h = 0; h < Variables[i].Rules[j].Count; h++)
                    {
                        Console.Write(Variables[i].Rules[j].Pop() + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("-------------------------------------------");
            }
        }
    }
}