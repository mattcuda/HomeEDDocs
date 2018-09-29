using System;
using System.Collections.Generic;
using System.Text;

namespace BOL
{
    public class User : BaseBusinessObject
    {

        private int _id = -1;
        private string _userName = string.Empty;
        private string _password = string.Empty;
        private string _lastKnownDevice = string.Empty;
        private string _softwareVersion = string.Empty;
        private DateTime _tokenCreateDate;
        private string _latitude = string.Empty;
        private string _longitude = string.Empty;
        private bool _isAuthorized = false;
        private string _securityAnswer1 = string.Empty;
        private string _securityAnswer2 = string.Empty;

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public string LastKnownDevice
        {
            get
            {
                return _lastKnownDevice;
            }
            set
            {
                _lastKnownDevice = value;
            }
        }

        public string SoftwareVersion
        {
            get
            {
                return _softwareVersion;
            }
            set
            {
                _softwareVersion = value;
            }
        }

        public DateTime TokenCreateDate
        {
            get
            {
                return _tokenCreateDate;
            }
            set
            {
                _tokenCreateDate = value;
            }
        }

        public string Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
            }
        }

        public string Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
            }
        }

        public bool IsAuthentic
        {
            get
            {
                return _isAuthorized;
            }
            set
            {
                _isAuthorized = value;
            }
        }

        public string SecurityAnswer1
        {
            get
            {
                return _securityAnswer1;
            }
            set
            {
                _securityAnswer1 = value;
            }
        }

        public string SecurityAnswer2
        {
            get
            {
                return _securityAnswer2;
            }
            set
            {
                _securityAnswer2 = value;
            }
        }

    }
}
