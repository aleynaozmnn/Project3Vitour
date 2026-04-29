using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Project3Vitour.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult Change(string culture)
        {
            Response.Cookies.Append(
                //Asp.net core'un dil ayarlarını okumak için baktığı standart çerez ismidir
                CookieRequestCultureProvider.DefaultCookieName,
                //Seçilen dili sistemin anlayacağı özel bir format haline getirir
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),

                //AddYears(1)->Bu dil tercihi kullanıcı tarayıcısında 1 yıl saklanır.Siteyi aç/kapa yapıp tekrar geldiğinde hala seçtiği dilde açılır
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            //Referer->Tarayıcının gönderdiği bir başlıktır.Kullanıcının dil değiştirme isteğinin hangi sayfadan gönderdiğini söyler.
            string returnUrl = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "AdminTour");
            }
            return Redirect(returnUrl);
        }
    }
}