namespace NoteApi.Model.Dtos
{
    public class NoteDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }
    }
}
