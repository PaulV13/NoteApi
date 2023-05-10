using NoteApi.Model;
using System.Security.Claims;

namespace NoteApi.Utilities
{
    public static class GetCurrentUserService
    {
        public static User GetCurrentUser(HttpContext context) {
            var identity = context.User?.Identity as ClaimsIdentity;

            var userClaims = identity?.Claims;

            return new User
            {
                Id = Convert.ToInt16(userClaims?.FirstOrDefault(o => o.Type == "Id")?.Value),
            };

        }
    }
}
