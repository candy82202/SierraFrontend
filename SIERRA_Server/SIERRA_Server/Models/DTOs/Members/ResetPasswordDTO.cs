namespace SIERRA_Server.Models.DTOs.Members
{
	public class ResetPasswordDTO
	{
        public int MemberId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmCode { get; set; }
    }
}
