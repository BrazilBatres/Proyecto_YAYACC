using System;
using System.Collections.Generic;
using System.Linq;
namespace YAYACC
{
    public class Grammar
    {
        public List<Variable> Variables { get; set; }
        public void Print()
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
                        Console.Write(Variables[i].Rules[j][h] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("-------------------------------------------");
            }
        }

        public void AddVariable(Variable _var)
        {
            bool _inserted = false;
            foreach (var item in Variables)
            {
                if (item.Name == _var.Name)
                {
                    foreach (var item2 in _var.Rules)
                    {
                        bool AlreadyInRule = false;
                        foreach (var item3 in item.Rules)
                        {
                            if (Enumerable.SequenceEqual(item2, item3))
                            {
                                AlreadyInRule = true;
                            }
                        }
                        if (!AlreadyInRule)
                        {
                            item.Rules.Add(item2);
                        }
                    }
                    _inserted = true;
                    break;
                }
            }
            if (!_inserted)
            {
                Variables.Add(_var);
            }
        }     
    }
}