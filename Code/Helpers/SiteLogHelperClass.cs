using System.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace template.Code;

public class SiteLogHelperClass
{
    public static void SiteLog(HttpRequest? httprequest, HttpContext? httpcontext, string PageName, string Identifier, string Message, 
        int ActionID = 0)
    {
        if (PageName.ToLower().Contains("mainfest.json")) return;
        string SourceIP = "";
        string Culture = "";
        if (httpcontext != null) SourceIP= httpcontext.Connection.RemoteIpAddress?.ToString();
        if (httprequest != null) Culture = httprequest.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name;
        try
        {
            // Log to database
            ExecQuery(
                "INSERT INTO L10_SiteLog (PageName, Identifier, Message, ActionID, IP, Culture, SessionID, UserAgent) VALUES (@PageName, @Identifier, @Message, @ActionID, @IP, @Culture, @SessionID, @UserAgent)",
                CreateParam(PageName, "@PageName"),
                CreateParam(Identifier, "@Identifier"),
                CreateParam(Message, "@Message"),
                CreateParam(ActionID, "@ActionID"),
                CreateParam(SourceIP, "@IP"),
                CreateParam(Culture, "Culture"),
                CreateParam(httprequest.Cookies.FirstOrDefault().Value, "@SessionID"),
                CreateParam(httprequest.Headers["User-Agent"].ToString(), "@UserAgent"));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            GeneralLog("sitelog", ex.Message, "", 2);
        }
    }
    
}