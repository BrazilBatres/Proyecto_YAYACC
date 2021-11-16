using System.Collections.Generic;
namespace YAYACC
{
    public class Variable
    {
        public string Name { get; set; }
        public List<List<Token>> Rules { get; set; }
    }
}