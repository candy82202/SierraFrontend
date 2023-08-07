namespace SIERRA_Server.Models.DTOs.Members
{
	public class RegisterDTO
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string? EncryptedPassword { get; set; }
		public bool IsConfirmed { get; set; } = false;
		public string? ConfirmCode { get; set; }
	}
}
