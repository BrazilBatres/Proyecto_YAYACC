using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YAYACC
{
    public class State : IComparable
    {
        public List<StateItem> items;

        public Dictionary<Token, int> Successors;
        public State()
        {
            items = new List<StateItem>();
        }

        public int CompareTo(object _object)
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
                        if (item.nameVariable == item2.nameVariable && item.pointIndex == item2.pointIndex && item.ruleProduction == item2.ruleProduction)
                        {
                            found = true;
                            break;
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
