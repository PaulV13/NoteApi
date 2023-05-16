using Microsoft.EntityFrameworkCore;
using NoteApi.Model;
using NoteApi.Model.Dtos;

namespace NoteApi.Data.Repositories.UserRepository
{
    public class UserRespository : RepositoryGeneric<User>, IUserRepository
    {

        public UserRespository(ApplicationDBContext context) : base(context)
        {

        }

        public async Task<User?> Login(UserLoginDto userLoginDto)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(user => user.Email == userLoginDto.Email && user.Password == userLoginDto.Password);

            return user;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }
    }
}
