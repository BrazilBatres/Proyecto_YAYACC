using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    public struct StateItem
    {
        public Rule rule;
        public int pointIndex;
        public List<char> Lookahead;
    }
}
