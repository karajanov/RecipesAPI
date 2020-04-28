namespace CookbookProject.DataTransferObjects
{
    public class VerificationStatus
    {
        public bool IsValid { get; set; }

        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
