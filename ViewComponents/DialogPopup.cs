using Microsoft.AspNetCore.Mvc;
using template.Models;

namespace template.Views.Shared.Components;

public class PopupDialog : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(PopupDialogModel m)
    {
        return View("~/Views/Shared/Components/PopupDialog.cshtml", m);
    }
}