namespace CookbookProject.Services.EmailClient
{
    public class EmailDetails
    {
        public string FromEmail { get; set; } = "karajanov@hotmail.com";

        public string ToEmail { get; set; }

        public string FromName { get; set; } = "Cookbook Android App";

        public string ToName { get; set; }

        public string Subject { get; set; } = "Verify your account";

        public string PlainText { get; set; }

        public string HtmlContent { get; set; }
    }
}
