using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using template.Models;
using template.Resources;
using template.Services;

namespace template.Controllers;

[Authorize]
public class PopupController : Controller
{
    // GET
    [HttpPost]
    public IActionResult GetPopup(string ElementID, int ZIndex, int Width, int Height, string sourceElementIdentifier,
        string Identifier)
    {
        PopupViewModel m = new PopupViewModel
        {
            ElementID = ElementID,
            ZIndex = ZIndex,
            sourceElementIdentifier = sourceElementIdentifier, // e.g. RowID6_CustomerDisplay
            Identifier = Identifier, // e.g. "assignments"

            // 0 is Auto.
            // Above 100 is pixels.
            // Values between 1 and 100 are considered to be percents.
            WidthStyle = Width == 0 ? "auto" : Width <= 100 ? $"{Width}%" : $"{Width}px",
            HeightStyle = Height switch
            {
                0 => "auto",
                <= 100 => $"{Height}%",
                _ => $"{Height}px"
            }
        };

        return View("PopupView", m);
    }
}