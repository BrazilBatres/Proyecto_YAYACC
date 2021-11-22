using System;
using System.Collections.Generic;
using System.Linq;
namespace YAYACC
{
    public class Grammar
    {
        public Dictionary<string,Variable> Variables { get; set; }
        public Variable InitVar { get; set; }
        public List<char> Terminals { get; set; } //Llenar esta lista
        //.y Parser
        List<Action[]> _ParseTableActions;
        Dictionary<int, int[]> _ParseTableGOTO;
        List<State> _states;
        int VarQty;
        int TermQty;
        int _statesQty;
        public void Print()
        {
            Console.WriteLine("-------------------------------------------");            
            foreach (var item1 in Variables)
            {
                Variable _actualVar = item1.Value;
                Console.WriteLine("Variable " + _actualVar.Name + ":");
                for (int i = 0; i < _actualVar.Rules.Count; i++)
                {
                    Console.Write("Regla " + (i + 1) + ": ");
                    for (int j = 0; j < _actualVar.Rules[i].Count; j++)
                    {
                        Console.Write(_actualVar.Rules[i][j].Value + " ");
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
                Variable _actualVar = item.Value;
                if (_actualVar.Name == _var.Name)
                {
                    foreach (var item2 in _var.Rules)
                    {
                        bool AlreadyInRule = false;
                        foreach (var item3 in _actualVar.Rules)
                        {
                            if (Enumerable.SequenceEqual(item2, item3))
                            {
                                AlreadyInRule = true;
                            }
                        }
                        if (!AlreadyInRule)
                        {
                            _actualVar.Rules.Add(item2);
                        }
                    }
                    _inserted = true;
                    break;
                }
            }
            if (!_inserted)
            {
                Variables.Add(_var.Name,_var);
            }
        }
        public void BuildParser()
        {
            VarQty = Variables.Count;
            TermQty = Terminals.Count;

            _ParseTableActions = new List<Action[]>();
            _ParseTableGOTO = new Dictionary<int, int[]>();
            _states = new List<State>();

            //Crear estado 0
            List<StateItem> _StateItem = new List<StateItem>
            {
                new StateItem()
                {
                    nameVariable = InitVar.Name + "'",
                    ruleProduction = new List<Token>
                    {
                        { new Token { Tag = TokenType.Variable, Value = InitVar.Name }}
                    },
                    pointIndex = 0,
                    Lookahead = new List<char>() { (char)0 }
                }
            };
            GenerateState(_StateItem);
        }
        public void GenerateState(List<StateItem> kernelItems)
        {
            State new_state = new State()
            {
                Successors = new Dictionary<Token, int>()
            };
            new_state.items.AddRange(kernelItems);

            //Generar todos los items con cerradura
            for (int i = 0; i < new_state.items.Count; i++)
            {
                Closure(new_state.items[i], new_state.items);
            }
            //Generar transiciones a nuevos estados
            foreach (var item in new_state.items)
            {
                int pointInd = item.pointIndex;
                Token afterPointToken = item.ruleProduction[pointInd];
                if (!new_state.Successors.ContainsKey(afterPointToken))
                {
                    _statesQty++; //solo si no hay un estado con el mismo core
                    new_state.Successors.Add(afterPointToken, _statesQty);
                }
            }
        }
        public void Closure(StateItem kernelItem, List<StateItem> items)
        {
            List<Token> kernelProd = kernelItem.ruleProduction;
            int pointInd = kernelItem.pointIndex;
            if (pointInd < kernelProd.Count) // Si el punto no está al final
            {
                List<char> Lookahead = First(kernelProd, kernelItem.Lookahead, pointInd);
                Token actualSymbol = kernelProd[pointInd]; //Obtener símbolo que está después del punto
                if (actualSymbol.Tag == TokenType.Variable) //si el punto está antes de una variable
                {
                    Variables.TryGetValue(actualSymbol.Value, out Variable currentVar);
                    foreach (var item in currentVar.Rules)//Por cada regla de currentVar
                    {
                        StateItem newItem = new StateItem()
                        {
                            ruleProduction = item,
                            nameVariable = actualSymbol.Value,
                            pointIndex = 0,
                            Lookahead = Lookahead
                        };
                        //verificar que no exista este item
                        bool alreadyExists = false;
                        foreach (var stateItem in items)
                        {
                            if (newItem.CompareTo(stateItem)==0)
                            {
                                alreadyExists = true;
                                foreach (var term in newItem.Lookahead)
                                {
                                    if (!stateItem.Lookahead.Contains(term))
                                    {
                                        stateItem.Lookahead.Add(term);
                                    }
                                }
                            }
                        }
                        if (!alreadyExists)
                        {
                            items.Add(newItem);
                        }
                    }
                }
            }
        }
        public List<char> First(List<Token> Production, List<char> Lookahead, int pointIndex)
        {
            List<char> toReturn = new List<char>();
            int Index = pointIndex++;

            if (Production.Count == Index) // Si el puntito queda hasta el final de la regla, solo se toma en cuenta el Lookahead
            {
                toReturn = Lookahead;
            }
            else if (Production[Index].Tag == TokenType.Terminal) // Si a la derecha del puntito hay un terminal, entonces solo ese terminal será el Lookahead
            {
                char[] terminal = Production[Index].Value.ToCharArray();
                toReturn.Add(terminal[0]);
            }
            else if (Production[Index].Tag == TokenType.Variable)
            {
                FirstVariable(Production[Index].Value, toReturn);
            }
            return toReturn;
        }
        public void FirstVariable(string variable, List<char> first)
        {
            Variable var = Variables[variable];
            for (int i = 0; i < var.Rules.Count; i++)
            {
                bool okFirst = false;
                int ruleIndex = 0;
                List<char> temporalFirst = new List<char>();
                while (!okFirst)
                {
                    if (ruleIndex < var.Rules[i].Count)
                    {
                        if (var.Rules[i][ruleIndex].Tag == TokenType.Terminal)
                        {
                            char[] terminal = var.Rules[i][0].Value.ToCharArray();
                            temporalFirst.Add(terminal[0]);
                            okFirst = true;
                        }
                        else if (var.Rules[i][ruleIndex].Tag == TokenType.Variable)
                        {
                            FirstVariable(var.Rules[i][ruleIndex].Value, first);
                        }
                    }
                    ruleIndex++;
                }

            }
        }
    }
}