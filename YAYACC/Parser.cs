using System;
using System.Collections.Generic;
using System.Linq;

namespace YAYACC
{
    class Parser
    {
        Scanner _scanner;
        Token _token;
        Stack<string> _stack;
        Stack<int> _Statestack;
        Stack<Token> _Lexemestack;
        
        readonly Dictionary<int, Rule> ToReduce = new Dictionary<int, Rule>
        {
            { 1, new Rule { PopQuantity = 2, Variable = "GRAM", Production = new List<string> { "GRAM", "RULE"}}},
            { 2, new Rule { PopQuantity = 1, Variable = "GRAM", Production = new List<string> { "RULE"}}},
            { 3, new Rule { PopQuantity = 4, Variable = "RULE", Production = new List<string> { "gRULE", "PROD", TokenType.Colon.ToString(), TokenType.Variable.ToString()}}},
            { 4, new Rule { PopQuantity = 3, Variable = "gRULE", Production = new List<string>{ "gRULE", "PROD", TokenType.Pipe.ToString()}}},
            { 5, new Rule { PopQuantity = 1, Variable = "gRULE", Production = new List<string>{ TokenType.Semicolon.ToString()}}},
            { 6, new Rule { PopQuantity = 2, Variable = "PROD", Production = new List<string> { "PROD", TokenType.Variable.ToString()}}},
            { 7, new Rule { PopQuantity = 2, Variable = "PROD", Production = new List<string> { "PROD", TokenType.Terminal.ToString()}}},
            { 8, new Rule { PopQuantity = 0, Variable = "PROD"}}
        };
        public Grammar grammar = new Grammar();
        List<List<Token>> Auxrules;
        Stack<Token> Auxrule;
        Variable Auxvariable;        
        bool newAuxRule = false;
        bool newAuxRules = false;
        bool isOtherRule = false;
        bool IsInitVar = true;

