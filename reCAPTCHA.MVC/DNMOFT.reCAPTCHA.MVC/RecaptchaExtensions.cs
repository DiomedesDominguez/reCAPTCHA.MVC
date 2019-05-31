using System.Web;
using System.Web.Mvc;

namespace DNMOFT.reCAPTCHA.MVC
{
    public static class RecaptchaExtensions
    {
        public static IHtmlString Recaptcha(this HtmlHelper htmlHelper, string publicKey = "[reCaptchaPublicKey]", string hl = "en", CaptchaTheme theme = CaptchaTheme.Light, CaptchaType type = CaptchaType.Image, string callback = "", string expiredCallback = "")
        {
            var recaptchaHtmlHelper = new RecaptchaHtmlHelper(publicKey, hl, theme, type, callback, expiredCallback);
            return htmlHelper.Raw(recaptchaHtmlHelper.ToString());
        }
    }
}
