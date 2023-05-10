using NoteApi.Model;
using NoteApi.Model.Dtos;
using System.Linq.Expressions;

namespace NoteApi.Data.Repositories.NoteReppository
{
    public interface INoteRepository : IRepositoryGeneric<Note>
    {
        Task<IEnumerable<Note>> GetNotesByTitle(string title);
        Task<IEnumerable<Note>> GetNotesDateCreatedDesc(Expression<Func<Note, bool>> expression);
        Task<IEnumerable<Note>> GetNotesDateCreatedAsc(Expression<Func<Note, bool>> expression);

    }
}
