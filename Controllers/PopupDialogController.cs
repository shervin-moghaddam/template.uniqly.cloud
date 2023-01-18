using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Newtonsoft.Json;
using template.Code.Helpers;
using template.Models;

namespace template.Controllers;

public class PopupDialogController : Controller
{
    // Return a dialog window from ViewComponent
    // Note: For security reasons, it is not possible to call a completely custom made
    // dialog box, as this can be tampered with.
    // Parameters
    // @dlgNo           : Dialog Identifier no. 
    // @popupElementID  : ID of PopupElement containing the dialog
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> getDlg(int dlgNo, string popupElementID)
    {
        // Create dialog box options from identifier
        PopupDialogModel m = PopupDialogHelperClass.CreateDialogOptions(dlgNo);
        m.PopupElementID = popupElementID;

        // Storing dialog popup html 
        //string PopupDialogHTML = await ViewRender.RenderToStringAsync("/Components/PopupDialog", m);
        string PopupDialogHTML = await RenderViewComponent("PopupDialog", m);

        // Storing list of buttons
        StringBuilder sbButtonList = new StringBuilder();
        foreach (PopupDialogModel.PopupDialogButton btn in m.ButtonList)
        {
            // Append element ID separated with comma (,)
            sbButtonList.Append(sbButtonList.Length > 0
                ? $",{btn.ElementID}"
                : btn.ElementID); // Only apply comma after the first entry
        }

        // Create JSON object
        return Json(JsonConvert.SerializeObject(new PopupDialogModel.JsonResponseModel
        {
            dlg = PopupDialogHTML,
            btnLst = sbButtonList.ToString(),
            dialogNo = dlgNo
        }));
    }

    public async Task<string> RenderViewComponent(string viewComponent, object args)
    {
        IServiceProvider sp = HttpContext.RequestServices;

        DefaultViewComponentHelper ComponentHelper = new DefaultViewComponentHelper(
            sp.GetRequiredService<IViewComponentDescriptorCollectionProvider>(),
            HtmlEncoder.Default,
            sp.GetRequiredService<IViewComponentSelector>(),
            sp.GetRequiredService<IViewComponentInvokerFactory>(),
            sp.GetRequiredService<IViewBufferScope>());

        await using StringWriterHelperClass.ExtentedStringWriter sw =
            new StringWriterHelperClass.ExtentedStringWriter(new StringBuilder(), Encoding.Unicode);
        ViewContext context = new ViewContext(ControllerContext, ViewRenderHelperClass.NullView.Instance, ViewData,
            TempData, sw,
            new HtmlHelperOptions());
        ComponentHelper.Contextualize(context);
        IHtmlContent result = await ComponentHelper.InvokeAsync(viewComponent, args);
        result.WriteTo(sw, HtmlEncoder.Default);
        await sw.FlushAsync();
        return sw.ToString();
    }
}