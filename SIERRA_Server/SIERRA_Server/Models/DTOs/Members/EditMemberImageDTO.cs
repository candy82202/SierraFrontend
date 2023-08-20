namespace SIERRA_Server.Models.DTOs.Members
{
    public class EditMemberImageDTO
    {
        public int Id { get; set; }
        public IFormFile UploadFile { get; set; }
    }
}
