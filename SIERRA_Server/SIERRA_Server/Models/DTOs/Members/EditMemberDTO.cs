namespace SIERRA_Server.Models.DTOs.Members
{
	public class EditMemberDTO
	{
		public int Id { get; set; }
		public string? Username { get; set; }
		public string? Email { get; set; }
		public string? Address { get; set; }
		public string? Phone { get; set; }
		public DateTime? Birth { get; set; }
		public bool? Gender { get; set; }
	}
}
