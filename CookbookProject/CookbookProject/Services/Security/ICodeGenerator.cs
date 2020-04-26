namespace CookbookProject.Services.Security
{
    public interface ICodeGenerator
    {
        string GetVerificationCode(int range);
    }
}
