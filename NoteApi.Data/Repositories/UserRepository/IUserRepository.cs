using NoteApi.Model;
using NoteApi.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApi.Data.Repositories.UserRepository
{
    public interface IUserRepository : IRepositoryGeneric<User>
    {
        Task<User?> Login(UserLoginDto userLoginDto);
    }
}
