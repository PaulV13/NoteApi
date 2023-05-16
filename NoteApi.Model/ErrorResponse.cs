using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApi.Model
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }

    }
}
