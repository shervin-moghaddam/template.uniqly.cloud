using Microsoft.AspNetCore.Html;

namespace template.Code;

public abstract class JSSourceFiles
{
    public static HtmlString GetStartupJS(string SubFolder = "")
    {
        // ~
        string jsFolder = "/js" + (SubFolder == "" ? "/" : $"/{SubFolder}/");
        string ext = "";
        if (SubFolder.ToLower() == "min") ext = "-min"; // If it's in the min folder, add -min to the file name
        
        // A list of the names of JS files to include on startup.
        // NOTE: Remember to put startup in the bottom of the list
        List<string> JSFiles = new List<string>()
        {
            EncapsulateJSListToHTML($"{jsFolder}alert{ext}.js"),
            EncapsulateJSListToHTML($"{jsFolder}startup{ext}.js")
        };
        return new HtmlString(string.Join("\r\n", JSFiles));
    }
    private static string EncapsulateJSListToHTML(string ScriptLink)
    {
        return $"<script src=\"{ScriptLink}\"></script>";
    }

    public static List<string>? GetJSFilesFromFolder(string Folder)
    {
        return null;
    }
}