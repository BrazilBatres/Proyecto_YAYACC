using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
