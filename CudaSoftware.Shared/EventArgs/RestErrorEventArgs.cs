using BOL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOL.EventArgs
{
    public class RestErrorEventArgs : System.EventArgs 
    {

        private RestError _error;

        public RestError Err
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
            }
        }

        public RestErrorEventArgs(RestError err)
        {
            _error = err;
        }

    }
}
