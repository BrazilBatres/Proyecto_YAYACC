using System.Collections.Generic;
namespace YAYACC
{
    public struct StateItem
    {
        public string nameVariable;
        public List<Token> rule;
        public int pointIndex;
        public List<char> Lookahead;
    }
}