        #region States
        public void State0(bool IsAction)
        {
            if (IsAction)
            {
                switch (_token.Tag)
                {
                    case TokenType.Variable:
                        _Statestack.Push(3);
                        Consume();
                        State3();
                        break;
                    default:
                        throw new Exception("Syntax Error");
                }
            }
            else //IsGOTO
            {
                switch (_stack.Peek())
                {
                    case "GRAM":
                        _Statestack.Push(1);
                        State1();
                        break;
                    case "RULE":
                        _Statestack.Push(2);
                        State2(true);
                        break;
                    default:
                        throw new Exception("Syntax Error");
                }
            }
        }
        public void State1()
        {
            switch (_token.Tag)
            {
                case TokenType.EOF:
                    //accept
                    break;
                default:
                    throw new Exception("Syntax Error");
            }
        }
        public void State2(bool IsAction)
        {
            if (IsAction)
            {
                switch (_token.Tag)
                {
                    case TokenType.Variable:
                        _Statestack.Push(3);
                        Consume();
                        State3();
                        break;
                    case TokenType.EOF:
                        Reduce(2);
                        break;
                    default:
                        throw new Exception("Syntax Error");
                }
            }
            else //IsGOTO
            {
                switch (_stack.Peek())
                {
                    case "GRAM":
                        _Statestack.Push(4);
                        State4();
                        break;
                    case "RULE":
                        _Statestack.Push(2);
                        State2(true);
                        break;
                    default:
                        throw new Exception("Syntax Error");
                }
            }
        }
        public void State3()
        {
            switch (_token.Tag)
            {
                case TokenType.Colon:
                    _Statestack.Push(5);
                    Consume();
                    State5(true);
                    break;
                default:
                    throw new Exception("Syntax Error");
            }
        }
        public void State4()
        {
            switch (_token.Tag)
            {
                case TokenType.EOF:                    
                    Reduce(1);
                    break;
                default:
                    throw new Exception("Syntax Error");
            }
        }
        public void State5(bool IsAction)
        {
            if (IsAction)
            {
                switch (_token.Tag)
                {
                    case TokenType.Semicolon:
                    case TokenType.Pipe:
                        Reduce(8);
                        break;
                    case TokenType.Terminal:
                        _Statestack.Push(8);
                        Consume();
                        State8(true);
                        break;
                    case TokenType.Variable:
                        _Statestack.Push(7);
                        Consume();
                        State7(true);
                        break;
                    default:
                        throw new Exception("Syntax Error");
                }
            }
            else //IsGOTO
            {
                if (_stack.Peek() == "PROD")
                {
                    _Statestack.Push(6);
                    State6(true);
                }
                else
                {
                    throw new Exception("Syntax Error");
                }

            }
        }
        public void State6(bool IsAction)
        {
            if (IsAction)
            {
                switch (_token.Tag)
                {
                    case TokenType.Semicolon:
                        _Statestack.Push(12);
                        Consume();
                        State12();
                        break;
                    case TokenType.Pipe:
                        _Statestack.Push(11);
                        Consume();
                        State11(true);
                        break;
                    default:
                        throw new Exception("Syntax Error");
                }
            }
            else
            {
                if (_stack.Peek() == "gRULE")
                {
                    _Statestack.Push(10);
                    State10();
                }
                else
                {
                    throw new Exception("Syntax Error");
                }
            }
        }
        public void State7(bool IsAction)
        {
            if (IsAction)
            {
                switch (_token.Tag)
                {
                    case TokenType.Semicolon:
                    case TokenType.Pipe:
                        Reduce(8);
                        break;
                    case TokenType.Terminal:
                        _Statestack.Push(8);
                        Consume();
                        State8(true);
                        break;
                    case TokenType.Variable:
                        _Statestack.Push(7);
                        Consume();
                        State7(true);
                        break;
                    default:
                        throw new Exception("Syntax Error");
                }
            }
            else
            {
                if (_stack.Peek() == "PROD")
                {
                    _Statestack.Push(13);
                    State13();
                }
                else
                {
                    throw new Exception("Syntax Error");
                }
            }
        }
        public void State8(bool IsAction)
        {
            if (IsAction)
            {
                switch (_token.Tag)
                {
                    case TokenType.Semicolon:
                    case TokenType.Pipe:
                        Reduce(8);
                        break;
                    case TokenType.Terminal:
                        _Statestack.Push(8);
                        Consume();
                        State8(true);
                        break;
                    case TokenType.Variable:
                        _Statestack.Push(7);
                        Consume();
                        State7(true);
                        break;
                    default:
                        throw new Exception("Syntax Error");
                }
            }
            else
            {
                if (_stack.Peek() == "PROD")
                {
                    _Statestack.Push(14);
                    State14();
                }
                else
                {
                    throw new Exception("Syntax Error");
                }
            }
        }
        public void State10()
        {
            switch (_token.Tag)
            {
                case TokenType.EOF:
                case TokenType.Variable:
                    Reduce(3);
                    break;
                default:
                    throw new Exception("Syntax Error");
            }
        }
        public void State11(bool IsAction)
        {
            if (IsAction)
            {
                switch (_token.Tag)
                {
                    case TokenType.Semicolon:
                    case TokenType.Pipe:
                        Reduce(8);
                        break;
                    case TokenType.Terminal:
                        _Statestack.Push(8);
                        Consume();
                        State8(true);
                        break;
                    case TokenType.Variable:
                        _Statestack.Push(7);
                        Consume();
                        State7(true);
                        break;
                    default:
                        throw new Exception("Syntax Error");
                }
            }
            else
            {
                if (_stack.Peek() == "PROD")
                {
                    _Statestack.Push(15);
                    State15(true);
                }
                else
                {
                    throw new Exception("Syntax Error");
                }
            }
        }
        public void State12()
        {
            switch (_token.Tag)
            {
                case TokenType.EOF:
                case TokenType.Variable:
                    Reduce(5);
                    break;
                default:
                    throw new Exception("Syntax Error");
            }
        }
        public void State13()
        {
            switch (_token.Tag)
            {
                case TokenType.Semicolon:
                case TokenType.Pipe:
                    Reduce(6);                    
                    break;
                default:
                    throw new Exception("Syntax Error");
            }
        }
        public void State14()
        {
            switch (_token.Tag)
            {
                case TokenType.Semicolon:
                case TokenType.Pipe:                    
                    Reduce(7);
                    break;
                default:
                    throw new Exception("Syntax Error");
            }
        }
        public void State15(bool IsAction)
        {
            if (IsAction)
            {
                switch (_token.Tag)
                {
                    case TokenType.Semicolon:
                        _Statestack.Push(12);
                        Consume();
                        State12();
                        break;
                    case TokenType.Pipe:
                        _Statestack.Push(11);
                        Consume();
                        State11(true);
                        break;
                    default:
                        throw new Exception("Syntax Error");
                }
            }
            else
            {
                if (_stack.Peek() == "gRULE")
                {
                    _Statestack.Push(16);
                    State16();
                }
                else
                {
                    throw new Exception("Syntax Error");
                }
            }
            
        }
        public void State16()
        {
            switch (_token.Tag)
            {
                case TokenType.EOF:
                case TokenType.Variable:
                    Reduce(4);
                    break;
                default:
                    throw new Exception("Syntax Error");
            }
        }
        #endregion

