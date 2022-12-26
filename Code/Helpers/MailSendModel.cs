using SendGrid.Helpers.Mail;

namespace robotmanden.Code.Helpers;

public class MailSendModel
{
    // API key
    public string APIKey { get; set; }
    
    // To 
    public string ToSingleAddress { get; set; }
    public string ToSingleName { get; set; }
    public Dictionary<string, string> dicToAddressList { get; set; } // Email, Name
    
    // From
    public string FromAddress { get; set; }
    public string FromName { get; set; }
    
    // Content
    public string Subject { get; set; }
    public string PlainTextContent { get; set; }
    public string HTMLContent { get; set; }
    public List<Attachment> Attachments { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Filename"></param>
    /// <param name="Base64Content"></param>
    /// <param name="Type">example: "application/pdf" or "image/jpeg"</param>
    /// <param name="ContentID">ID (optional)</param>
    /// <param name="Disposition">"attachment"(default) or "inline"</param>
    public void AddAttachment(string Filename, string Base64Content , string Type = "application/pdf", string? ContentID = null, string Disposition = "attachment")
    {
        Attachments ??= new List<Attachment>();
        Attachments.Add(
            new Attachment
            {
                Content = Base64Content,
                ContentId = ContentID,
                Filename = Filename,
                Type = Type,
                Disposition = Disposition  
            });
    }
}