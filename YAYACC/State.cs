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
            int i = 0;

            if (_stateItem.items.Count != items.Count)
            {
                return 1;
            }
            foreach (var item in items)
            {
                var item2 = _stateItem.items[i];
                if (item.nameVariable == item2.nameVariable && item.pointIndex == item2.pointIndex && item.ruleProduction == item2.ruleProduction) 
                {
                    return 0;
                }
                i++;
            }
            return 1;
        }
    }
}
