namespace NoteApi.Model
{
    public class User
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Note> Notes { get; } = new List<Note>();
    }
}
