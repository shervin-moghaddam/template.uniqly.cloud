using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace template.Controllers;

public class CultureController : Controller
{
    [HttpPost]
    public IActionResult SelectCulture(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return LocalRedirect(returnUrl);
    }
}