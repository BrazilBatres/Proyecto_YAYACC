using System.Collections.Generic;
namespace YAYACC
{
    public class RuleGrammar
    {
        public int PopQuantity { get; set; }
        public string Variable { get; set; }
        public List<string> Production { get; set; }
    }
}