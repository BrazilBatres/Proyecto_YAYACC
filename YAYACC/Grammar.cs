using System;
using System.Collections.Generic;
using System.Linq;
namespace YAYACC
{
    public class Grammar
    {
        public Dictionary<string,Variable> Variables { get; set; }
        //Dictionary<int, State> LALRstates;
        public Variable InitVar { get; set; }
        public List<char> Terminals { get; set; } //Llenar esta lista
        public ParseTable parseTable { get; set; }
        //.y Parser
        Dictionary<int, State> _states;
        List<int> ToRemoveStates;
        int VarQty;
        int TermQty;
        int _statesQty = -1;
        public void Print()
        {
            Console.WriteLine("-------------------------------------------");            
            foreach (var item1 in Variables)    
            {
                Variable _actualVar = item1.Value;
                Console.WriteLine("Variable " + _actualVar.Name + ":");
                for (int i = 0; i < _actualVar.Rules.Count; i++)
                {
                    Console.Write("Production " + (i + 1) + ": ");
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
            _states = new Dictionary<int, State>();

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
            FromCLRtoLALR();
            List<State> statesToSend = _states.Values.ToList();
            parseTable = new ParseTable(statesToSend, Terminals, Variables);
            parseTable.GenerateTable();
        }
        public void FromCLRtoLALR()
        {
            ToRemoveStates = new List<int>();
            for (int i = 0; i < _states.Count; i++)
            {
                for (int j = i + 1; j < _states.Count; j++)
                {
                    if (!ToRemoveStates.Contains(i) && !ToRemoveStates.Contains(j))
                    {
                        if (_states[i].CompareToState(_states[j], true) == 0)//si encuentra estado con mismo CORE
                        {

                            UnifyStates(i, j);
                            break;
                        }
                    }
                }
            }
            foreach (var item in ToRemoveStates)
            {
                _states.Remove(item);
            }
        }
        public void UnifyStates(int stateID1, int stateID2)
        {
            //Unificar lookaheads 
            for (int i = 0; i < _states[stateID1].items.Count; i++)
            {
                for (int j = 0; j < _states[stateID2].items.Count; j++)
                {
                    if (_states[stateID1].items[i].CompareTo(_states[stateID2].items[j]) == 0)//cuando encuentre su item gemelo
                    {
                        foreach (var lkhd in _states[stateID2].items[j].Lookahead)
                        {
                            if (!_states[stateID1].items[i].Lookahead.Contains(lkhd))//si el lookahead no está ya entre sus lookahead
                            {
                                _states[stateID1].items[i].Lookahead.Add(lkhd);
                            }
                        }
                        break;
                    }
                }
            }
            //Redirigir sucesores que llegan a state2 para que lleguen a state1
            foreach (var actualstate in _states)
            {
                List<int> auxval = actualstate.Value.Successors.Values.ToList();
                List<Token> auxkey = actualstate.Value.Successors.Keys.ToList();
                for (int i = 0; i < auxval.Count; i++)
                {
                    if (auxval[i] == stateID2)
                    {
                        actualstate.Value.Successors[auxkey[i]] = stateID1;
                    }
                }
            }
            //Eliminar state2
            ToRemoveStates.Add(stateID2);
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
            _statesQty++;
            _states.Add(_statesQty,new_state);
            return _statesQty;
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
                    bool istOK = Variables.TryGetValue(actualSymbol.Value, out Variable currentVar);
                    if (!istOK)
                    {
                        throw new Exception("The variable \"" + actualSymbol.Value + "\" does not exist");
                    }
                    foreach (var item in currentVar.Rules)//Por cada regla de currentVar
                    {
                        StateItem newItem = new StateItem()
                        {
                            ruleProduction = item,
                            nameVariable = actualSymbol.Value,
                            pointIndex = 0,
                            Lookahead = new List<char>()
                        };
                        newItem.Lookahead.AddRange(Lookahead);
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
        public void ParserGrammar(string word)
        {
            string completeWord = word + (char)1;
            if (!Terminals.Contains((char)32))
            {
                completeWord = completeWord.Replace(" ","");
            }
            Stack<int> _Statestack = new Stack<int>();
            Stack<string> _stack = new Stack<string>();

            bool accept = false;            
            _Statestack.Push(0);
            _stack.Push("#");

            List<Action[]> Actions = parseTable.Actions;
            Dictionary<int, int[]> GOTO = parseTable.GOTO;            
            while (!accept)
            {
                bool useEpsilon = false;
                int CurrentState = _Statestack.Peek();
                string toRead = completeWord.Substring(0, 1);
                Action[] stateActions = Actions[CurrentState];
                int index = Terminals.IndexOf(Convert.ToChar(toRead));

                if (index != -1)
                {
                    Action _actualAction;
                    try
                    {
                        _actualAction = stateActions[index];
                        if (_actualAction.action == (char)0)
                        {
                            int indexAux = Terminals.IndexOf((char)0);
                            _actualAction = stateActions[indexAux];
                            if (_actualAction.action == (char)0)
                            {
                                throw new Exception("SYNTAX ERROR: Terminal \"" + toRead + "\" could not Shift or Reduce");
                            }
                            useEpsilon = true;
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("SYNTAX ERROR: Terminal \"" + toRead + "\" could not Shift or Reduce");
                    }                    
                    if (_actualAction.action == 'S')
                    {
                        _Statestack.Push(_actualAction.num);
                        if (!useEpsilon)
                        {
                            _stack.Push(toRead);
                            completeWord = completeWord.Remove(0, 1);
                        }                                                
                    }
                    else if (_actualAction.action == 'R')
                    {
                        if (_actualAction.num == -1)
                        {
                            accept = true;
                        }
                        else
                        {
                            List<Token> _production = parseTable._numberedRules[_actualAction.num];
                            bool toDo = true;
                            if (_production.Count == 1)
                            {
                                if (_production[0].Value == "")
                                {
                                    toDo = false;
                                }
                            }
                            if (toDo)
                            {
                                for (int i = 0; i < _production.Count; i++)
                                {
                                    _stack.Pop();
                                    _Statestack.Pop();
                                }
                            }
                            else
                            {
                                for (int i = 0; i < _production.Count; i++)
                                {
                                    _Statestack.Pop();
                                }
                            }                            

                            _stack.Push(parseTable._correspondingVariable[_actualAction.num]);
                            //GOTO
                            int Aux = _Statestack.Peek();
                            bool okGoto = GOTO.TryGetValue(Aux, out int[] _goto);
                            if (!okGoto)
                            {
                                throw new Exception("SYNTAX ERROR: State \"" + Aux + "\" doesn't have a GOTO with the Variable \"" + parseTable._correspondingVariable[index] + "\"");
                            }

                            int _index = 0;
                            foreach (var item in parseTable._variables)
                            {
                                if (item.Key == _stack.Peek())
                                {
                                    break;
                                }
                                _index++;
                            }
                            _Statestack.Push(_goto[_index]);
                        }                        
                    }
                }
                else
                {                    
                    throw new Exception("LEXERROR: Terminal \"" + toRead + "\" doesn't exist in the Grammar");
                }
            }
        }
    }
}