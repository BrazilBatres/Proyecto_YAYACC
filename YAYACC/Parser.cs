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

        };
        public void State0()
        {
            switch (_token.Tag)
            {
                case TokenType.Variable:
                    _Statestack.Push(0);
                    Consume();
                    State3();
                    break;
                default:
                    throw new Exception("Syntax Error");
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
        public void State2()
        {
            switch (_token.Tag)
            {
                case TokenType.Variable:
                    _Statestack.Push(2);
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
        public void State3()
        {
            switch (_token.Tag)
            {
                case TokenType.Colon:
                    _Statestack.Push(3);
                    Consume();
                    State5();
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
        public void State5()
        {
            switch (_token.Tag)
            {
                case TokenType.Semicolon:
                case TokenType.Pipe:
                    Reduce(8);
                    break;
                case TokenType.Terminal:
                    _Statestack.Push(5);
                    Consume();
                    State8();
                    break;
                case TokenType.Variable:
                    _Statestack.Push(5);
                    Consume();
                    State7();
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
                        _Statestack.Push(6);
                        Consume();
                        State12();
                        break;
                    case TokenType.Pipe:
                        _Statestack.Push(6);
                        Consume();
                        State11();
                        break;
                    default:
                        throw new Exception("Syntax Error");
                }
            }
            else
            {
                
            }
            
        }
        public void State7()
        {
            switch (_token.Tag)
            {
                case TokenType.Semicolon:
                case TokenType.Pipe:
                    Reduce(8);
                    break;
                case TokenType.Terminal:
                    _Statestack.Push(7);
                    Consume();
                    State8();
                    break;
                case TokenType.Variable:
                    _Statestack.Push(7);
                    Consume();
                    State7();
                    break;
                default:
                    throw new Exception("Syntax Error");
            }
        }
        public void State8()
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
                    State8();
                    break;
                case TokenType.Variable:
                    _Statestack.Push(8);
                    Consume();
                    State7();
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
        public void State11()
        {
            switch (_token.Tag)
            {
                case TokenType.Semicolon:
                case TokenType.Pipe:
                    Reduce(8);
                    break;
                case TokenType.Terminal:
                    _Statestack.Push(11);
                    Consume();
                    State8();
                    break;
                case TokenType.Variable:
                    _Statestack.Push(11);
                    Consume();
                    State7();
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
        public void State15()
        {
            switch (_token.Tag)
            {
                case TokenType.Semicolon:
                    _Statestack.Push(15);
                    Consume();
                    State11();
                    break;
                case TokenType.Pipe:
                    _Statestack.Push(15);
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
            switch (_Statestack.Peek())
            {
                case 0:

                default:
                    break;
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
