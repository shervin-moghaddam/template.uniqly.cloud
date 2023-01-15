namespace template.Models;

public class ProjectSetupClass
{
    public string SQLConnection { get; set; }
    public string CustomerName { get; set; }
    public string CustomerNo { get; set; }
    public string DBName { get; set; }
    public string WebsiteTitle { get; set; }
    public string Version { get; set; }

    public string EncryptionKey { get; set; }
    public string EncryptionKey2 { get; set; }

    public class Settings
    {
        public int IdentityCookieExpiresMinutes { get; set; }
    }
}