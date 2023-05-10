using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApi.Data.Dtos;
using NoteApi.Data.Repositories.NoteReppository;
using NoteApi.Model;
using NoteApi.Model.Dtos;
using NoteApi.Utilities;

namespace NoteApi.Controllers
{
    [ApiController]
    [Route("/api/notes")]
    [Produces("application/json")]
    [Authorize]
    public class NoteController : ControllerBase
    {

        private readonly INoteRepository _repository;
        private readonly IMapper _mapper;

        public NoteController(INoteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NoteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAll()
        {
            User user = GetCurrentUserService.GetCurrentUser(HttpContext);

            IEnumerable<Note> notes = await _repository.GetAll(n => n.UserId == user.Id);
            
            return Ok(_mapper.Map<IEnumerable<NoteDto>>(notes));
        }

        [HttpGet("{id}", Name = "GetNoteById")]
        [ProducesResponseType(typeof(NoteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoteDto>> GetById(int id)
        {
            if (id == 0) return BadRequest();

            User user = GetCurrentUserService.GetCurrentUser(HttpContext);

            Note? note = await _repository.GetById(id);

            if (note == null)
            {
                return NotFound();
            }

            if (note.UserId != user.Id)
            {
                return Forbid();
            }

            return Ok(_mapper.Map<NoteDto>(note));
        }

        [HttpPost]
        [ProducesResponseType(typeof(NoteDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Post([FromBody] NoteCreateDto noteCreateDto)
        {
            if (noteCreateDto == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = GetCurrentUserService.GetCurrentUser(HttpContext);

            Note note = _mapper.Map<Note>(noteCreateDto);
            note.UserId = user.Id;
            note.DateCreated = DateTime.Now;

            await _repository.Insert(note);

            return Created("GetNoteById", _mapper.Map<NoteDto>(note));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] NoteUpdateDto noteUpdateDto)
        {
            if (id != noteUpdateDto.Id) return BadRequest();

            if (noteUpdateDto == null)
            {
                return BadRequest();
            }

            User user = GetCurrentUserService.GetCurrentUser(HttpContext);

            Note? note = await _repository.GetById(n => n.Id == id);

            if (note == null) return NotFound();

            if (note.UserId != user.Id)
            {
                return Forbid();
            }

            Note noteUpdated = _mapper.Map<Note>(noteUpdateDto);
            noteUpdated.UserId = user.Id;

            await _repository.Update(noteUpdated);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            User user = GetCurrentUserService.GetCurrentUser(HttpContext);

            Note? note = await _repository.GetById(id);

            if (note == null) return NotFound();

            if (note.UserId != user.Id)
            {
                return Forbid();
            }

            await _repository.Delete(note);

            return NoContent();
        }

        [HttpGet("dateCreated/desc")]
        [ProducesResponseType(typeof(IEnumerable<NoteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotesDateCreatedDesc()
        {
            User user = GetCurrentUserService.GetCurrentUser(HttpContext);

            IEnumerable<Note> notes = await _repository.GetNotesDateCreatedDesc(n => n.UserId == user.Id);

            return Ok(_mapper.Map<NoteDto>(notes));
        }

        [HttpGet("dateCreated/asc")]
        [ProducesResponseType(typeof(IEnumerable<NoteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotesDateCreatedAsc()
        {
            User user = GetCurrentUserService.GetCurrentUser(HttpContext);

            IEnumerable<Note> notes = await _repository.GetNotesDateCreatedAsc(n => n.UserId == user.Id);

            return Ok(_mapper.Map<NoteDto>(notes));
        }
    }
}
