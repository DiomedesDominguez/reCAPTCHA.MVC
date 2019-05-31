using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace DNMOFT.reCAPTCHA.MVC
{
    internal class RecaptchaValidator
    {
        private const string VerifyUrl = "https://www.google.com/recaptcha/api/siteverify";
        private string privateKey;
        private string remoteIp;
        private string response;

        public string PrivateKey
        {
            get
            {
                return privateKey;
            }
            set
            {
                privateKey = value;
            }
        }

        public string RemoteIP
        {
            get
            {
                return remoteIp;
            }
            set
            {
                var ipAddress = IPAddress.Parse(value);
                if (ipAddress == null || ipAddress.AddressFamily != AddressFamily.InterNetwork && ipAddress.AddressFamily != AddressFamily.InterNetworkV6)
                    throw new ArgumentException($"Expecting an IP address, got {ipAddress}");
                remoteIp = ipAddress.ToString();
            }
        }

        public string Response
        {
            get
            {
                return response;
            }
            set
            {
                response = value;
            }
        }

        private void CheckNotNull(object obj, string name)
        {
            if (obj == null)
                throw new ArgumentNullException(name);
        }

        public RecaptchaResponse Validate()
        {
            CheckNotNull(PrivateKey, "PrivateKey");
            CheckNotNull(RemoteIP, "RemoteIp");
            if (string.IsNullOrWhiteSpace(response))
                return RecaptchaResponse.CaptchaRequired;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify");
            httpWebRequest.ProtocolVersion = HttpVersion.Version11;
            httpWebRequest.Timeout = 30000;
            httpWebRequest.Method = "POST";
            httpWebRequest.UserAgent = "reCAPTCHA/ASP.NET";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = Encoding.ASCII.GetBytes(string.Format("secret={0}&response={1}&remoteip={2}", HttpUtility.UrlEncode(PrivateKey), HttpUtility.UrlEncode(Response), HttpUtility.UrlEncode(RemoteIP)));
            using (Stream requestStream = httpWebRequest.GetRequestStream())
                requestStream.Write(bytes, 0, bytes.Length);
            var captchaResponse = new CaptchaResponse();
            try
            {
                using (WebResponse response = httpWebRequest.GetResponse())
                {
                    using (var textReader = (TextReader)new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(textReader.ReadToEnd());
                }
            }
            catch
            {
                return RecaptchaResponse.RecaptchaNotReachable;
            }
            if (captchaResponse.Success)
                return RecaptchaResponse.Valid;
            string errorCode = string.Empty;
            if (captchaResponse.ErrorCodes != null)
                errorCode = string.Join(",", captchaResponse.ErrorCodes.ToArray());
            return new RecaptchaResponse(false, errorCode);
        }
    }
}
