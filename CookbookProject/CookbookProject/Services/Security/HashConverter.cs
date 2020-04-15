using System.Security.Cryptography;
using System.Text;

namespace CookbookProject.Services.Security
{
    public class HashConverter : IHashConverter
    {
        private readonly SHA256Managed sha;

        public HashConverter() => sha = new SHA256Managed();

        public byte[] GetHashedPassword(string password)
        {
            var byteFormat = Encoding.ASCII.GetBytes(password);
            var hash = sha.ComputeHash(byteFormat);

            return hash;
        }
    }
}
