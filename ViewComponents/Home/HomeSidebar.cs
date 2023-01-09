using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.Models;
using template.Resources;

namespace template.ViewComponents;
public class HomeSidebar: ViewComponent
{
    public HomeSidebar(
        IStringLocalizer<HomeResource> localizer,
        UserManager<AccountUserModel> userManager,
        SignInManager<AccountUserModel> signInManager)
    {
        UserManager = userManager;
        SignInManager = signInManager;
        Localizer = localizer;
    }

    private readonly IStringLocalizer<HomeResource> Localizer;
    private readonly UserManager<AccountUserModel> UserManager;
    private readonly SignInManager<AccountUserModel> SignInManager;
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("~/Views/Shared/Components/Home/HomeSidebar.cshtml");
    }
}