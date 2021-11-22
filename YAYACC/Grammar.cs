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
        public ParseTable parseTable { get; set; }
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
                    Lookahead = new List<char>() { (char)1 } //(char)1 -> #
                }
            };
            GenerateState(_StateItem);
            for (int i = 0; i < _states.Count; i++)
            {
                if (_states[i].Successors.Count == 0)
                {
                    AddSuccessors(_states[i]);
                }
            }
            //AddSuccessors(_states.Last());
            GenerateKernelTable();
            parseTable = new ParseTable(_states, Terminals, Variables);
            parseTable.GenerateTable();
        }
        public int GetKernelSuccessor(State currentstate, StateItem item)
        {
            Token tosearchToken = item.ruleProduction[0];
            int ToReturn = currentstate.Successors[tosearchToken];
            return ToReturn;
        }
        public void GenerateKernelTable()
        {
            //Dictionary<int, Kernel> kernels = new Dictionary<int, Kernel>(); //en esta lista se guarda el índice el kernel dentro del estado al que pertenece
            //int kernelQty = 0;
            ////Obtener cantidad total de kernels
            //for (int i = 0; i < _states.Count; i++)
            //{
            //    for (int j = 0; j < _states[i].items.Count; j++)
            //    {
            //        var item = _states[i].items[j];
            //        if (item.pointIndex != 0) // si es kernel
            //        {
            //            Kernel kernel = new Kernel()
            //            {
            //                item = item,
            //                ToState = GetKernelSuccessor(_states[i], item)
            //            };
            //            kernels.Add(j, kernel);
            //            kernelQty++;
            //        }
            //    }
            //}
            ////inicializar tabla
            //List<List<char>[]> KernelTable = new List<List<char>[]>()
            //{
            //    new List<char>[kernelQty]
            //};
            ////primera columna de la tabla
            //List<char>[] auxColumn = new List<char>[kernelQty];
            ////Colocar $ al lookahead del state 0
            //auxColumn[0] = new List<char>
            //{
            //    (char)1
            //};
            ////llenar primera columna
            //for (int i = 1; i < kernelQty; i++) //omite el estado 0
            //{
            //    if (kernels[i].item.Spontaneous == true)
            //    {
            //        auxColumn[i] = kernels[i].item.Lookahead;
            //        if (kernels[i].item.Lookahead.Count > 1) //si tiene más de un lookahead, omitir el #
            //        {
            //            auxColumn[i].Remove((char)0);
            //        }
            //    }
            //}
            //KernelTable.Add(auxColumn);//Agregar primera columna
            ////
            //int toState;
            //for (int j = 0; j < KernelTable.Count; j++)
            //{
            //    auxColumn = new List<char>[kernelQty];
            //    for (int i = 0; i < kernelQty; i++)
            //    {
            //        if (KernelTable[j][i] != null)
            //        {
            //            int actualState = kernels[i].State;
            //            foreach (var item in _states[actualState].items)
            //            {
            //                if (!item.Spontaneous)
            //                {
            //                    toState = GetKernelSuccessor(_states[actualState], item);
            //                    auxColumn[toState]
            //                }
            //            }
                        
            //        }
            //    }
            //}
            
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
                if (item.CompareToState(new_state, false) == 0)
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
                if (pointInd < item.ruleProduction.Count)
                {
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
                            //Lookahead = new List<char>() { (char)1 }
                            Lookahead = item.Lookahead
                        };
                        Kernels.Add(firstkernel);
                        //buscar otros kernels
                        for (int j = i + 1; j < lastState.items.Count; j++)
                        {
                            var tocheck_item = lastState.items[j];
                            int tocheck_pointInd = tocheck_item.pointIndex;
                            if (tocheck_pointInd < tocheck_item.ruleProduction.Count)
                            {
                                Token tocheck_aftPointToken = tocheck_item.ruleProduction[tocheck_pointInd];
                                if (tocheck_aftPointToken.CompareTo(afterPointToken) == 0) //si tiene el mismo símbolo después del punto, entonces va como kernel al mismo estado
                                { 
                                    StateItem otherkernel = new StateItem()
                                    {
                                        nameVariable = tocheck_item.nameVariable,
                                        pointIndex = tocheck_item.pointIndex + 1,
                                        ruleProduction = tocheck_item.ruleProduction,
                                        //Lookahead = new List<char>() { (char)1 }
                                        Lookahead = tocheck_item.Lookahead
                                    };
                                    Kernels.Add(otherkernel);
                                }
                            }
                            
                        }
                        int SucessorStateID = GenerateState(Kernels);
                        //AddSuccessors(_states.Last());
                        lastState.Successors.Add(afterPointToken, SucessorStateID);
                    }
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
            int Index = pointIndex + 1;

            if (Production.Count == Index) // Si el puntito queda hasta el final de la regla, solo se toma en cuenta el Lookahead
            {
                toReturn = Lookahead;
            }
            else if (Production[Index].Tag == TokenType.Terminal) // Si a la derecha del puntito hay un terminal, entonces solo ese terminal será el Lookahead
            {
                var _char = Production[Index].Value switch
                {
                    "\\\\" => (char)92,
                    "\\n" => (char)10,
                    "\\t" => (char)8,
                    "\\'" => (char)39,
                    "" => (char)0,
                    _ => Convert.ToChar(Production[Index].Value),
                };
                toReturn.Add(_char);
            }
            else if (Production[Index].Tag == TokenType.Variable) // Si a la derecha del puntito hay una variable, entonces se debe calcular su FIRST
            {
                FirstVariable(Production[Index].Value, toReturn, Lookahead);
            }
            return toReturn;
        }
        public void FirstVariable(string variable, List<char> first, List<char> lookahead)
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
                                if (!first.Contains(Convert.ToChar(var.Rules[i][0].Value)))
                                {
                                    var _char = var.Rules[i][0].Value switch
                                    {
                                        "\\\\" => (char)92,
                                        "\\n" => (char)10,
                                        "\\t" => (char)8,
                                        "\\'" => (char)39,
                                        "" => (char)0,
                                        _ => Convert.ToChar(var.Rules[i][0].Value),
                                    };
                                    first.Add(_char);                                   
                                }
                                okFirst = true;
                            }
                        }
                        else if (var.Rules[i][ruleIndex].Tag == TokenType.Variable)
                        {
                            if (var.Name == var.Rules[i][ruleIndex].Value)
                            {
                                okFirst = true;
                            }
                            else
                            {
                                FirstVariable(var.Rules[i][ruleIndex].Value, first, lookahead);
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
                        }
                    }
                }
                if (first.Contains((char)0))
                {
                    first.Remove((char)0);
                    for (int i = 0; i < lookahead.Count; i++)
                    {
                        first.Add(lookahead[i]);
                    }
                    HashSet<char> vs = new HashSet<char>(first);
                    List<char> charList = vs.ToList();
                    first = charList;
                }
            }
            else
            {
                string errorMessage = "The variable \"" + variable +  "\" does not exist";                
                throw new Exception(errorMessage);
            }            
        }
        public void parserGrammar(string word)
        {
            string completeWord = word + "$";
            if (!Terminals.Contains((char)32))
            {
                completeWord.Replace(" ","");
            }
            Stack<int> _Statestack = new Stack<int>();
            Stack<string> _stack = new Stack<string>();

            bool accept = false;            
            _Statestack.Push(0);
            _stack.Push("#");

            List<Action[]> Actions = parseTable.Actions;
            Dictionary<int, int[]> GOTO = parseTable.GOTO;
            while (!false)
            {
                int CurrentState = _Statestack.Peek();
                string toRead = completeWord.Substring(0, 1);
                Action[] stateActions = Actions[CurrentState];
            }
        }
    }
}