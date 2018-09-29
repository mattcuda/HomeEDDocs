using System;
using System.Collections.Generic;
using System.Text;

namespace BOL
{
    public class UrlParameter
    {

        string _name = string.Empty;
        string _nameValue = string.Empty;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string NameValue
        {
            get
            {
                return _nameValue;
            }
            set
            {
                _nameValue = value;
            }
        }

    }
}
