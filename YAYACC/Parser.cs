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
        public void State0()
        {
            switch (_token.Tag)
            {
                case TokenType.Variable:
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
                    Consume();
                    State8();
                    break;
                case TokenType.Variable:
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
                        Consume();
                        State12();
                        break;
                    case TokenType.Pipe:
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
                    Consume();
                    State8();
                    break;
                case TokenType.Variable:
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
                    Consume();
                State8();
                break;
                case TokenType.Variable:
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
                    Consume();
                    State8();
                    break;
                case TokenType.Variable:
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
                    Consume();
                    State11();
                    break;
                case TokenType.Pipe:
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
        public void Reduce(int rule)
        {
            //Necesitamos -> cantidad de POPs y Variable a meter, obtenerla con # de regla
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
