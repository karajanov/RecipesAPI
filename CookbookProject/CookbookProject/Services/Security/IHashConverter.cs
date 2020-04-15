namespace CookbookProject.Services.Security
{
    public interface IHashConverter
    {
        byte[] GetHashedPassword(string password);
    }
}
