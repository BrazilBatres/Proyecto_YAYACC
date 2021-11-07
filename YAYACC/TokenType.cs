using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    public enum TokenType
    {
        Colon = ':',
        Semicolon = ';',
        Pipe = '|',
        EOF = (char)0,
        Terminal = (char)1,
        Variable = (char)2
    }
}
