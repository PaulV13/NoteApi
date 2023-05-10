using Microsoft.EntityFrameworkCore;
using NoteApi.Model;
using System.Linq.Expressions;

namespace NoteApi.Data.Repositories.NoteReppository
{
    public class NoteRepository : RepositoryGeneric<Note>, INoteRepository
    {

        public NoteRepository(ApplicationDBContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Note>> GetNotesByTitle(string title)
        {
            var notes = await _context.Notes.Where(note => note.Title.ToLower().Contains(title.ToLower())).ToListAsync();

            return notes;
        }

        public async Task<IEnumerable<Note>> GetNotesDateCreatedDesc(Expression<Func<Note, bool>> expression)
        {
            var notes = await _context.Notes.Where(expression).OrderByDescending(note => note.DateCreated).ToListAsync();

            return notes;
        }

        public async Task<IEnumerable<Note>> GetNotesDateCreatedAsc(Expression<Func<Note, bool>> expression)
        {
            var notes = await _context.Notes.OrderBy(note => note.DateCreated).ToListAsync();

            return notes;
        }
    }
}
