using FluentValidation;
using NoteApi.Model.Dtos;

namespace NoteApi.Utilities
{
    public class UserValidator : AbstractValidator<UserCreateDto>
    {
        public UserValidator() 
        { 
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).MinimumLength(8).MaximumLength(16);
        }
    }
}
