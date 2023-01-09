using System.Diagnostics;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using template.Models;

namespace template.Code.Helpers;

public class SendGridClass
{
    public static async Task<List<SendGridModels.InvalidEmailModel>> GetAllBlocks(string APIKey)
    {
        try
        {
            var headers = new Dictionary<string, string> { { "Accept", "application/json" } };
            var client =
                new SendGridClient(apiKey: APIKey,
                    requestHeaders: headers);

            var response = await client.RequestAsync(
                method: SendGridClient.Method.GET,
                urlPath: "suppression/blocks"
            );

            Console.WriteLine(response.StatusCode);
            return JsonConvert.DeserializeObject<List<SendGridModels.InvalidEmailModel>>(response.Body
                .ReadAsStringAsync().Result);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static async Task<List<SendGridModels.InvalidEmailModel>> GetAllInvalidMails(string APIKey)
    {
        try
        {
            var headers = new Dictionary<string, string> { { "Accept", "application/json" } };
            var client =
                new SendGridClient(apiKey: APIKey,
                    requestHeaders: headers);

            var response = await client.RequestAsync(
                method: SendGridClient.Method.GET,
                urlPath: "suppression/invalid_emails"
            );

            Console.WriteLine(response.StatusCode);
            return JsonConvert.DeserializeObject<List<SendGridModels.InvalidEmailModel>>(response.Body
                .ReadAsStringAsync().Result);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static async Task<List<SendGridModels.InvalidEmailModel>> GetAllBouncess(string APIKey)
    {
        try
        {
            var headers = new Dictionary<string, string> { { "Accept", "application/json" } };
            var client =
                new SendGridClient(apiKey: APIKey,
                    requestHeaders: headers);

            var response = await client.RequestAsync(
                method: SendGridClient.Method.GET,
                urlPath: "suppression/bounces"
            );

            Console.WriteLine(response.StatusCode);
            return JsonConvert.DeserializeObject<List<SendGridModels.InvalidEmailModel>>(response.Body
                .ReadAsStringAsync().Result);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static async Task<MailResponseModel> SendMail(MailSendModel m)
    {
        Response response = null;
        MailResponseModel ResponseModel = null;
        try
        {
            // Client
            SendGridClient Client = new SendGridClient(m.APIKey);

            // Prepare basics
            SendGridMessage msg = new SendGridMessage
            {
                Subject = m.Subject,
                PlainTextContent = m.PlainTextContent,
                HtmlContent = m.HTMLContent
            };

            // Attachments
            if (m.Attachments != null) msg.Attachments = m.Attachments;

            // From
            msg.From = new EmailAddress(m.FromAddress, m.FromName);

            // To
            if (!string.IsNullOrEmpty(m.ToSingleAddress))
                msg.AddTo(new EmailAddress(m.ToSingleAddress, m.ToSingleName));
            else if (m.dicToAddressList.Count > 0) // Multiple recipients
            {
                // Prepare email list
                List<EmailAddress> EmailAddressList = new List<EmailAddress>();

                // Iterate through address list dictionary
                foreach (KeyValuePair<string, string> entry in m.dicToAddressList)
                {
                    EmailAddressList.Add(new EmailAddress(entry.Key, entry.Value));
                }

                // Create to object
                List<Personalization> PersonalizationList = new List<Personalization>();
                PersonalizationList.Add(new Personalization
                {
                    Tos = EmailAddressList
                });

                msg.Personalizations = PersonalizationList;
            }

            // Todo: Check if there are receipients
            response = await Client.SendEmailAsync(msg);

            // Response
            ResponseModel = new MailResponseModel
            {
                StatusCode = response.StatusCode,
                IsSuccess = response.IsSuccessStatusCode,
                response = response
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            ResponseModel = new MailResponseModel
            {
                ex = ex,
                IsSuccess = false
            };
            if (response != null)
            {
                ResponseModel.response = response;
                ResponseModel.StatusCode = response.StatusCode;
            }
        }

        return ResponseModel;
    }
}