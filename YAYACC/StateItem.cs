using System.Collections.Generic;
using System.Linq;
namespace YAYACC
{
    public struct StateItem
    {
        public string nameVariable;
        public List<Token> ruleProduction;
        public int pointIndex;
        public List<char> Lookahead;

        public int CompareTo(object _object)
        {
            StateItem _stateItem = (StateItem)_object;

            if ((Enumerable.SequenceEqual(ruleProduction,_stateItem.ruleProduction)) && (pointIndex == _stateItem.pointIndex) && (nameVariable == _stateItem.nameVariable))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}