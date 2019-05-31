using System.Web.Mvc;

namespace DNMOFT.reCAPTCHA.MVC
{
    public class CaptchaValidatorAttribute : ActionFilterAttribute
    {
        public string ErrorMessage { get; set; }

        public string RequiredMessage { get; set; }

        public string PrivateKey { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RecaptchaResponse recaptchaResponse = new RecaptchaValidator()
            {
                PrivateKey = string.IsNullOrWhiteSpace(PrivateKey) ? RecaptchaKeyHelper.ParseKey("[reCaptchaPrivateKey]") : PrivateKey,
                RemoteIP = filterContext.HttpContext.Request.UserHostAddress,
                Response = filterContext.HttpContext.Request.Form["g-recaptcha-response"]
            }.Validate();

            if (!recaptchaResponse.IsValid)
            {
                ((Controller)filterContext.Controller).ModelState.AddModelError("ReCaptcha", GetErrorMessage(recaptchaResponse.ErrorCode));
            }

            filterContext.ActionParameters["captchaValid"] = recaptchaResponse.IsValid;
            base.OnActionExecuting(filterContext);
        }

        private string GetErrorMessage(string errorCode)
        {
            string str;
            switch (errorCode)
            {
                case "captcha-required":
                    str = string.IsNullOrWhiteSpace(RequiredMessage) ? "Captcha field is required." : RequiredMessage;
                    break;
                case "missing-input-secret":
                    str = "The secret parameter is missing.";
                    break;
                case "invalid-input-secret":
                    str = "The secret parameter is invalid or malformed.";
                    break;
                case "missing-input-response":
                    str = "The response parameter is missing.";
                    break;
                case "invalid-input-response":
                    str = string.IsNullOrWhiteSpace(ErrorMessage) ? "Incorrect Captcha" : ErrorMessage;
                    break;
                default:
                    str = string.IsNullOrWhiteSpace(ErrorMessage) ? "Incorrect Captcha" : ErrorMessage;
                    break;
            }
            return str;
        }

        public CaptchaValidatorAttribute()
            : base()
        {
        }
    }
}
