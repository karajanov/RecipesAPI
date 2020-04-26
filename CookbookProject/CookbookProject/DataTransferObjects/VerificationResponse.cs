namespace CookbookProject.DataTransferObjects
{
    public class VerificationResponse
    {
        public bool IsValid { get; set; } = false;

        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
