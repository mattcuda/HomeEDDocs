using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MattCudaPhotography.BOL;
using MattCudaPhotography.BOL.EventArgs;
using RestSharp;
using System.Threading;
using BOL;
using BOL.EventArgs;

namespace DAL.RestConnectors
{
    public class BaseRestConnector
    {

        #region "Delegates"

        public delegate void RESTJsonFetchedEventHandler(object sender, RestEventArgs e);
        public delegate void RESTJsonFetchErrorHandler(object sender, RestErrorEventArgs e);

        #endregion

        #region "Events"

        public event RESTJsonFetchedEventHandler RESTJsonFetched;
        public event RESTJsonFetchErrorHandler RESTJsonError;

        #endregion

        #region "Properties"

        private string _userName = string.Empty;
        private string _password = string.Empty;
        private string _appType = string.Empty;
        private string _osVersion = string.Empty;
        private string _appVersion = string.Empty;
        private string _certificateSubject = string.Empty;
        private string _callbackKey = string.Empty;
        private int _clientRequestTimeout = 30;
        private string _baseUrl = string.Empty;

        protected string UserName
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

        protected string Password
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

        protected string AppType
        {
            get
            {
                return _appType;
            }
            set
            {
                _appType = value;
            }
        }

        protected string OSVersion
        {
            get
            {
                return _osVersion;
            }
            set
            {
                _osVersion = value;
            }
        }

        protected string AppVersion
        {
            get
            {
                return _appVersion;
            }
            set
            {
                _appVersion = value;
            }
        }

        protected string CertificateSubject
        {
            get
            {
                return _certificateSubject;
            }
            set
            {
                _certificateSubject = value;
            }
        }

        protected string CallbackKey
        {
            get
            {
                return _callbackKey;
            }
            set
            {
                _callbackKey = value;
            }
        }

        protected int ClientRequestTimeout
        {
            get
            {
                return _clientRequestTimeout;
            }
            set
            {
                _clientRequestTimeout = value;
            }
        }

        #endregion

        #region "Constructors"



        public BaseRestConnector(string baseUrl)
        {

            if (!baseUrl.EndsWith("/"))
                baseUrl = string.Concat(baseUrl, "/");

            _baseUrl = baseUrl;
        }

        #endregion


        #region "Protected Methods"

        protected virtual void ExecuteRestPost(string baseUrl, string resourceUrl, string jsonToPost, string returnKey)
        {

           
            //needed for SSL
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, errors) => cert.Subject.ToUpper().Contains(_certificateSubject));
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var client = new RestClient(baseUrl);

            client.AddDefaultHeader("CustomAuthType", "CUSTOMFORMS");
            client.AddDefaultHeader("UserName", _userName);
            client.AddDefaultHeader("Password", Utilities.HttpUtility.HtmlEncode(_password));
            client.AddDefaultHeader("OSVersion", _osVersion);
            client.AddDefaultHeader("AppVersion", _appVersion);
            client.AddDefaultHeader("AppType", _appType);
            client.AddDefaultHeader("Content-Type", "application/json");

            var request = new RestRequest();
            request.Method = Method.POST;
            request.Resource = resourceUrl;
            request.RequestFormat = DataFormat.Json;
            request.Timeout = _clientRequestTimeout * 1000;
            request.AddParameter("application/json", jsonToPost, ParameterType.RequestBody);

