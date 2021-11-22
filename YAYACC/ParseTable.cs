using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    public class ParseTable
    {
        List<Action[]> Actions = new List<Action[]>();
        Dictionary<int, int[]> GOTO;
        List<State> _states;
        List<char> _terminals;
        Dictionary<string, Variable> _variables;
        List<List<Token>> _numberedRules;
        
        List<string> _correspondingVariable = new List<string>();
        public ParseTable(List<State> states, List<char> Terminals, Dictionary<string,Variable> Variables)
        {
            _states = states;
            _terminals = Terminals;
            _variables = Variables;
            foreach (var item in Variables)
            {
                foreach (var production in item.Value.Rules)
                {
                    _numberedRules.Add(production);
                    _correspondingVariable.Add(item.Value.Name);
                }
            }

        }
        public int GenerateTable()
        {
            foreach (var currentState in _states)
            {
                Action[] result = new Action[_terminals.Count];
                foreach (var sucesor in currentState.Successors)
                {
                    
                    if (sucesor.Key.Value == "Terminal")
                    {
                        InsertShift(sucesor, ref result);
                    }
                    else
                    {
                        InsertGOTO();
                    }
                  
                }
                Actions.Add(result);

                foreach (var item in currentState.items)
                {
                    if (item.pointIndex == item.ruleProduction.Count)
                    {
                        int Key = _numberedRules.IndexOf(item.ruleProduction);
                        InsertReduce();
                    }
                }
            }
        }


        public void InsertReduce(KeyValuePair<Token, int> tag, ref Action[] result, int ruleNum)
        {
            char _char;

            switch (tag.Key.Value)
            {
                case "\\\\":
                    _char = (char)92;
                    break;
                case "\\n":
                    _char = (char)10;
                    break;
                case "\\t":
                    _char = (char)8;
                    break;
                case "\\'":
                    _char = (char)39;
                    break;
                case "":
                    _char = (char)0;
                    break;
                default:
                    _char = Convert.ToChar(tag.Key.Value);
                    break;
            }

            int _index = _terminals.IndexOf(_char);

            Action action = new Action();
            action.action = 'R';
            action.num = ruleNum;
            result[_index] = action;
        }

        public void InsertShift(KeyValuePair<Token, int> tag, ref Action[] result)
        {
            char _char;

            switch (tag.Key.Value)
            {
                case "\\\\":
                    _char = (char)92;
                    break;
                case "\\n":
                    _char = (char)10;
                    break;
                case "\\t":
                    _char = (char)8;
                    break;
                case "\\'":
                    _char = (char)39;
                    break;
                case "":
                    _char = (char)0;
                    break;
                default:
                    _char = Convert.ToChar(tag.Key.Value);
                    break;
            }

            Action action = new Action();
            int _index = _terminals.IndexOf(_char);
            action.action = 'S';
            action.num = tag.Value;
            result[_index] = action;
        }
    }
}
