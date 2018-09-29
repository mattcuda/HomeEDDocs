using System;
using System.Collections.Generic;
using System.Text;

namespace MattCudaPhotography.BOL.EventArgs
{
    public class RestEventArgs : System.EventArgs
    {

        private string _jsonResponse = string.Empty;
        private string _returnKey = string.Empty;

        public string JsonResponse
        {
            get
            {
                return _jsonResponse;
            }
        }

        public string ReturnKey
        {
            get
            {
                return _returnKey;
            }
        }

        public RestEventArgs(string jsonResponse, string returnKey)
        {
            _jsonResponse = jsonResponse;
            _returnKey = returnKey;
        }

    }
}
