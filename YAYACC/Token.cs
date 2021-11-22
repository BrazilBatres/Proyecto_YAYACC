namespace YAYACC
{
    public struct Token
    {
        public TokenType Tag;
        public string Value;

        public int CompareTo(object _object)
        {
            Token _token = (Token)_object;

            if (_token.Tag == Tag && _token.Value == Value)
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