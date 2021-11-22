using System;
using System.Collections.Generic;
using System.Linq;
namespace YAYACC
{
    public struct StateItem : IComparable
    {
        public string nameVariable;
        public List<Token> ruleProduction;
        public int pointIndex;
        public List<char> Lookahead;
        //solo para kernels:
        public bool Spontaneous;

        public int CompareTo(object _object)
        {
            StateItem _stateItem = (StateItem)_object;
            bool listEquals = Enumerable.SequenceEqual(ruleProduction, _stateItem.ruleProduction);
            if (listEquals && (pointIndex == _stateItem.pointIndex) && (nameVariable == _stateItem.nameVariable))
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