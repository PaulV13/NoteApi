using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApi.Data.Dtos
{
    public class NoteCreateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
