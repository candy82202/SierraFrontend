namespace SIERRA_Server.Models.DTOs.Orders
{
    public class MemberItemDTO
    {
        public string MemberName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime Birth { get; set; }
        public bool Gender { get; set; }
    }
}
