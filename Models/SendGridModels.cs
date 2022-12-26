namespace robotmanden.Models;
public class SendGridModels
{
    public class InvalidEmailModel
    {
        public int created { get; set; }
        public string email { get; set; }
        public string reason { get; set; }
        public string status { get; set; }
    }
}