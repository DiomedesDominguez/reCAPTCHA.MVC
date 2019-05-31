using System.Configuration;

namespace DNMOFT.reCAPTCHA.MVC
{
    internal class RecaptchaKeyHelper
    {
        internal static string ParseKey(string key)
        {
            if (key.StartsWith("[") && key.EndsWith("]"))
                return ConfigurationManager.AppSettings[key.Trim().Substring(1, key.Length - 2)];
            return key;
        }
    }
}
