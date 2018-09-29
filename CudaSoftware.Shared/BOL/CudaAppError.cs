using System;
using System.Collections.Generic;
using System.Text;

namespace BOL
{
    public class CudaAppError
    {

        private string _errorMessage = string.Empty;
        private string _errorDescription = string.Empty;
        private ErrorCode _currentCode;
        private string _returnKey = string.Empty;

        public enum ErrorCode
        {
            unknown = 1
        }

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

        public string ErrorDescription
        {
            get
            {
                return _errorDescription;
            }
            set
            {
                _errorDescription = value;
            }
        }

        public string ReturnKey
        {
            get
            {
                return _returnKey;
            }
            set
            {
                _returnKey = value;
            }
        }

        public CudaAppError(string errorMessage, string errorDescription, ErrorCode code)
        {

            _errorMessage = errorMessage;
            _errorDescription = errorDescription;
            _currentCode = code;

        }

    }
}
