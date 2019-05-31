
# Welcome to reCAPTCHA library for MVC

ReCAPTCHA lets you embed a CAPTCHA in your web pages in order to protect them against spam and other types of automated abuse. Here, we explain how to add reCAPTCHA to your page.

reCAPTCHA for .NET is an open source .NET library that allows developers to easily use Google's reCAPTCHA in MVC web applications.

## Features

The primary features of the library are:

    reCAPTCHA API verion 2.0 Support
    The color theme of the widget. (dark, light)
    The type of CAPTCHA to serve. (image, button)

## Creating a reCAPTCHA Keys

Before you can use reCAPTCHA in your web application, you must first create a reCAPTCHA key (a pair of public and private keys). Creating reCAPTCHA key is very straight-forward. The following are the steps:

* Go to the Google's reCAPTCHA site.
* Click on the Get reCAPTCHA button. You will be required to login with your Google account.
* Enter a label for this reCAPTCHA and the domain of your web application. You can enter more than one domain if you want to.
* Expand Keys under the Adding reCAPTCHA to your site section. Note down your Site Key and Secret Key.

## Installing reCAPTCHA for MVC
### reCAPTCHA Nuget Package

If the Package Manager Console is not visible in your Microsoft Visual Studio IDE, click on the Tools > Library Package Manager > Package Manager Console menu.
```
PM> Install-Package DNMOFT.reCAPTCHA.MVC
```

### How to Set reCAPTCHA Key in Web.config File

In the appSettings section of your web.config file, add the keys as follows:
```xml
<appSettings>
<add name="reCaptchaPublicKey" value="Your site key" />
<add name="reCaptchaPrivateKey" value="Your secret key" />
</appSettings>
```

Note: The appSettings keys are automatically added to your web.config file if you install reCAPTCHA for .NET through Nuget. However, you would still need to provide your own public and private keys in the web.config file of your project.
How to Use reCAPTCHA in MVC Web Application
Add the reCAPTCHA Control to Your MVC View
```csharp
@using reCAPTCHA.MVC
@using (Html.BeginForm())
{
    @Html.Recaptcha(publicKey:"xxxxxxxxxxxxx",theme: CaptchaTheme.Dark,type:CaptchaType.Image, 
            callback: "verifyCallback",expiredCallback:"expiredCallback")

    <!-- OR -->
    @Html.Recaptcha()

    @Html.ValidationMessage("ReCaptcha")
    <input type="submit" value="Register" />
}
```
### reCAPTCHA Callback
```html
<script>
var verifyCallback = function (response) {
    alert("grecaptcha is ready!");
};

var expiredCallback = function () {
    alert("grecaptcha is expired!");
};
</script>
```
### Attributes and render parameters
Parameter | Value | Default | Description
--- | --- | --- | --- 
publicKey | Your sitekey. | | 
theme | Dark, Light | Light | Optional. The color theme of the widget.
type | Image, Button | Image |Optional. The type of CAPTCHA to serve.
callback | | | Optional. Your callback function that's executed when the user submits a successful CAPTCHA response. will be the input for your callback function.
expiredCallback | | | Optional. Your callback function that's executed when the recaptcha response expires and the user needs to solve a new CAPTCHA. Verifying the user's response.

```csharp
public class HomeController : Controller
{
    [HttpPost]
    [CaptchaValidator(
        PrivateKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
        ErrorMessage = "Invalid input captcha.",
        RequiredMessage = "The captcha field is required.")]
    public ActionResult Index(RegisterModel registerModel, bool captchaValid)
    {
        if (ModelState.IsValid)
        {

        }

        return View(registerModel);
    }
}
```

### Attributes parameters for Captcha Validator
Parameter | Default | Description
--- | --- | --- 
PrivateKey |  | Optional. Your site secret key.
ErrorMessage | Incorrect Captcha | Optional. Custom validation message for user incorrect captch input.
RequiredMessage | Captcha field is required. | Optional. The error message that is associated with the validation control. Captcha Validation Using AJAX BeginForm.

```csharp
@using (Ajax.BeginForm("Index", "Home", new AjaxOptions { OnBegin = "onBeginSubmit" }))
{
    @Html.Recaptcha(theme: CaptchaTheme.Dark)
         
    <input type="submit" value="Register" />
}
```
```html
<script>
    var onBeginSubmit = function () {
        var v = grecaptcha.getResponse();
        if (v.length == 0) {
            alert("You can't leave without Captcha.");
            return false;
        }
    };
</script>
```