        public void Consume()
        {
            _stack.Push(_token.Tag.ToString());
            if (_stack.Peek() == "Pipe")
            {
                isOtherRule = true;
            }
            if (_token.Value != "\0")
            {
                Token _newToken = new Token
                {
                    Tag = _token.Tag,
                    Value = _token.Value
                };
                _Lexemestack.Push(_newToken);
            }            
            _token = _scanner.GetToken();            
        }
        public void Reduce(int ruleNumber)
        {            
            Rule rule = ToReduce[ruleNumber];
            for (int i = 0; i < rule.PopQuantity; i++)
            {
                string Popped = _stack.Pop();
                if (Popped != rule.Production[i])
                {
                    throw new Exception("Syntax Error");
                }
                _Statestack.Pop();
            }
            _stack.Push(rule.Variable);

            //Construcción de Gramática
            switch (ruleNumber)
            {
                case 3:
                    Token _actualToken = _Lexemestack.Pop();
                    Auxvariable = new Variable
                    {
                        Name = _actualToken.Value
                    };
                    Auxvariable.Rules = Auxrules;
                    newAuxRule = false;
                    newAuxRules = false;

                    if (IsInitVar) //Setear valor de grammar.InitVar
                    {
                        grammar.InitVar = Auxvariable;
                        IsInitVar = false;
                    }

                    if (grammar.Variables == null)
                    {
                        //grammar.Variables = new List<Variable>();
                        grammar.Variables = new Dictionary<string, Variable>();
                    }
                    grammar.AddVariable(Auxvariable);                    
                    break;
                case 5:
                    if (!newAuxRules)
                    {
                        Auxrules = new List<List<Token>>();
                        newAuxRules = true;
                    }
                    AddRule(Auxrule);
                    break;
                case 6:
                case 7:
                    if (isOtherRule)
                    {
                        if (!newAuxRules)
                        {
                            Auxrules = new List<List<Token>>();
                            newAuxRules = true;
                        }
                        AddRule(Auxrule);
                        isOtherRule = false;
                        newAuxRule = false;
                    }
                    if (!newAuxRule)
                    {
                        Auxrule = new Stack<Token>();
                        newAuxRule = true;
                    }
                    Auxrule.Push(_Lexemestack.Pop());
                    if (Auxrule.Peek().Tag == TokenType.Terminal)
                    {
                        AddTerminal(Auxrule.Peek().Value);
                    }                    
                    break;                                    
                default:
                    break;
            }                     
            //GOTO
            switch (_Statestack.Peek())
            {
                case 0:
                    State0(false);
                    break;
                case 2:
                    State2(false);
                    break;
                case 5:
                    State5(false);
                    break;
                case 6:
                    State6(false);
                    break;
                case 7:
                    State7(false);
                    break;
                case 8:
                    State8(false);
                    break;
                case 11:
                    State11(false);
                    break;
                case 15:
                    State15(false);
                    break;
                default:
                    throw new Exception("Syntax Error");
            }
        }

        public void Parse(string path)
        {
            _scanner = new Scanner(path);
            _stack = new Stack<string>();
            _Statestack = new Stack<int>();
            _Lexemestack = new Stack<Token>();
            _stack.Push("#");
            _token = _scanner.GetToken();
            _Statestack.Push(0);
            grammar.Terminals = new List<char>();
            State0(true);

        }

        public void AddRule(Stack<Token> rule)
        {
            List<Token> FixedRules = new List<Token>();
            int _count = rule.Count;
            for (int i = 0; i < _count; i++)
            {                
                FixedRules.Add(rule.Pop());
            }            
            bool exists = false;
            for (int i = 0; i < Auxrules.Count; i++)
            {
                if (Enumerable.SequenceEqual(Auxrules[i],FixedRules))
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                Auxrules.Add(FixedRules);
            }            
        }

        public void AddTerminal(string terminal)
        {
            bool exist = false;
            char[] _char = terminal.ToCharArray();
            for (int i = 0; i < grammar.Terminals.Count; i++)
            {                
                if (grammar.Terminals[i].Equals(_char[0]))
                {
                    exist = true;
                }
            }
            if (!exist)
            {
                grammar.Terminals.Add(_char[0]);
            }
        }
    }
}