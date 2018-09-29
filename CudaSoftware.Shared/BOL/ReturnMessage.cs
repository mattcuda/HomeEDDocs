using System;
using System.Collections.Generic;
using System.Text;

namespace BOL
{
    public enum MessageType
    {
        Information = 0,
        Error = 1,
        Warning = 2
    }

    public class ReturnMessage
    {

        private string _message = string.Empty;
        private MessageType _messType = new MessageType();
        private string _returnValue = string.Empty;
        private string _returnValue2 = string.Empty;
        private string _returnValue3 = string.Empty;
        private string _returnValue4 = string.Empty;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        public MessageType AssignedMessageType
        {
            get
            {
                return _messType;
            }
        }

        public MessageType MessType
        {
            get
            {
                return _messType;
            }
            set
            {
                _messType = value;
            }
        }

        public string ReturnValue
        {
            get
            {
                return _returnValue;
            }
            set
            {
                _returnValue = value;
            }
        }

        public string ReturnValue2
        {
            get
            {
                return _returnValue2;
            }
            set
            {
                _returnValue2 = value;
            }
        }

        public string ReturnValue3
        {
            get
            {
                return _returnValue3;
            }
            set
            {
                _returnValue3 = value;
            }
        }

        public string ReturnValue4
        {
            get
            {
                return _returnValue4;
            }
            set
            {
                _returnValue4 = value;
            }
        }

    }
}
