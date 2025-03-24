using System.Security.Cryptography;
using System.Text;

namespace LoginEkrani
{
    public class Sifrele
    {
        // Şifreyi SHA256 ile hash'leme
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                StringBuilder sb = new StringBuilder();

                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        // Verilen hash ile şifreyi doğrulama
        public bool VerifyPassword(string password, string storedHash)
        {
            // Kullanıcının girdiği şifreyi hash'le
            string hashedInputPassword = HashPassword(password);

            // Hash'leri karşılaştır
            return hashedInputPassword.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
