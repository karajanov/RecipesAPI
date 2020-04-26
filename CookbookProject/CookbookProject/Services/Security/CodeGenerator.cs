using System.Security.Cryptography;
using System.Text;

namespace CookbookProject.Services.Security
{
    public class CodeGenerator : ICodeGenerator
    {
        private readonly RNGCryptoServiceProvider rng;

        public CodeGenerator() => rng = new RNGCryptoServiceProvider();

        public string GetVerificationCode(int range)
        {
            byte[] code = new byte[range];
            StringBuilder sb = new StringBuilder("");

            rng.GetBytes(code);

            foreach(var b in code)
            {
                sb.Append(b.ToString());
            }

            return sb.ToString();
        }
    }
}
