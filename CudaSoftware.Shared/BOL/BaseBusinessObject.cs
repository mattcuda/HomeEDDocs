using System;
using System.Collections.Generic;
using System.Text;

namespace BOL
{
    public class BaseBusinessObject
    {

        private string _errorMessage = string.Empty;

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
            }
        }

    }
}
