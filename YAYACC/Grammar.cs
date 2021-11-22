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
            AddSuccessors(_states.Last());
        }
        public int GenerateState(List<StateItem> kernelItems)
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
            //Verificar que no exista un estado igual
            for (int i = 0; i < _states.Count; i++)
            {
                var item = _states[i];
                if (item.CompareTo(new_state) == 0)
                {
                    return i;
                }
            }
            _states.Add(new_state);
            return _statesQty++;
        }
        public void AddSuccessors(State lastState)
        {
            //Generar transiciones a nuevos estados
            for (int i = 0; i < lastState.items.Count; i++)
            {
                var item = lastState.items[i];
                int pointInd = item.pointIndex;
                Token afterPointToken = item.ruleProduction[pointInd];
                if (!lastState.Successors.ContainsKey(afterPointToken))
                {
                    //primer kernel
                    List<StateItem> Kernels = new List<StateItem>();
                    StateItem firstkernel = new StateItem()
                    {
                        nameVariable = item.nameVariable,
                        pointIndex = item.pointIndex + 1,
                        ruleProduction = item.ruleProduction,
                        Lookahead = new List<char>() { (char)0 }
                    };
                    Kernels.Add(firstkernel);
                    //buscar otros kernels
                    for (int j = i + 1; j < lastState.items.Count; j++)
                    {
                        var tocheck_item = lastState.items[j];
                        int tocheck_pointInd = tocheck_item.pointIndex;
                        Token tocheck_aftPointToken = tocheck_item.ruleProduction[tocheck_pointInd];
                        if (tocheck_aftPointToken.CompareTo(afterPointToken) == 0) //si tiene el mismo símbolo después del punto, entonces va como kernel al mismo estado
                        {
                            Kernels.Add(tocheck_item);
                        }
                    }
                    int SucessorStateID = GenerateState(Kernels);
                    AddSuccessors(_states.Last());
                    lastState.Successors.Add(afterPointToken, SucessorStateID);
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
                toReturn.Add(Convert.ToChar(Production[Index].Value));
            }
            else if (Production[Index].Tag == TokenType.Variable) // Si a la derecha del puntito hay una variable, entonces se debe calcular su FIRST
            {
                FirstVariable(Production[Index].Value, toReturn);
            }
            return toReturn;
        }
        public void FirstVariable(string variable, List<char> first)
        {
            bool varExist = Variables.TryGetValue(variable, out Variable var);
            if (varExist)
            {
                for (int i = 0; i < var.Rules.Count; i++)
                {
                    bool okFirst = false;
                    int ruleIndex = 0;

                    while (!okFirst)
                    {
                        if (var.Rules[i][ruleIndex].Tag == TokenType.Terminal)
                        {
                            char[] terminal = var.Rules[i][0].Value.ToCharArray();
                            if (terminal.Length == 0)
                            {
                                first.Add((char)0);
                            }
                            else
                            {
                                if (!first.Contains(terminal[0]))
                                {
                                    first.Add(terminal[0]);
                                }
                            }
                            okFirst = true;
                        }
                        else if (var.Rules[i][ruleIndex].Tag == TokenType.Variable)
                        {
                            if (var.Name == var.Rules[i][ruleIndex].Value)
                            {
                                okFirst = true;
                            }
                            else
                            {
                                FirstVariable(var.Rules[i][ruleIndex].Value, first);
                            }
                        }
                        if (!okFirst)
                        {
                            if (first.Contains((char)0) && ((ruleIndex + 1) < var.Rules[i].Count))
                            {
                                first.Remove((char)0);
                                ruleIndex++;
                            }
                            else
                            {
                                okFirst = true;
                            }
                            //else if(first.Contains((char)0) && ((ruleIndex + 1) == var.Rules[i].Count))
                            //{
                            //    okFirst = true;
                            //}
                            //else
                            //{
                            //    okFirst = true;
                            //}
                        }
                    }
                }
            }
            else
            {
                string errorMessage = "The variable \"" + variable +  "\" does not exist";                
                throw new Exception(errorMessage);
            }            
        }
    }
}