using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YAYACC
{
    public class State
    {
        public List<StateItem> items;

        public Dictionary<Token, int> Successors;
        public State()
        {
            items = new List<StateItem>();
        }

        public int CompareToState(object _object, bool onlyCore)
        {
            State _stateItem = (State)_object;

            if (_stateItem.items.Count != items.Count)
            {
                return 1;
            }

            bool found = true;
            foreach (var item in items)
            {
                if (found)
                {
                    found = false;
                    foreach (var item2 in _stateItem.items)
                    {
                        if (item.nameVariable == item2.nameVariable && item.pointIndex == item2.pointIndex && Enumerable.SequenceEqual(item.ruleProduction, item2.ruleProduction))//A->Sa A->aS
                        {
                            List<char> Lookahead1 = item.Lookahead.OrderBy(x => x).ToList();
                            List<char> Lookahead2 = item2.Lookahead.OrderBy(x => x).ToList();
                            if (!onlyCore && (Enumerable.SequenceEqual(Lookahead1, Lookahead2)))
                            {
                                found = true;
                                break;
                            }
                            else if (onlyCore)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    return 1;
                }
            }
            if (!found)
            {
                return 1;
            }
            return 0;
            
        }
    }
}
