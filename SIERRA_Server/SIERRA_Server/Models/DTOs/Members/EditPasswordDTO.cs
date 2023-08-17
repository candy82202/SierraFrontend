namespace SIERRA_Server.Models.DTOs.Members
{
	public class EditPasswordDTO
	{
        public int MemberId { get; set; }
        public string OriginalPassword { get; set; }
		public string NewPassword { get; set; }
	}
}
