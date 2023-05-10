using Microsoft.EntityFrameworkCore;
using NoteApi.Model;

namespace NoteApi.Data
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
    }
}
