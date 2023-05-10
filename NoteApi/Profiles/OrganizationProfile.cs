using AutoMapper;
using NoteApi.Data.Dtos;
using NoteApi.Model;
using NoteApi.Model.Dtos;

namespace NoteApi.Profiles
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Note, NoteDto>().ReverseMap();
            CreateMap<Note, NoteUpdateDto>().ReverseMap();
            CreateMap<Note, NoteCreateDto>().ReverseMap();

            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();

        }
    }
}
