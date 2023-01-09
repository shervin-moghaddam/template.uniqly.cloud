using System.Diagnostics;

namespace template.Code.Helpers;

public class LogClass
{
    /// <summary>
    /// Insert log entry in L02_SecurityLog
    /// </summary>
    /// <param name="Text1">Max 4000 chars</param>
    /// <param name="Text2">Unlimited chars</param>
    /// <param name="UserID">Current user logged in</param>
    /// <param name="StatusID">Not used</param>
    /// <param name="Identifier">eg.: assignment_delete</param>
    public static void SecurityLog(string Text1, string Text2, int UserID, int StatusID, string Identifier)
    {
        try
        {
            ExecStoredProcedure("spLog_SecurityLog",
                CreateParam(Text1, "Text1"),
                CreateParam(Text2, "Text2"),
                CreateParam(UserID, "UserID"),
                CreateParam(StatusID, "StatusID"),
                CreateParam(Identifier, "Identifier"));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Insert log entry in L02_SecurityLog
    /// </summary>
    /// <param name="Text1">Max 4000 chars</param>
    /// <param name="Text2">Unlimited chars</param>
    /// <param name="UserID">Current user logged in</param>
    /// <param name="StatusID">Not used</param>
    /// <param name="Identifier">eg.: assignment_delete</param>
    public static void GeneralLog(string Text1, string Text2, int UserID, int StatusID, string Identifier)
    {
        try
        {
            ExecStoredProcedure("spLog_SecurityLog",
                CreateParam(Text1, "Text1"),
                CreateParam(Text2, "Text2"),
                CreateParam(UserID, "UserID"),
                CreateParam(StatusID, "StatusID"),
                CreateParam(Identifier, "Identifier"));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}