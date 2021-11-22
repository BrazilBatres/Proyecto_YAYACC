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
                
                foreach (var item in currentState.items)
                {
                    if (item.pointIndex == item.ruleProduction.Count)
                    {
                        int Key = _numberedRules.IndexOf(item.ruleProduction);
                        InsertReduce(item.Lookahead,ref result,Key);
                    }
                }

                Actions.Add(result);
            }
        }


        public void InsertReduce(List<char> Lookahead, ref Action[] result, int ruleNum)
        {
            foreach (var item in Lookahead)
            {
                Action action = new Action();
                action.action = 'R';
                action.num = ruleNum;
                result[item] = action;
            }
        }

        public void InsertShift(KeyValuePair<Token, int> tag, ref Action[] result)
        {
            var _char = tag.Key.Value switch
            {
                "\\\\" => (char)92,
                "\\n" => (char)10,
                "\\t" => (char)8,
                "\\'" => (char)39,
                "" => (char)0,
                _ => Convert.ToChar(tag.Key.Value),
            };
            Action action = new Action();
            int _index = _terminals.IndexOf(_char);
            action.action = 'S';
            action.num = tag.Value;
            result[_index] = action;
        }

        public void InsertGOTO()
        {
            int[] Goto = new int[_variables.Count];
        }
    }
}
