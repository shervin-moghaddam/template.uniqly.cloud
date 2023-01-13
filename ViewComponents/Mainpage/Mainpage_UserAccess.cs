using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using robotmanden.Models;
using robotmanden.Resources;
using template.Models;
using template.Resources;

namespace robotmanden.ViewComponents;

public class Mainpage_UserAccess : ViewComponent
{
    public Mainpage_UserAccess(
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
        return View("~/Views/Shared/Components/Mainpages/Mainpage_UserAccess.cshtml", new UserAccessRightsModel());
    }
}