using System.Security.Cryptography;
using System.Text;


namespace SIERRA_Server.Models.Infra
{
	public class HashUtility
	{
		private readonly IConfiguration _config;
		public HashUtility(IConfiguration config)
		{
			_config = config;
		}
		public string ToSHA256(string plainText, string salt)
		{
			// ref https://docs.microsoft.com/zh-tw/dotnet/api/system.security.cryptography.sha256?view=net-6.0
			using (var mySHA256 = SHA256.Create())
			{
				var passwordBytes = Encoding.UTF8.GetBytes(salt + plainText);
				var hash = mySHA256.ComputeHash(passwordBytes);
				var sb = new StringBuilder();
				foreach (var b in hash) sb.Append(b.ToString("X2"));

				return sb.ToString();
			}
		}

		public string GetSalt()
		{
			return _config["Salt"];
			//return "Fu1n2BT1am3F1ghTlng";

        }
	}
}
