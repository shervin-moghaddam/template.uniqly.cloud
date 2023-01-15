using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using template.Models;
using template.Services;
using template.SQL;
using static template.Code.Helpers.SQLHelperClass;
using static template.Code.Helpers.EncryptionHelperClass;

namespace template.Code;

public class LogService
{
    public LogService(
        ProjectSetupClass projectSetup,
        GlobalContainerService GCS,
        SQLService sqlService)
    {
        GlobalContainer = GCS;
        ProjectSetup = projectSetup;
        SQL = sqlService;
        SQL.Logger = this;
    }

    private readonly ProjectSetupClass ProjectSetup;
    private readonly GlobalContainerService GlobalContainer;
    
    private readonly SQLService SQL;

    /// <summary>
    /// This is to match the old version of GeneralLog in SQL helpers
    /// </summary>
    public void GeneralLog(string Text1, string Text2, string ExceptionMessage, int OldLogStatus)
    {
        GeneralLogModel m = new GeneralLogModel();
        m.Type = OldLogStatus switch
        {
            0 => (int)LogType.Default,
            1 => (int)LogType.Confirmation,
            2 => (int)LogType.Error,
            _ => m.Type
        };

        m.Text1 = Text1;
        m.Text2 = Text2;
        m.ExceptionMessage = ExceptionMessage;
        
        GeneralLog(m);
    }

    public void GeneralLog(GeneralLogModel m)
    {
        try
        {
            SQL.ExecStoredProcedure("spLog_GeneralLog",
                CreateParam(m.Identifier, "Identifier"),
                CreateParam(m.Text1, "Text1"),
                CreateParam(m.Text2, "Text2"),
                CreateParam(m.ExceptionMessage, "ExceptionMessage"),
                CreateParam(m.Stacktrace, "Stacktrace"),
                CreateParam(m.StatusCode, "StatusCode"),
                CreateParam(m.Type, "Type"),
                CreateParam(m.UserID, "UserID"));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public void SecurityLog(SecurityLogModel m)
    {
        try
        {
            try
            {
                if (m.IPAddress.Length < 10)
                    m.IPAddress = EncryptString(m.IPAddress, "E546C8DF278CD5931069B522E695D4F2");
            }
            catch (Exception ex)
            {
                m.IPAddress = null;
                Debug.WriteLine($"Error encrypting IP Address in Security log: {ex.Message}");
            }

            SQL.ExecStoredProcedure("spLog_SecurityLog",
                CreateParam(m.Identifier, "Identifier"),
                CreateParam(m.Text1, "Text1"),
                CreateParam(m.Text2, "Text2"),
                CreateParam(m.IPAddress, "IPAddress"),
                CreateParam(m.StatusCode, "StatusCode"),
                CreateParam(m.Type, "Type"),
                CreateParam(m.UserID, "UserID"));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Security log failed: {ex.Message}");
        }
    }
}