namespace CookbookProject.Services.Verification
{
    public class EmailDetails
    {
        public string FromEmail { get; set; }

        public string ToEmail { get; set; }

        public string FromName { get; set; }

        public string ToName { get; set; }

        public string Subject { get; set; }

        public string PlainText { get; set; }

        public string HtmlContent { get; set; }
    }
}
