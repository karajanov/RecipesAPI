namespace CookbookProject.DataTransferObjects
{
    public class RegularStatus
    {
        public bool IsValid { get; set; } = false;

        public string ErrorMessage { get; set; }
    }
}
