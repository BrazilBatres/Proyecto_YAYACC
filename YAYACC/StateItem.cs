using System.Collections.Generic;
namespace YAYACC
{
    public struct StateItem
    {
        public Variable variable;
        public int pointIndex;
        public List<char> Lookahead;
    }
}