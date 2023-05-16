﻿using System.ComponentModel.DataAnnotations;

namespace NoteApi.Model.Dtos
{
    public class UserCreateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
