using System.Security.Cryptography;
using System.Text;

namespace aiProj.Services
{
    public class Cryptographer
    {
        public string GenerateEmailCode()
        {
            char[] chars = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

            Random random = new Random();
            string code = String.Empty;
            for (int i = 0; i < 4; i++)
            {
                code += chars[random.Next(0, 7)];
            }
            return code;
        }

        public string CryptPassword(string password)
        {
            string pas = String.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                pas = BitConverter.ToString(hashValue).Replace("-", "");
            }
            return pas;
        }

        public string DecryptPassword(string password)
        {
            string pas = string.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                pas = BitConverter.ToString(hashValue).Replace("-", "");
            }
            return pas;
        }
    }
}