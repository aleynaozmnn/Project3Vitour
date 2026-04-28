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
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Geldiği sayfaya geri döner, eğer sayfa bulunamazsa ana sayfaya gider
            string returnUrl = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "AdminTour");
            }
            return Redirect(returnUrl);
        }
    }
}