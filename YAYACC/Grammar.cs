using System;
using System.Collections.Generic;
using System.Linq;
namespace YAYACC
{
    public class Grammar
    {
        public List<Variable> Variables { get; set; } //convertir a Diccionario que tenga como llave el nombre
        public Variable InitVar { get; set; } //Setear este valor
        public List<char> Terminals { get; set; } //Llenar esta lista
        //.y Parser
        List<Action[]> _ParseTableActions;
        Dictionary<int, int[]> _ParseTableGOTO;
        int VarQty;
        int TermQty;
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
        public void BuildParser()
        {
            VarQty = Variables.Count;
            TermQty = Terminals.Count;

            _ParseTableActions = new List<Action[]>();
            _ParseTableGOTO = new Dictionary<int, int[]>();

            //Preparación para regla de gramática aumentada
            Rule AugGrammarRule = new Rule();
            AugGrammarRule.Variable = InitVar.Name + "'";
            Token InitVarToken = new Token();
            InitVarToken.Tag = TokenType.Variable;

            //Creación de regla de gramática aumentada
            InitVarToken.Value = InitVar.Name;
            AugGrammarRule.Production.Add(InitVarToken);

            //Crear estado 0
            StateItem kernel = new StateItem()
            {
                rule = AugGrammarRule,
                pointIndex = 0,
                Lookahead = new List<char>() { (char)0 }
            };
            newState(kernel);
        }
        public void newState(StateItem kernelItem)
        {
            List<StateItem> items = new List<StateItem>();
            //Closure
            
        }
        public void Closure(StateItem kernelItem, List<StateItem> items)
        {
            Rule kernelRule = kernelItem.rule;
            int pointInd = kernelItem.pointIndex;
            if (pointInd < kernelRule.Production.Count)
            {
                Token actualSymbol = kernelRule.Production[pointInd];
                if (actualSymbol.Tag == TokenType.Variable)
                {
                    Variable variable = Variables.G
                }
            }
            else
            {
                if (rule.Variable == InitVar.Name + "'") //accept
                {

                }
            }
        }
    }
}