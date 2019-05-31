using System;
using System.IO;
using System.Web.UI;

namespace DNMOFT.reCAPTCHA.MVC
{
    internal class RecaptchaHtmlHelper
    {
        public RecaptchaHtmlHelper(string publicKey, string hl, CaptchaTheme theme, CaptchaType type, string callback, string expiredCallback)
        {
            PublicKey = RecaptchaKeyHelper.ParseKey(publicKey);
            Hl = hl;
            Theme = theme;
            Type = type;
            Callback = callback;
            ExpiredCallback = expiredCallback;
            if (string.IsNullOrEmpty(publicKey))
                throw new InvalidOperationException("Public key cannot be null or empty.");
        }

        public string PublicKey { get; private set; }

        public string PrivateKey { get; private set; }
        public string Hl { get; }
        public CaptchaTheme Theme { get; private set; }

        public CaptchaType Type { get; private set; }

        public string Callback { get; private set; }

        public string ExpiredCallback { get; private set; }

        public override string ToString()
        {
            var stringWriter = new StringWriter();
            using (var htmlTextWriter = new HtmlTextWriter(stringWriter))
            {
                htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Src, $"//www.google.com/recaptcha/api.js?hl={(string.IsNullOrEmpty(Hl) ? "en" : Hl)}");
                htmlTextWriter.AddAttribute("async", (string)null);
                htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Script);
                htmlTextWriter.RenderEndTag();
                htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "g-recaptcha");
                htmlTextWriter.AddAttribute("data-sitekey", PublicKey);
                htmlTextWriter.AddAttribute("data-theme", Theme.ToString().ToLower());
                if (!string.IsNullOrWhiteSpace(Callback))
                {
                    htmlTextWriter.AddAttribute("data-callback", Callback);
                }
                if (!string.IsNullOrWhiteSpace(ExpiredCallback))
                {
                    htmlTextWriter.AddAttribute("data-expired-callback", ExpiredCallback);
                }
                switch (Type)
                {
                    case CaptchaType.Button:
                        htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Button);
                        break;
                    case CaptchaType.Image:
                        htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                        break;
                    default:
                        break;
                }
                htmlTextWriter.RenderEndTag();
            }
            return stringWriter.ToString();
        }
    }
}
