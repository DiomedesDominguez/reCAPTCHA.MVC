namespace DNMOFT.reCAPTCHA.MVC
{
    internal class RecaptchaResponse
    {
        public static readonly RecaptchaResponse Valid = new RecaptchaResponse(true, string.Empty);
        public static readonly RecaptchaResponse CaptchaRequired = new RecaptchaResponse(false, "captcha-required");
        public static readonly RecaptchaResponse InvalidCaptcha = new RecaptchaResponse(false, "incorrect-captcha");
        public static readonly RecaptchaResponse RecaptchaNotReachable = new RecaptchaResponse(false, "recaptcha-not-reachable");
        private readonly bool isValid;
        private readonly string errorCode;

        internal RecaptchaResponse(bool isValid, string errorCode)
        {
            this.isValid = isValid;
            this.errorCode = errorCode;
        }

        public bool IsValid
        {
            get
            {
                return isValid;
            }
        }

        public string ErrorCode
        {
            get
            {
                return errorCode;
            }
        }

        public override bool Equals(object obj)
        {
            var recaptchaResponse = (RecaptchaResponse)obj;
            if (recaptchaResponse == null || recaptchaResponse.IsValid != IsValid)
            {
                return false;
            }

            return recaptchaResponse.ErrorCode == ErrorCode;
        }

        public override int GetHashCode()
        {
            return IsValid.GetHashCode() ^ ErrorCode.GetHashCode();
        }
    }
}