           client.PostAsync<int>(request, (response, handler) =>
            {

                //check for a timeout first
                if (response.ResponseStatus == ResponseStatus.TimedOut)
                {
                    // request timed out
                    RestError re = new RestError("Error fetching " + _callbackKey, "Request timed out.");
                    re.ReturnKey = _callbackKey;
                    RESTJsonError(this, new RestErrorEventArgs(re));
                    return;
                }

                System.Diagnostics.Debug.WriteLine(response.StatusDescription);
                System.Diagnostics.Debug.WriteLine("id: " + response.Data);
                string xml = response.Content.ToString();


                //handle any known errors
                if (xml.Contains("Server Error"))
                {
                    xml = ""; //blank xml indicates an error ocurred
                    RestError re = new RestError("Error fetching " + _callbackKey, "Unknown server error");
                    re.ReturnKey = _callbackKey;
                    RESTJsonError(this, new RestErrorEventArgs(re));
                }
                else if (xml.Contains("Endpoint not found"))
                {
                    RestError re = new RestError("Error fetching " + _callbackKey, "Unable to establish connection with ECMD");
                    re.ReturnKey = _callbackKey;
                    RESTJsonError(this, new RestErrorEventArgs(re));
                }
                else if (xml.StartsWith("ERROR"))
                {
                    RestError re = new RestError("Error fetching " + _callbackKey, "Data processing error!");
                    re.ReturnKey = _callbackKey;
                    RESTJsonError(this, new RestErrorEventArgs(re));
                }
                else if (xml.Equals(""))
                {
                    RestError re = new RestError("Error fetching " + _callbackKey, "Unable to establish connection with ECMD");
                    re.ReturnKey = _callbackKey;
                    RESTJsonError(this, new RestErrorEventArgs(re));
                }
                else
                {
                    RESTJsonFetched(this, new RestEventArgs(xml, returnKey));
                }


            });

        }

    

     

        protected virtual void RaiseRestError(RestErrorEventArgs re)
        {
            RESTJsonError(this, re);
        }

        /// <summary>
        /// This was dropped into the base class since
        /// many add or submits to the services return the
        /// [ReturnMessage] object.
        /// </summary>
        /// <returns>The return message.</returns>
        /// <param name="json">Raw Json string</param>
        protected virtual ReturnMessage ParseReturnMessage(string json)
        {

            ReturnMessage message = new ReturnMessage();
            message = JsonConvert.DeserializeObject<ReturnMessage>(json);
            return message;

        }

        protected string ParseStringResponse(string json)
        {
            return JsonConvert.DeserializeObject<string>(json);
        }

        #region "RestClient Implementation"

        protected virtual string ExecuteRestGet(string baseUrl, string resourceUrl)
        {

            //needed for SSL
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, errors) => cert.Subject.ToUpper().Contains(_certificateSubject));
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);

            //ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var client = createRestClientObject(baseUrl);

            var request = new RestRequest();
            request.Method = Method.GET;
            request.Resource = resourceUrl;

            IRestResponse response = client.Execute(request);
            string json = response.Content.ToString();
            return json;

        }


        protected virtual void ExecuteRestGetAsynch(string baseUrl, string resourceUrl, List<UrlParameter> parms)
        {

            EventWaitHandle Wait = new AutoResetEvent(false);

            //needed for SSL
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, errors) => cert.Subject.ToUpper().Contains(_certificateSubject));
            //ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var client = createRestClientObject(baseUrl);

            var request = new RestRequest();
            request.Method = Method.GET;
            request.Resource = resourceUrl;
            request.RequestFormat = DataFormat.Json;

            foreach (UrlParameter parm in parms)
                request.AddQueryParameter(parm.Name, parm.NameValue);




            Uri uri = client.BuildUri(request);
            string json = string.Empty;

            client.ExecuteAsync(request, response1 =>
            {
                if (response1.ResponseStatus == ResponseStatus.Completed)
                {
                    json = response1.Content.ToString();
                    RESTJsonFetched(this, new RestEventArgs(json, _callbackKey));
                    Wait.Set();
                }          
            });

           

        }

        #endregion

        #region "WebClient
        //Note that the WebClient appears more reliable for some reason now.
        //It looks like there are some bugs with returning async data in RestClient

        /// <summary>
        /// Not only a different overload but also uses the 
        /// WebClient instead of the REST toolkit.
        /// </summary>
        /// <param name="completeUrl">The entire rest Url</param>
        /// <returns>An xml string</returns>
        protected virtual string ExecuteRestGet(string completeUrl)
        {

            //needed for SSL
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, errors) => cert.Subject.ToUpper().Contains(_certificateSubject));
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);

            if (CanPingServices() == false)
            {
                return "ERROR: A connection could not be established to our servers!  Please check that your device is connected to the internet before continuing";
            }


            WebClient client = createWebClientObject();

            string xml = client.DownloadString(completeUrl);
            System.Diagnostics.Debug.WriteLine(xml);

            return xml;

        }

        /// <summary>
        /// A method for posting serialized data 
        /// to the server.
        /// </summary>
        /// <param name="completeUrl">The complete url to post to</param>
        /// <param name="json">The json payload to post</param>
        protected virtual void ExecuteRestPost(string completeUrl, string json)
        {

            //needed for SSL
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, errors) => cert.Subject.ToUpper().Contains(_certificateSubject));
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);

            if (CanPingServices() == false)
            {
                RestError re = new RestError("Connection Error", "A connection could not be established to our servers!  Please check that your device is connected to the internet before continuing.");
                re.ReturnKey = "CONNECT";
                RestErrorEventArgs evg = new RestErrorEventArgs(re);
                RESTJsonError(this, evg);
                return;
            }

            WebClient client = createWebClientObject();
            client.UploadDataCompleted += Client_UploadDataCompleted;
            byte[] payload = System.Text.Encoding.ASCII.GetBytes(json);

            client.UploadDataAsync(new Uri(completeUrl), "POST", payload);

        }

        private void Client_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string json = System.Text.Encoding.Default.GetString(e.Result);
                RESTJsonFetched(sender, new RestEventArgs(json, _callbackKey));

            }
            else
            {
                RestError re = new RestError("Error fetching " + _callbackKey, e.Error.Message.ToString());
                re.ReturnKey = _callbackKey;
                RestErrorEventArgs evg = new RestErrorEventArgs(re);
                RESTJsonError(sender, evg);
            }
        }

        protected virtual void ExecuteRestGetAsynch(string completeUrl)
        {

            //needed for SSL
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, errors) => cert.Subject.ToUpper().Contains(_certificateSubject));
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);


            if (CanPingServices() == false)
            {
                RestError re = new RestError("Connection Error", "A connection could not be established to our servers!  Please check that your device is connected to the internet before continuing.");
                re.ReturnKey = "CONNECT";
                RestErrorEventArgs evg = new RestErrorEventArgs(re);
                RESTJsonError(this, evg);
                return;
            }

            WebClient client = createWebClientObject();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(completeUrl));

        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            if (e.Error == null)
            {

                RESTJsonFetched(sender, new RestEventArgs(e.Result, _callbackKey));

            }
            else
            {
                RestError re = new RestError("Error fetching " + _callbackKey, e.Error.Message.ToString());
                re.ReturnKey = _callbackKey;
                RestErrorEventArgs evg = new RestErrorEventArgs(re);
                RESTJsonError(sender, evg);
            }

        }


        private WebClient createWebClientObject()
        {

            WebClient client = new WebClient();
            client.Headers.Add("UserName", _userName);
            client.Headers.Add("Password", _password);
            client.Headers.Add("OSVersion", _osVersion);
            client.Headers.Add("AppVersion", _appVersion);
            client.Headers.Add("Content-Type", "application/json");

            return client;

        }


        #endregion


        private RestClient createRestClientObject(string baseUrl)
        {

            var client = new RestClient(baseUrl);

            client.AddDefaultHeader("UserName", _userName);
            client.AddDefaultHeader("Password", _password);
            client.AddDefaultHeader("OSVersion", _osVersion);
            client.AddDefaultHeader("AppVersion", _appVersion);
            client.AddDefaultHeader("Content-Type", "application/json");

            return client;

        }

        protected bool CanPingServices()
        {
            //needed for SSL
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, errors) => cert.Subject.ToUpper().Contains(_certificateSubject));
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);

            var client = new WebClient();
            string json = client.DownloadString(_baseUrl + "CameraSafe/Ping");
     
            ReturnMessage message = ParseReturnMessage(json);
            if (message.Message.StartsWith("100 OK"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // callback used to validate the certificate in an SSL conversation
        private bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        {

            return true;

            //bool result = false;
            //if (cert.Subject.ToUpper().Contains(_certificateSubject))
            //{
            //    result = true;
            //}
            //return result;
        }

        #endregion

    }
}
