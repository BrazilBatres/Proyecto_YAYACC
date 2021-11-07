using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    class Parser
    {
        Scanner _scanner;
        Token _token;
        Stack<string> _stack;
        Stack<int> _Statestack;
        Dictionary<int, Rule> ToReduce = new Dictionary<int, Rule>
        {
            { 1, new Rule { PopQuantity = 2, Variable = "GRAM", Production = new List<string> { "RULE", "GRAM"}}},
            { 2, new Rule { PopQuantity = 1, Variable = "GRAM", Production = new List<string> { "RULE"}}},
            { 3, new Rule { PopQuantity = 4, Variable = "RULE", Production = new List<string> { TokenType.Variable.ToString(), TokenType.Colon.ToString(), "PROD", "RULE"}}},
            { 4, new Rule { PopQuantity = 3, Variable = "gRULE", Production = new List<string>{ TokenType.Pipe.ToString(), "PROD gRULE"}}},
            { 5, new Rule { PopQuantity = 1, Variable = "gRULE", Production = new List<string>{ TokenType.Semicolon.ToString()}}},
            { 6, new Rule { PopQuantity = 2, Variable = "PROD", Production = new List<string> { TokenType.Variable.ToString(), "PROD"}}},
            { 7, new Rule { PopQuantity = 2, Variable = "PROD", Production = new List<string> { TokenType.Terminal.ToString(), "PROD"}}},
            { 8, new Rule { PopQuantity = 1, Variable = "PROD"}}
        };
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
                        break;
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
                        break;
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
                
            }
            
        }
        public void State7(bool IsAction)
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
        public void State8(bool IsAction)
        {
            switch(_token.Tag)
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
            switch (_token.Tag)
            {
                case TokenType.Semicolon:
                    _Statestack.Push(11);
                    Consume();
                    State11(true);
                    break;
                case TokenType.Pipe:
                    _Statestack.Push(12);
                    Consume();
                    State12();
                    break;
                default:
                    throw new Exception("Syntax Error");
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
        public void Consume()
        {
            _stack.Push(_token.Tag.ToString());
            _token = _scanner.GetToken();
        }
        public void Reduce(int ruleNumber)
        {
            //Necesitamos -> cantidad de POPs y Variable a meter, obtenerla con # de regla
            Rule rule = ToReduce[ruleNumber];
            for (int i = 0; i < rule.PopQuantity; i++)
            {
                _stack.Pop();
                _Statestack.Pop();
            }
            _stack.Push(rule.Variable);

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
            _stack.Push("#");
            _token = _scanner.GetToken();
            switch (_token.Tag)
            {
                case TokenType.Colon:
                    break;
                case TokenType.Semicolon:
                    break;
                case TokenType.Pipe:
                    break;
                case TokenType.EOF:
                    break;
                case TokenType.Terminal:
                    break;
                case TokenType.Variable:
                    break;
                default:
                    break;
            }
        }
    }
}
