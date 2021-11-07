using System.Collections.Generic;
namespace YAYACC
{
    public class Rule
    {
        public int PopQuantity { get; set; }
        public string Variable { get; set; }
        public List<string> Production { get; set; }        
    }
}