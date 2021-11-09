using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
namespace YAYACC
{
    public class Scanner
    {
        //colocar dos veces " para que lo reconozca como símbolo en un string
        //colocar @ para que no den error los backslash
        readonly string term_regex = @"^'([a-zA-Z!""#%&()*+,\-./:;<=>?[\]^_{|}~\d ]|(\\\\)|(\\n)|(\\t)|(\\'))'";
        readonly string var_regex = @"^_*[a-zA-Z][a-zA-Z\d_]*";

        public Queue<string> _grammar;
        readonly Regex _Terminal;
        readonly Regex _Variable;
        Match _matcher;
        string currentLine = "";
        public Scanner(string path)
        {
            _grammar = new Queue<string>(File.ReadLines(path));
            _Terminal = new Regex(term_regex);
            _Variable = new Regex(var_regex);
        }

        public Token GetToken()
        {
            Token result = new Token() { Value = ((char)0).ToString() };
            //bool tokenFound = false;
            currentLine = currentLine.TrimStart();
            if (currentLine.Length == 0)
            {
                GetNewLine();
            }
            
            if (_Terminal.IsMatch(currentLine))
            {
                _matcher = _Terminal.Match(currentLine);
                result.Tag = TokenType.Terminal;
                result.Value = _matcher.Value.Replace("'", string.Empty);
                currentLine = currentLine.Remove(0, _matcher.Length);
            }
            else if (_Variable.IsMatch(currentLine))
            {
                _matcher = _Variable.Match(currentLine);
                result.Tag = TokenType.Variable;
                result.Value = _matcher.Value;
                currentLine = currentLine.Remove(0, _matcher.Length);
            }
            else
            {
                char peek = currentLine[0];
                switch (peek)
                {
                    case (char)TokenType.Colon:
                    case (char)TokenType.EOF:
                    case (char)TokenType.Pipe:
                    case (char)TokenType.Semicolon:
                        result.Tag = (TokenType)peek;
                        currentLine = currentLine.Remove(0, 1);
                        break;
                    default:
                        throw new Exception("Lex Error");
                }
            }
            return result;
        }

        private void GetNewLine()
        {
            int count = _grammar.Count;
            if (count > 0)
            {
                currentLine = _grammar.Dequeue().TrimStart();
                if (currentLine.Length == 0)
                {
                    GetNewLine();
                }
                if (count == 1)
                {
                    currentLine += (char)TokenType.EOF;
                }
            }
        }

        public bool IsEmpty()
        {
            if (_grammar.Count == 0 && currentLine.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}