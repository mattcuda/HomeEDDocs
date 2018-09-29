using System;
using System.Collections.Generic;
using System.Text;

namespace BOL
{
    public class RestError : CudaAppError
    {

        public RestError(string errorMessage, string description) : base(errorMessage, description, ErrorCode.unknown)
        {

        }

    }
}
