using System.Net;
using SendGrid;

namespace template.Code.Helpers;

public class MailResponseModel
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public Exception ex { get; set; }
    public Response response { get; set; }
}