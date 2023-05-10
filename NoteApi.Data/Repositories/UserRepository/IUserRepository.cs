using NoteApi.Model;
using NoteApi.Model.Dtos;

namespace NoteApi.Data.Repositories.UserRepository
{
    public interface IUserRepository : IRepositoryGeneric<User>
    {
        Task<User?> Login(UserLoginDto userLoginDto);
    }
}
