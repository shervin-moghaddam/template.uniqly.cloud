namespace template.Models;

public enum LogType
{
    Default = 0,
    Confirmation = 1,
    Error = 2,
    Warning = 3
}

public enum SecurityLogStatusCode
{
    Login_Success = 0,
    Login_Failed = 1,
    Login_Invalid = 2,
    Login_ResetPassword = 3,
    Login_LockedUser = 8,
    Login_Logout = 9,
    User_Create = 20,
    User_Update = 21,
    User_Delete = 22
}

public enum GeneralLogStatusCode
{
    Default = 0
}

public class GeneralLogModel
{
    public string Identifier { get; set; } // Unique identifier for the log
    public string Text1 { get; set; }
    public string Text2 { get; set; }
    public string ExceptionMessage { get; set; } // In case of exception
    public string Stacktrace { get; set; } // In case of exception
    public int StatusCode { get; set; } = 0;
    public int Type { get; set; } = 0; // From LogType
    public int UserID { get; set; } // Currently logged in
}

public class SecurityLogModel
{
    public string Identifier { get; set; } // "Login"
    public string Text1 { get; set; }
    public string Text2 { get; set; }
    public string IPAddress { get; set; } // IP address (will be stored encrypted!)
    public int StatusCode { get; set; } = 0; // From SecurityStatusCode
    public int Type { get; set; } = 0; // From LogType
    public int UserID { get; set; } // Currently logged in
}